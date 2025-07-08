namespace PetTrack.Core.Helpers
{
    public class CoreHelper
    {
        public static DateTimeOffset SystemTimeNow => CoreHelper.ConvertToUtcPlus7(DateTimeOffset.UtcNow);

        public static DateTimeOffset ConvertToUtcPlus7NotChanges(DateTimeOffset dateTimeOffset)
        {
            // UTC+7 is 7 hours ahead of UTC
            TimeSpan utcPlus7Offset = new(7, 0, 0);
            return dateTimeOffset.ToOffset(utcPlus7Offset).AddHours(-7);
        }
        public static DateTimeOffset ConvertToUtcPlus7(DateTimeOffset dateTimeOffset)
        {
            // UTC+7 is 7 hours ahead of UTC
            TimeSpan utcPlus7Offset = new(7, 0, 0);
            return dateTimeOffset.ToOffset(utcPlus7Offset);
        }

    }
}
