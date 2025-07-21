using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Entity;

namespace PetTrack.Repositories.EntityConfigurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("Feedbacks");

            builder.HasKey(f => f.Id);

            builder.HasOne(f => f.User)
                   .WithMany()
                   .HasForeignKey(f => f.UserId);

            builder.HasOne(f => f.Booking)
                   .WithMany()
                   .HasForeignKey(f => f.BookingId);
        }
    }
}
