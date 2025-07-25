using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BootcampProject.Entities.Concrete;

namespace BootcampProject.Repositories.Configurations
{
    public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
    {
        public void Configure(EntityTypeBuilder<Applicant> builder)
        {
            builder.ToTable("Applicants");
            
            builder.Property(a => a.About)
                .HasMaxLength(500);
        }
    }
}