using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Entity;

namespace PetTrack.Repositories.EntityConfigurations
{
    public class ServicePackageConfiguration : IEntityTypeConfiguration<ServicePackage>
    {
        public void Configure(EntityTypeBuilder<ServicePackage> builder)
        {
            builder.ToTable("ServicePackages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

            builder.HasIndex(x => x.ClinicId);

            builder.HasOne(x => x.Clinic).WithMany(c => c.ServicePackages).HasForeignKey(x => x.ClinicId);
        }
    }
}
