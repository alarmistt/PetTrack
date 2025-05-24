using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetTrack.Entity;

namespace PetTrack.Repositories.EntityConfigurations
{
    public class ClinicScheduleConfiguration : IEntityTypeConfiguration<ClinicSchedule>
    {
        public void Configure(EntityTypeBuilder<ClinicSchedule> builder)
        {
            builder.ToTable("ClinicSchedules");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.DayOfWeek).IsRequired();

            builder.Property(x => x.OpenTime).IsRequired();

            builder.Property(x => x.CloseTime).IsRequired();

            builder.HasOne(x => x.Clinic).WithMany(c => c.Schedules).HasForeignKey(x => x.ClinicId);
        }
    }
}
