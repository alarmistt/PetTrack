using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PetTrack.Repositories.Base
{
    public class UtcDateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
    {
        public UtcDateTimeOffsetConverter()
            : base(
                v => v.ToUniversalTime(), // Convert to UTC when saving
                v => DateTime.SpecifyKind(v.DateTime, DateTimeKind.Utc)) // Mark as UTC when reading
        {
        }
    }

}
