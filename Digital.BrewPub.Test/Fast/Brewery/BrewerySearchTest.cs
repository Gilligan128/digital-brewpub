using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Digital.BrewPub.Features.Brewery;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Digital.BrewPub.Test.Fast.Brewery
{
    public class BrewerySearchTest
    {
        BreweryController sut;
        StubbedBrewerySearchGateway stubbedBrewerySearchGateway;
        StubbedNotesQueryHandler stubbedNotesQueryHandler;

        public BrewerySearchTest()
        {
            stubbedBrewerySearchGateway = new StubbedBrewerySearchGateway();
            stubbedNotesQueryHandler = new StubbedNotesQueryHandler();
            sut = new BreweryController(stubbedBrewerySearchGateway, stubbedNotesQueryHandler);
        }

        [Fact]
        public async Task shouldHaveNoNotesForABreweryWhenNoneAreAdded()
        {
            stubbedBrewerySearchGateway.AddBrewery(new BrewerySearchResult.Brewery {
                Name = "Love Shack",
                StreetAddress = "1234 Love Street"
            });
           
            var result = await sut.Search();
            var model = result.GetModel<BrewerySearchViewModel>();

            model.Breweries[0].Notes.Should().BeEmpty();
        }

        [Fact]
        public async Task shouldHaveANoteFromOneUser()
        {
            stubbedBrewerySearchGateway.AddBrewery(new BrewerySearchResult.Brewery
            {
                Name = "Love Shack",
                StreetAddress = "1234 Love Street"
            });
            stubbedNotesQueryHandler.AddNote(new NotesByBreweryResult.Note { Brewery = "Love Shack", Text = "I think this brewery is cool!" });

            var result = await sut.Search();
            var model = result.GetModel<BrewerySearchViewModel>();

            model.Breweries[0].Notes[0].Text.Should().Be("I think this brewery is cool!");
        }

       
    }
}
