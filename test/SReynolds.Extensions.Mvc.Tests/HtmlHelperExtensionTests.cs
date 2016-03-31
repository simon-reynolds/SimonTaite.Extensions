using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using SReynolds.Extensions.Mvc

namespace SReynolds.Extensions.Mvc.Tests
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dnx.html
    public class HtmlHelperExtensionTests
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
