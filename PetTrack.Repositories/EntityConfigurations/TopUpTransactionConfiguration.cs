using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Entity;

namespace PetTrack.Repositories.EntityConfigurations
{
    public class TopUpTransactionConfiguration : IEntityTypeConfiguration<TopUpTransaction>
    {
        public void Configure(EntityTypeBuilder<TopUpTransaction> builder)
        {
            builder.ToTable("TopUpTransactions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");

            builder.Property(x => x.PaymentMethod).IsRequired().HasMaxLength(50);

            builder.Property(x => x.Status).IsRequired().HasMaxLength(50);

            builder.Property(x => x.TransactionCode).HasMaxLength(255);

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Status);

            builder.HasOne(x => x.User).WithMany(u => u.TopUps).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Booking).WithOne(u => u.TopUpTransaction).HasForeignKey<TopUpTransaction>(x => x.BookingId);
        }
    }
}
