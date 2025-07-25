using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using BootcampProject.Repositories.Concrete;
using System.Diagnostics;

namespace BootcampProject.WebAPI.Services
{
    public class DatabaseMigrationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseMigrationService> _logger;
        private readonly IWebHostEnvironment _environment;

        public DatabaseMigrationService(
            IServiceProvider serviceProvider, 
            ILogger<DatabaseMigrationService> logger,
            IWebHostEnvironment environment)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _environment = environment;
        }

        public async Task EnsureDatabaseAsync()
        {
            if (!_environment.IsDevelopment())
            {
                _logger.LogInformation("Migration check skipped - not in development environment.");
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BootcampDbContext>();

            try
            {
                _logger.LogInformation("Starting database migration check...");

                // Check if any migrations exist
                var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

                _logger.LogInformation($"Applied migrations: {appliedMigrations.Count()}");
                _logger.LogInformation($"Pending migrations: {pendingMigrations.Count()}");

                // If no migrations are applied and no pending migrations, create initial migration
                if (!appliedMigrations.Any() && !pendingMigrations.Any())
                {
                    _logger.LogInformation("No migrations found. Creating initial migration...");
                    await CreateInitialMigrationAsync();
                    
                    // Wait a bit for file system to update
                    await Task.Delay(2000);
                    
                    // Refresh migration info after creating
                    pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                }

                // Check if database exists
                var canConnect = await context.Database.CanConnectAsync();
                
                if (!canConnect)
                {
                    _logger.LogInformation("Database does not exist. Creating database...");
                }

                // Apply pending migrations
                if (pendingMigrations.Any())
                {
                    _logger.LogInformation($"Applying {pendingMigrations.Count()} pending migrations...");
                    foreach (var migration in pendingMigrations)
                    {
                        _logger.LogInformation($"Applying migration: {migration}");
                    }
                    await context.Database.MigrateAsync();
                    _logger.LogInformation("All migrations applied successfully.");
                }
                else
                {
                    _logger.LogInformation("Database is up to date. No migrations to apply.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during database migration process.");
                throw;
            }
        }

        private async Task CreateInitialMigrationAsync()
        {
            try
            {
                _logger.LogInformation("Creating initial migration...");
                
                var startInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "ef migrations add InitialCreate --project ../BootcampProject.Repositories --startup-project .",
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = new Process { StartInfo = startInfo };
                process.Start();

                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();

                if (process.ExitCode == 0)
                {
                    _logger.LogInformation("Initial migration created successfully.");
                    _logger.LogInformation($"Migration output: {output}");
                }
                else
                {
                    _logger.LogError($"Failed to create initial migration. Error: {error}");
                    _logger.LogError($"Output: {output}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while creating initial migration.");
            }
        }
    }
} 