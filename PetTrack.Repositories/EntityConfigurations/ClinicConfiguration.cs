using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Entity;

namespace PetTrack.Repositories.EntityConfigurations
{
    public class ClinicConfiguration : IEntityTypeConfiguration<Clinic>
    {
        public void Configure(EntityTypeBuilder<Clinic> builder)
        {
            builder.ToTable("Clinics");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);

            builder.Property(x => x.PhoneNumber).HasMaxLength(50);

            builder.Property(x => x.Slogan).HasMaxLength(255);

            builder.Property(x => x.BannerUrl).HasMaxLength(255);

            builder.Property(x => x.Status).IsRequired().HasMaxLength(50);

            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.OwnerUserId);

            builder.HasOne(x => x.Owner).WithMany(x => x.Clinics).HasForeignKey(x => x.OwnerUserId);


        }
    }
}
