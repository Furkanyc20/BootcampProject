using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BootcampProject.Entities.Concrete;
using BootcampProject.Entities.Enums;
using BootcampProject.Core.Utilities;
using BootcampProject.Repositories.Concrete;

namespace BootcampProject.Repositories.SeedData
{
    public static class SeedDataExtensions
    {
        public static async Task SeedDataAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BootcampDbContext>();
            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("SeedDataExtensions");

            try
            {
                logger.LogInformation("Starting database seeding...");

                // Ensure database is created
                await context.Database.EnsureCreatedAsync();

                // Check if data already exists
                if (await context.Users.AnyAsync())
                {
                    logger.LogInformation("Database already contains data. Skipping seed.");
                    return;
                }

                var seedData = await SeedUsersAsync(context, logger);
                await SeedBootcampsAsync(context, seedData, logger);
                await SeedApplicationsAsync(context, seedData, logger);

                await context.SaveChangesAsync();
                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private static async Task<SeedDataContext> SeedUsersAsync(BootcampDbContext context, ILogger logger)
        {
            logger.LogInformation("Seeding users...");

            // Create sample Instructor
            var instructorSalt = HashingHelper.GenerateSalt();
            var instructorHash = HashingHelper.CreateSHA256Hash("instructor123", instructorSalt);
            var instructor = new Instructor
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@bootcamp.com",
                DateOfBirth = new DateTime(1985, 5, 15),
                NationalityIdentity = "12345678901",
                PasswordHash = instructorHash,
                PasswordSalt = instructorSalt,
                CompanyName = "Tech Solutions Inc."
            };

            // Create sample Employee
            var employeeSalt = HashingHelper.GenerateSalt();
            var employeeHash = HashingHelper.CreateSHA256Hash("employee123", employeeSalt);
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Sarah",
                LastName = "Johnson",
                Email = "sarah.johnson@bootcamp.com",
                DateOfBirth = new DateTime(1990, 8, 22),
                NationalityIdentity = "98765432109",
                PasswordHash = employeeHash,
                PasswordSalt = employeeSalt,
                Position = "HR Manager"
            };

            // Create sample Applicants
            var applicant1Salt = HashingHelper.GenerateSalt();
            var applicant1Hash = HashingHelper.CreateSHA256Hash("applicant123", applicant1Salt);
            var applicant1 = new Applicant
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Brown",
                Email = "alice.brown@email.com",
                DateOfBirth = new DateTime(1995, 3, 10),
                NationalityIdentity = "11111111111",
                PasswordHash = applicant1Hash,
                PasswordSalt = applicant1Salt,
                About = "Computer Science graduate looking to enhance my skills in software development."
            };

            var applicant2Salt = HashingHelper.GenerateSalt();
            var applicant2Hash = HashingHelper.CreateSHA256Hash("applicant456", applicant2Salt);
            var applicant2 = new Applicant
            {
                Id = Guid.NewGuid(),
                FirstName = "Michael",
                LastName = "Davis",
                Email = "michael.davis@email.com",
                DateOfBirth = new DateTime(1992, 11, 5),
                NationalityIdentity = "22222222222",
                PasswordHash = applicant2Hash,
                PasswordSalt = applicant2Salt,
                About = "Experienced developer seeking to learn new technologies and frameworks."
            };

            var applicant3Salt = HashingHelper.GenerateSalt();
            var applicant3Hash = HashingHelper.CreateSHA256Hash("applicant789", applicant3Salt);
            var applicant3 = new Applicant
            {
                Id = Guid.NewGuid(),
                FirstName = "Emily",
                LastName = "Wilson",
                Email = "emily.wilson@email.com",
                DateOfBirth = new DateTime(1993, 7, 18),
                NationalityIdentity = "33333333333",
                PasswordHash = applicant3Hash,
                PasswordSalt = applicant3Salt,
                About = "Recent bootcamp graduate looking to continue professional development."
            };

