using Microsoft.EntityFrameworkCore;
using System.Reflection;
using BootcampProject.Entities.Concrete;

namespace BootcampProject.Repositories.Concrete
{
    public class BootcampDbContext : DbContext
    {
        public BootcampDbContext(DbContextOptions<BootcampDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Bootcamp> Bootcamps { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Blacklist> Blacklists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}