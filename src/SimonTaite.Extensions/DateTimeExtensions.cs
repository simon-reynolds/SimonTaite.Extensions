using System;

namespace SimonTaite.Extensions
{
    public static class DateTimeExtensions
    {       
        // Number of days in a non-leap year
        private const int DaysPerYear = 365;
        // Number of days in 4 years
        private const int DaysPer4Years = DaysPerYear * 4 + 1;       // 1461
        // Number of days in 100 years
        private const int DaysPer100Years = DaysPer4Years * 25 - 1;  // 36524
        // Number of days in 400 years
        private const int DaysPer400Years = DaysPer100Years * 4 + 1; // 146097
        
        // Number of days from 1/1/0001 to 12/31/1969
        private const int DaysTo1970 = DaysPer400Years * 4 + DaysPer100Years * 3 + DaysPer4Years * 17 + DaysPerYear; // 719,162
        
        private const long UnixEpochTicks = TimeSpan.TicksPerDay * DaysTo1970; // 621,355,968,000,000,000
        private const long UnixEpochSeconds = UnixEpochTicks / TimeSpan.TicksPerSecond; // 62,135,596,800
        private const long UnixEpochMilliseconds = UnixEpochTicks / TimeSpan.TicksPerMillisecond; // 62,135,596,800,000
        
        private static readonly DateTime UnixEpoch = new System.DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
       
        public static DateTime FromUnixTimeSeconds(this long timestamp)
        {
#if DOTNET5_4
            DateTimeOffset dt = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return dt.DateTime;
#endif
            return UnixEpoch.AddSeconds(timestamp);
        }
        
        public static DateTime FromUnixTimeMilliseconds(this long timestamp)
        {
#if DOTNET5_4
            DateTimeOffset dt = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
            return dt.DateTime;
#endif
            return UnixEpoch.AddMilliseconds(timestamp);
        }
        
        public static long ToUnixTimeSeconds(this DateTime dateTime)
        {
#if DOTNET5_4
            var dt = new DateTimeOffset(dateTime);
            return dt.ToUnixTimeSeconds();
#endif 
            long seconds = dateTime.Ticks / TimeSpan.TicksPerSecond;
            return seconds - UnixEpochSeconds;
        }
        
        public static long ToUnixTimeMilliseconds(this DateTime dateTime)
        {
#if DOTNET5_4
            var dt = new DateTimeOffset(dateTime);
            return dt.ToUnixTimeMilliseconds();
#endif 
            // Truncate sub-millisecond precision before offsetting by the Unix Epoch to avoid
            // the last digit being off by one for dates that result in negative Unix times
            long milliseconds = dateTime.Ticks / TimeSpan.TicksPerMillisecond;
            return milliseconds - UnixEpochMilliseconds;
        }
        
        /// <summary>
        /// Determines if a <see cref="DateTime" /> lies within a specified range
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime" /> to check</param>
        /// <param name="start">The <see cref="DateTime" /> that represents the start of the range</param>
        /// <param name="end"></param>
        /// <returns>True if the <see cref="DateTime" /> lies within the range, False otherwise</returns>
        public static bool IsWithinRange(this DateTime dateTime, DateTime start, DateTime end)
        {
            return dateTime >= start && dateTime <= end;
        }
        
        /// <summary>
        /// Determines if a <see cref="DateTime" /> lies within a specified range. An EndDate of null is considered equivalent to DateTime.MaxValue
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime" /> to check</param>
        /// <param name="start">The <see cref="DateTime" /> that represents the start of the range</param>
        /// <param name="end">The <see cref="Nullable<DateTime>" /> that represents the end of the range</param>
        /// <returns>True if the <see cref="DateTime" /> lies within the range, False otherwise</returns>
        public static bool IsWithinRange(this DateTime dateTime, DateTime start, DateTime? end)
        {
            return dateTime >= start && dateTime <= end.GetValueOrDefault(DateTime.MaxValue);
        }
    }
}