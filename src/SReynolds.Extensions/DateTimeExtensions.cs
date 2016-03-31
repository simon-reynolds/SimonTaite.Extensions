using System;

namespace SReynolds.Extensions
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
        
        public static DateTime UnixEpoch
        {
            get { return new System.DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc); }
        }
        
        public static DateTime FromUnixTimestamp(long timestamp)
        {
            return UnixEpoch.AddMilliseconds(timestamp);
        }
        
        public static long ToUnixTimeSeconds()
        {
            long seconds = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
            return seconds - UnixEpochSeconds;
        }
        
        public static long ToUnixTimeSeconds(DateTime dateTime)
        {
            long seconds = dateTime.Ticks / TimeSpan.TicksPerSecond;
            return seconds - UnixEpochSeconds;
        }
        
        public long ToUnixTimeMilliseconds() {
            // Truncate sub-millisecond precision before offsetting by the Unix Epoch to avoid
            // the last digit being off by one for dates that result in negative Unix times
            long milliseconds = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
            return milliseconds - UnixEpochMilliseconds;
        }
        
        public long ToUnixTimeMilliseconds(DateTime dateTime) {
            // Truncate sub-millisecond precision before offsetting by the Unix Epoch to avoid
            // the last digit being off by one for dates that result in negative Unix times
            long milliseconds = dateTime.Ticks / TimeSpan.TicksPerMillisecond;
            return milliseconds - UnixEpochMilliseconds;
        }
        
        
    }
}