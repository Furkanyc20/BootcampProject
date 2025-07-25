using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BootcampProject.Entities.Concrete;

namespace BootcampProject.Repositories.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.ToTable("Applications");
            
            builder.HasKey(a => a.Id);
            
            builder.Property(a => a.ApplicantId)
                .IsRequired();
            
            builder.Property(a => a.BootcampId)
                .IsRequired();
            
            builder.Property(a => a.ApplicationState)
                .IsRequired();
            
            builder.HasOne(a => a.Applicant)
                .WithMany()
                .HasForeignKey(a => a.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(a => a.Bootcamp)
                .WithMany(b => b.Applications)
                .HasForeignKey(a => a.BootcampId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}