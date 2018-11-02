using System;
using Xunit;

using SimonTaite.Extensions;

namespace SimonTaite.Extensions.Test.Unit
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void DateTimeToUnixTimeSeconds()
        {
            long expected = 1234567890;
            DateTime test = new DateTime(2009, 02, 13, 23, 31, 30, 0, DateTimeKind.Utc); //23:31:30 UTC on 13 February 2009
            
            var result = DateTimeExtensions.ToUnixTimeSeconds(test);
            
            Assert.Equal(expected, result);
        }
        
        [Fact]
        public void DateTimeFromUnixTimeSeconds()
        {
            long test = 1000000000;
            var expected = new DateTime(2001, 9, 9, 01, 46, 40, 0, DateTimeKind.Utc); //01:46:40 UTC on 9 September 2001
            
            var result = DateTimeExtensions.FromUnixTimeSeconds(test);
            
            Assert.Equal(expected, result);
        }

    }
}
