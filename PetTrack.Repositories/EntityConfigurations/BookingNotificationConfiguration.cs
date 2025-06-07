using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Entity;

namespace PetTrack.Repositories.EntityConfigurations
{
    public class BookingNotificationConfiguration : IEntityTypeConfiguration<BookingNotification>
    {
        public void Configure(EntityTypeBuilder<BookingNotification> builder)
        {
            builder.ToTable("BookingNotifications");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type).IsRequired().HasMaxLength(50);

            builder.Property(x => x.Subject).IsRequired().HasMaxLength(50);


            builder.Property(x => x.Content).HasMaxLength(2000);

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.BookingId);

            builder.HasOne(x => x.Booking).WithMany(b => b.Notifications).HasForeignKey(x => x.BookingId);
            builder.HasOne(x => x.User).WithMany(u => u.Notifications).HasForeignKey(x => x.UserId);
        }
    }
}
