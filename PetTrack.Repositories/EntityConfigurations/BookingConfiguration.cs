using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Entity;

namespace PetTrack.Repositories.EntityConfigurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status).IsRequired().HasMaxLength(50);

            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

            builder.Property(x => x.PlatformFee).HasColumnType("decimal(18,2)");

            builder.Property(x => x.ClinicReceiveAmount).HasColumnType("decimal(18,2)");

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.AppointmentDate);
            builder.HasIndex(x => x.Status);

            builder.HasOne(x => x.User)
                    .WithMany(u => u.Bookings)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Clinic)
                   .WithMany()
                   .HasForeignKey(x => x.ClinicId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ServicePackage)
                   .WithMany()
                   .HasForeignKey(x => x.ServicePackageId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
