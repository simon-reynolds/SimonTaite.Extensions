using System;

namespace SReynolds.Extensions
{
    public static class DateTimeExtensions
    {

#if NET451        
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
#endif
       
        public static DateTime FromUnixTimeSeconds(this long timestamp)
        {
#if DOTNET5_4
            DateTimeOffset dt = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return dt.DateTime;
#elif NET451
            return UnixEpoch.AddSeconds(timestamp);
#endif
        }
        
        public static DateTime FromUnixTimeMilliseconds(this long timestamp)
        {
#if DOTNET5_4
            DateTimeOffset dt = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
            return dt.DateTime;
#elif NET451
            return UnixEpoch.AddMilliseconds(timestamp);
#endif
        }
        
        public static long ToUnixTimeSeconds(this DateTime dateTime)
        {
#if DOTNET5_4
            var dt = new DateTimeOffset(dateTime);
            return dt.ToUnixTimeSeconds();
#elif NET451 
            long seconds = dateTime.Ticks / TimeSpan.TicksPerSecond;
            return seconds - UnixEpochSeconds;
#endif
        }
        
        public static long ToUnixTimeMilliseconds(this DateTime dateTime)
        {
#if DOTNET5_4
            var dt = new DateTimeOffset(dateTime);
            return dt.ToUnixTimeMilliseconds();
#elif NET451 
            // Truncate sub-millisecond precision before offsetting by the Unix Epoch to avoid
            // the last digit being off by one for dates that result in negative Unix times
            long milliseconds = dateTime.Ticks / TimeSpan.TicksPerMillisecond;
            return milliseconds - UnixEpochMilliseconds;
#endif
        }
    }
}