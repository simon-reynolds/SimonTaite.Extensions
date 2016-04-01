using System;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Xunit;

namespace SReynolds.Extensions.Mvc.Tests
{
    public class HtmlHelperExtensionTests
    {
        //[Fact]
        public void PassingTest()
        {
            var expected = "Property1";
            
            //TODO: Mock does not instantiate HtmlHelper correctly
            var htmlHelper = Moq.Mock.Of<HtmlHelper<TestClass>>();
            var actual = HtmlHelperExtensions.FieldIdFor(htmlHelper, m => m.ObjectProperty);
            
            Assert.Equal(expected, actual);
        }
        
        public class TestClass
        {
            public string StringProperty { get; set; }
            public object ObjectProperty { get; set; }
        }
    }
}
