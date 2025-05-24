using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Entity;

namespace PetTrack.Repositories.EntityConfigurations
{
    public class BookingStatusHistoryConfiguration : IEntityTypeConfiguration<BookingStatusHistory>
    {
        public void Configure(EntityTypeBuilder<BookingStatusHistory> builder)
        {
            builder.ToTable("BookingStatusHistories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OldStatus).HasMaxLength(50);

            builder.Property(x => x.NewStatus).HasMaxLength(50);

            builder.Property(x => x.ChangedBy).HasMaxLength(100);

            builder.Property(x => x.ChangeReason).HasMaxLength(255);

            builder.HasIndex(x => x.BookingId);

            builder.HasOne(x => x.Booking).WithMany(b => b.StatusHistories).HasForeignKey(x => x.BookingId);
        }
    }
}
