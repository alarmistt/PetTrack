using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Entity;

namespace PetTrack.Repositories.EntityConfigurations
{
    public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            builder.ToTable("WalletTransactions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type).IsRequired().HasMaxLength(50);

            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");

            builder.Property(x => x.Description).HasMaxLength(255);

            builder.HasIndex(x => x.WalletId);
            builder.HasIndex(x => x.Type);

            builder.HasOne(x => x.Wallet).WithMany(w => w.Transactions).HasForeignKey(x => x.WalletId);
            builder.HasOne(x => x.Booking).WithMany(b => b.Transactions).HasForeignKey(x => x.BookingId);
        }
    }
}
