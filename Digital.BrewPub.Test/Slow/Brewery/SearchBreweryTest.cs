using System;
using System.Net;
using System.Threading.Tasks;
using Digital.BrewPub.Features.Brewery;
using FluentAssertions;
using Xunit;

namespace Digital.BrewPub.Test.Slow.Brewery
{
    [Collection("1")]
    public class SearchBreweryTest : IDisposable
    {
        private FunctionalTestFixture fixture;

        public SearchBreweryTest()
        {
            fixture = new FunctionalTestFixture();
        }


        //One can only run these tests up to 400 times a day, due to API limitations.
        [Fact]
        public async Task ListsDetroitBreweries()
        {
            var searchResponse = await fixture.Client.GetAsync("/Brewery/Search?SearchType=City&Term=Detroit");
            var searchContent = await searchResponse.Content.ReadAsStringAsync();

            searchResponse.EnsureSuccessStatusCode();
            searchContent.Contains("Brew Detroit").Should().BeTrue();
        }

        [Fact]
        public async Task ListsNothingForUnkownCity()
        {
            var searchResponse = await fixture.Client.GetAsync("/Brewery/Search?SearchType=City&Term=Philly");
            var searchResponseContent = await searchResponse.GetModelAsync<BrewerySearchViewModel>();

            searchResponse.EnsureSuccessStatusCode();
            searchResponseContent.Breweries.Should().BeEmpty();
        }

        [Fact]
        public async Task ListsNotesForABrewery()
        {
            
            fixture.WithinDbContext(dbContext =>
            {
                dbContext.Notes.Add(new Features.Note.Note
                {
                    Id = Guid.NewGuid(),
                    Text = "I love it",
                    Brewery = "BrewDetroit",
                    AuthorId = "cbernholdt"
                });
                dbContext.SaveChanges();
            });
               

            var searchResponse = await fixture.Client.GetAsync("/Brewery/Search?SearchType=City&Term=Detroit");
            var searchContent = await searchResponse.GetModelAsync<BrewerySearchViewModel>();

            searchResponse.EnsureSuccessStatusCode();
            searchContent.Breweries[0].Notes[0].Text.Should().Be("I love it");
 
        }

        public void Dispose()
        {
            fixture.Dispose();
        }
    }
}
