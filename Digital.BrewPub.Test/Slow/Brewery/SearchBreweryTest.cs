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
        //One can only run these tests up to 400 times a day, due to API limitations.
        [Fact]
        public async Task ListsDetroitBreweries()
        {
           
            using (var fixture = new FunctionalTestFixture())
            {
                var searchResponse = await fixture.Client.GetAsync("/Brewery/Search");

                searchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
                var searchContent = await searchResponse.Content.ReadAsStringAsync();
                searchContent.Contains("Brew Detroit").Should().BeTrue();
            }
        }

        [Fact]
        public async Task ListsNotesForABrewery()
        {
            using (var fixture = new FunctionalTestFixture())
            {
                //add notes through the backend.
                //get the list of breweries
                //see if the Brewery has the notes.
            }
        }
    
    }
}
