using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using BootcampProject.Repositories.Concrete;
using BootcampProject.Repositories.Abstract;
using BootcampProject.Business.Abstract;
using BootcampProject.Business.Concrete;
using BootcampProject.Business.BusinessRules;
using BootcampProject.Business.Profiles;
using BootcampProject.Core.Security;
using BootcampProject.Core.Middleware;
using BootcampProject.Repositories.SeedData;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;

// 1. Entity Framework DbContext Configuration
builder.Services.AddDbContext<BootcampDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Repository Registrations (Scoped)
builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IBlacklistRepository, BlacklistRepository>();
builder.Services.AddScoped<IBootcampRepository, BootcampRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();

// 3. Business Rules Registrations (Scoped)
builder.Services.AddScoped<ApplicantBusinessRules>();
builder.Services.AddScoped<ApplicationBusinessRules>();
builder.Services.AddScoped<BlacklistBusinessRules>();
builder.Services.AddScoped<BootcampBusinessRules>();

// 4. Service/Manager Registrations (Scoped)
builder.Services.AddScoped<IApplicantService, ApplicantManager>();
builder.Services.AddScoped<IApplicationService, ApplicationManager>();
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<IBlacklistService, BlacklistManager>();
builder.Services.AddScoped<IBootcampService, BootcampManager>();
builder.Services.AddScoped<IEmployeeService, EmployeeManager>();
builder.Services.AddScoped<IInstructorService, InstructorManager>();

// 5. AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(ApplicantProfile));

// 6. JWT Helper Registration (Scoped)
builder.Services.AddScoped<IJwtHelper, JwtHelper>();

// 7. Authentication & Authorization Configuration
var jwtSettings = configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecurityKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Set to true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// 8. Controllers Configuration
builder.Services.AddControllers();

// 9. Swagger/OpenAPI Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Bootcamp Project API",
        Version = "v1",
        Description = "A comprehensive bootcamp management system API"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// 10. CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 11. Global Exception Handling Middleware Registration
builder.Services.AddScoped<GlobalExceptionHandlingMiddleware>();

// 12. Database Migration Service Registration
builder.Services.AddScoped<BootcampProject.WebAPI.Services.DatabaseMigrationService>();

var app = builder.Build();

// Configure the HTTP request pipeline

// 1. Swagger Configuration (Development only) - EN �NCE!
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(options =>
//    {
//        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Bootcamp Project API v1");
//        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
//        options.DefaultModelsExpandDepth(-1);
//    });
//}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bootcamp Project  API v1");
    c.RoutePrefix = "swagger";
});

// 2. Global Exception Handling
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// 3. HTTPS Redirection
app.UseHttpsRedirection();

// 4. CORS
app.UseCors("AllowAll");

// 5. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 6. Controller Mapping
app.MapControllers();

// Database Migration and Seeding (Development only)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            // Use the migration service for automatic migration handling
            var migrationService = scope.ServiceProvider.GetRequiredService<BootcampProject.WebAPI.Services.DatabaseMigrationService>();
            await migrationService.EnsureDatabaseAsync();
        }
        catch (Exception ex)
        {
            var migrationLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            migrationLogger.LogError(ex, "An error occurred during automatic database migration.");
            throw;
        }
    }
}

// Seed Database (Development only)
if (app.Environment.IsDevelopment())
{
    try
    {
        await app.Services.SeedDataAsync();
    }
    catch (Exception ex)
    {
        var seedLogger = app.Services.GetRequiredService<ILogger<Program>>();
        seedLogger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Application Startup Information
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("BootcampProject API is starting...");
logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);
logger.LogInformation("Swagger UI available at: /swagger");

app.Run();