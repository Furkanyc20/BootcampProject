using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BootcampProject.Entities.Concrete;

namespace BootcampProject.Repositories.Configurations
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.ToTable("Instructors");
            
            builder.Property(i => i.CompanyName)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}