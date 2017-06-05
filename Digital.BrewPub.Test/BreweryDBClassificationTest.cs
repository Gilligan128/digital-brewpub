using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Digital.BrewPub.Test
{
    public class BreweryDBClassificationTest
    {
        [Fact]
        [Trait("Category","Classification")]
        public void testingExplicitRun()
        {
            true.Should().BeTrue();
        }
    }
}
