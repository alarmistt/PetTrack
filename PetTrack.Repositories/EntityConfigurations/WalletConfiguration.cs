using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Entity;

namespace PetTrack.Repositories.EntityConfigurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Balance).HasColumnType("decimal(18,2)");

            builder.HasIndex(x => x.UserId);

            builder.HasOne(x => x.User).WithOne(u => u.Wallet).HasForeignKey<Wallet>(x => x.UserId);
        }
    }
}
