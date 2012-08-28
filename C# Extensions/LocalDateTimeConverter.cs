using System;

namespace Extensions
{
    public static class LocalDateTimeConverter
    {
        public static DateTime ToMountainTime(this DateTime utcDateTime)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZoneInfo);
        }
    }
}