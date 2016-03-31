using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using SReynolds.Extensions;

namespace SReynolds.Extensions.Tests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
