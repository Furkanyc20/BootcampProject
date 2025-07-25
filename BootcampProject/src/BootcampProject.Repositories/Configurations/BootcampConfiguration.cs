using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BootcampProject.Entities.Concrete;

namespace BootcampProject.Repositories.Configurations
{
    public class BootcampConfiguration : IEntityTypeConfiguration<Bootcamp>
    {
        public void Configure(EntityTypeBuilder<Bootcamp> builder)
        {
            builder.ToTable("Bootcamps");
            
            builder.HasKey(b => b.Id);
            
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(b => b.InstructorId)
                .IsRequired();
            
            builder.Property(b => b.StartDate)
                .IsRequired();
            
            builder.Property(b => b.EndDate)
                .IsRequired();
            
            builder.Property(b => b.BootcampState)
                .IsRequired();
            
            builder.HasOne(b => b.Instructor)
                .WithMany()
                .HasForeignKey(b => b.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(b => b.Applications)
                .WithOne(a => a.Bootcamp)
                .HasForeignKey(a => a.BootcampId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}