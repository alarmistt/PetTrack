using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Entity;

namespace PetTrack.Repositories.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Address).HasMaxLength(255);

            builder.Property(x => x.PasswordHash).HasMaxLength(255);

            builder.Property(x => x.FullName).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Role).IsRequired().HasMaxLength(50);

            builder.Property(x => x.PhoneNumber).HasMaxLength(50);

            builder.Property(x => x.AvatarUrl).HasMaxLength(255);

            builder.Property(x => x.Role).IsRequired();

            builder.HasOne(x => x.Wallet)
                   .WithOne(w => w.User)
                   .HasForeignKey<Wallet>(w => w.UserId);
        }
    }
}