            await context.Instructors.AddAsync(instructor);
            await context.Employees.AddAsync(employee);
            await context.Applicants.AddAsync(applicant1);
            await context.Applicants.AddAsync(applicant2);
            await context.Applicants.AddAsync(applicant3);

            // Return seed data for later use
            var seedData = new SeedDataContext
            {
                InstructorId = instructor.Id,
                EmployeeId = employee.Id,
                Applicant1Id = applicant1.Id,
                Applicant2Id = applicant2.Id,
                Applicant3Id = applicant3.Id
            };

            logger.LogInformation("Users seeded successfully.");
            return seedData;
        }

        private static async Task SeedBootcampsAsync(BootcampDbContext context, SeedDataContext seedData, ILogger logger)
        {
            logger.LogInformation("Seeding bootcamps...");

            var bootcamp1 = new Bootcamp
            {
                Id = Guid.NewGuid(),
                Name = "Full Stack Web Development Bootcamp",
                InstructorId = seedData.InstructorId,
                StartDate = DateTime.Now.AddDays(30),
                EndDate = DateTime.Now.AddDays(120),
                BootcampState = BootcampState.OPEN_FOR_APPLICATION
            };

            var bootcamp2 = new Bootcamp
            {
                Id = Guid.NewGuid(),
                Name = "Data Science and Machine Learning",
                InstructorId = seedData.InstructorId,
                StartDate = DateTime.Now.AddDays(60),
                EndDate = DateTime.Now.AddDays(150),
                BootcampState = BootcampState.PREPARING
            };

            var bootcamp3 = new Bootcamp
            {
                Id = Guid.NewGuid(),
                Name = "Mobile App Development with React Native",
                InstructorId = seedData.InstructorId,
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now.AddDays(30),
                BootcampState = BootcampState.IN_PROGRESS
            };

            await context.Bootcamps.AddAsync(bootcamp1);
            await context.Bootcamps.AddAsync(bootcamp2);
            await context.Bootcamps.AddAsync(bootcamp3);

            // Update seed data with bootcamp IDs
            seedData.Bootcamp1Id = bootcamp1.Id;
            seedData.Bootcamp2Id = bootcamp2.Id;
            seedData.Bootcamp3Id = bootcamp3.Id;

            logger.LogInformation("Bootcamps seeded successfully.");
        }

        private static async Task SeedApplicationsAsync(BootcampDbContext context, SeedDataContext seedData, ILogger logger)
        {
            logger.LogInformation("Seeding applications...");

            var applications = new List<Application>
            {
                new Application
                {
                    Id = Guid.NewGuid(),
                    ApplicantId = seedData.Applicant1Id,
                    BootcampId = seedData.Bootcamp1Id,
                    ApplicationState = ApplicationState.PENDING
                },
                new Application
                {
                    Id = Guid.NewGuid(),
                    ApplicantId = seedData.Applicant2Id,
                    BootcampId = seedData.Bootcamp1Id,
                    ApplicationState = ApplicationState.APPROVED
                },
                new Application
                {
                    Id = Guid.NewGuid(),
                    ApplicantId = seedData.Applicant3Id,
                    BootcampId = seedData.Bootcamp3Id,
                    ApplicationState = ApplicationState.IN_REVIEW
                },
                new Application
                {
                    Id = Guid.NewGuid(),
                    ApplicantId = seedData.Applicant1Id,
                    BootcampId = seedData.Bootcamp2Id,
                    ApplicationState = ApplicationState.REJECTED
                }
            };

            await context.Applications.AddRangeAsync(applications);

            logger.LogInformation("Applications seeded successfully.");
        }
    }

    // Helper class to store seed data IDs during seeding process
    public class SeedDataContext
    {
        public Guid InstructorId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid Applicant1Id { get; set; }
        public Guid Applicant2Id { get; set; }
        public Guid Applicant3Id { get; set; }
        public Guid Bootcamp1Id { get; set; }
        public Guid Bootcamp2Id { get; set; }
        public Guid Bootcamp3Id { get; set; }
    }
}