﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Digital.BrewPub.Features.Brewery;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Digital.BrewPub.Test.Fast.Brewery
{
    public class BrewerySearchNotesTest
    {
        BreweryController sut;
        StubbedBrewerySearchGateway stubbedBrewerySearchGateway;
        StubbedNotesQueryHandler stubbedNotesQueryHandler;

        public BrewerySearchNotesTest()
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
            stubbedNotesQueryHandler.AddNote(new NotesByBreweryResult.Note {
                Brewery = "Love Shack",
                Text = "I think this brewery is cool!"
            });

            var result = await sut.Search();
            var model = result.GetModel<BrewerySearchViewModel>();

            model.Breweries[0].Notes[0].Text.Should().Be("I think this brewery is cool!");
        }

        [Fact]
        public async Task ShouldCorrelateNotesToBreweries()
        {
            stubbedBrewerySearchGateway.AddBrewery(new BrewerySearchResult.Brewery
            {
                Name = "Love Shack",
                StreetAddress = "1234 Love Street"
            });
            stubbedBrewerySearchGateway.AddBrewery(new BrewerySearchResult.Brewery
            {
                Name = "Like Shack",
                StreetAddress = "3078 Loveless Street"
            });
            const string loveShackNote = "I think this brewery is cool!";
            stubbedNotesQueryHandler.AddNote(new NotesByBreweryResult.Note
            {
                Brewery = "Love Shack",
                Text = loveShackNote
            });
            const string likeShackNote = "This brewery is alright";
            stubbedNotesQueryHandler.AddNote(new NotesByBreweryResult.Note
            {
                Brewery = "Like Shack",
                Text = likeShackNote
            });


            var result = await sut.Search();
            var model = result.GetModel<BrewerySearchViewModel>();

            var sortedBreweries = model.Breweries.OrderBy(b => b.Name).ToArray();
            sortedBreweries[0].Notes.Length.Should().Be(1);
            sortedBreweries[0].Notes[0].Text.Should().Be(likeShackNote);
            sortedBreweries[1].Notes[0].Text.Should().Be(loveShackNote);
        }
       
        [Fact]
        public async Task NotesShouldNotBeEditableWhenTheyAreAuthoredByAnotherUser()
        {
            stubbedBrewerySearchGateway.AddBrewery(new BrewerySearchResult.Brewery
            {
                Name = "Love Shack",
                StreetAddress = "1234 Love Street"
            });
            const string loveShackNote = "I think this brewery is cool!";
            stubbedNotesQueryHandler.AddNote(new NotesByBreweryResult.Note
            {
                Brewery = "Love Shack",
                AuthorId = "cmoorebutz",
                Text = loveShackNote
            });

            var result = await sut.Search();
            var model = result.GetModel<BrewerySearchViewModel>();

            model.Breweries[0].Notes[0].IsEditable.Should().Be(false);
        }

        [Fact]
        public async Task NotesShouldBeEditableWhenTheyAreAuthoredByCurrentUser()
        {
            stubbedBrewerySearchGateway.AddBrewery(new BrewerySearchResult.Brewery
            {
                Name = "Love Shack",
                StreetAddress = "1234 Love Street"
            });
            const string loveShackNote = "I think this brewery is cool!";
            stubbedNotesQueryHandler.AddNote(new NotesByBreweryResult.Note
            {
                Brewery = "Love Shack",
                AuthorId = "cbernholdt",
                Text = loveShackNote
            });

            var user = new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "cbernholdt")}));
            sut.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            var result = await sut.Search();
            var model = result.GetModel<BrewerySearchViewModel>();

            model.Breweries[0].Notes[0].IsEditable.Should().Be(true);
        }
    }
}