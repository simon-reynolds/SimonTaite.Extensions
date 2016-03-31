namespace SReynolds.Extensions
{
    public static class DateTimeExtensions
    {
        public static System.DateTime UnixEpoch
        {
            get { return new System.DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc); }
        }
        
        public static System.DateTime FromTimestamp(long timestamp)
        {
            return UnixEpoch.AddMilliseconds(timestamp);
        }   
    }
}