using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BootcampProject.Entities.Concrete;

namespace BootcampProject.Repositories.Configurations
{
    public class BlacklistConfiguration : IEntityTypeConfiguration<Blacklist>
    {
        public void Configure(EntityTypeBuilder<Blacklist> builder)
        {
            builder.ToTable("Blacklists");
            
            builder.HasKey(bl => bl.Id);
            
            builder.Property(bl => bl.Reason)
                .IsRequired()
                .HasMaxLength(500);
            
            builder.Property(bl => bl.Date)
                .IsRequired();
            
            builder.Property(bl => bl.ApplicantId)
                .IsRequired();
            
            builder.HasOne(bl => bl.Applicant)
                .WithMany()
                .HasForeignKey(bl => bl.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}