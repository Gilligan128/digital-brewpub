using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Digital.BrewPub.Test.Slow.Brewery
{
    public class SearchBreweryTest
    {
      
        [Fact]
        public async Task ListsDetroitBreweries()
        {
            //One can only run this test up to 400 times a day, due to API limitations.
            using (var fixture = new FunctionalTestFixture())
            {
                var searchResponse = await fixture.Client.GetAsync("/Brewery/Search");

                searchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
                var searchContent = await searchResponse.Content.ReadAsStringAsync();
                searchContent.Contains("Brew Detroit").Should().BeTrue();
            }
        }
    
    }
}
