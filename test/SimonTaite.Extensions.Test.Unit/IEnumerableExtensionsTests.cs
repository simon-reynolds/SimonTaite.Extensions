using System;
using Xunit;

using SimonTaite.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace SimonTaite.Extensions.Test.Unit
{
    public class IEnumerableExtensionsTests
    {
        [Fact]
        public void ToUnarySequence_WorksAsExpected()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            var actual = guid.ToUnarySequence();

            // Assert
            Assert.True(actual.Count() == 1);

            Assert.True(actual.First() == guid);
        }

        [Fact]
        public void Iterate_WorksAsExpected()
        {
            // Arrange
            var input = Enumerable.Range(0, 100);
            var expected = new int[100];

            // Act
            input.Iterate(i => expected[i] = (i + 1));

            // Assert
            Assert.True(expected.Length == 100);
            Assert.True(expected.All(i => i > 0));

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(expected[i] == (i + 1));
            }
        }

        [Fact]
        public void Partition_WorksAsExpected()
        {
            // Arrange
            var input = Enumerable.Range(0, 100);

            // Act
            var output = input.Partition(10);

            // Assert
            Assert.True(output.Count() == 10);
            Assert.True(output.All(a => a.Count() == 10));

            var o = output.SelectMany(a => a).OrderBy(i => i);
            Assert.Equal(o, input);
        }
    }
}