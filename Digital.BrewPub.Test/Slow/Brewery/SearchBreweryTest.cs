﻿using System;
using System.Net;
using System.Threading.Tasks;
using Digital.BrewPub.Features.Brewery;
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
                fixture.DbContext.Notes.Add(new Features.Note.Note {
                    Id = Guid.NewGuid(), Text = "I love it", Brewery = "Brew Detroit" , AuthorId="cbernholdt"});
                fixture.DbContext.SaveChanges();

                var searchResponse = await fixture.Client.GetAsync("/Brewery/Search");
                var searchContent = await searchResponse.GetModelAsync<BrewerySearchViewModel>();

                searchResponse.EnsureSuccessStatusCode();
                searchContent.Breweries[0].Notes[0].Text.Should().Be("I love it");
            }
        }
    
    }
}
