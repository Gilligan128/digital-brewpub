using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Digital.BrewPub.Data;
using Digital.BrewPub.Features.Note;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Digital.BrewPub.Test.Fast.Note
{
    public class NoteTest
    {

        private static class MakeNoteForLoveShackScenarioData {
            public const string NoteText = "I love it!";
            public const string BreweryName = "Love Shack";
         }   
       

        [Fact]
        public void EnthusiastCanMakeNewNoteWithTextForBrewery()
        {
            using (ApplicationDbContext appDbContext = createUnitTestableDbContext())
            {
                var sut = new NoteController(appDbContext);

                MakeNoteForLoveShackScenario(sut);

                var note = FindLoveShackNote(appDbContext);
                note.Text.Should().Be(MakeNoteForLoveShackScenarioData.NoteText);
            }
        }


        [Fact]
        public void NotesKnowWhoMadeThem()
        {
            using (ApplicationDbContext appDbContext = createUnitTestableDbContext())
            {
                var sut = new NoteController(appDbContext);
                const string username = "cbernholdt";
                sut.ControllerContext = createContextForUser(username);

                MakeNoteForLoveShackScenario(sut);
                
                var note = FindLoveShackNote(appDbContext);
                note.AuthorId.Should().Be(username);
            }
        }

        [Fact]
        public void NotesCanBeUpdated()
        {
            using (ApplicationDbContext appDbContext = createUnitTestableDbContext())
            {
                var sut = new NoteController(appDbContext);
                appDbContext.Notes.Add(new Features.Note.Note
                {
                    Id = Guid.NewGuid(),
                    Brewery = "Note Brew",
                    AuthorId = "cbernholdt",
                    Text = "old text"
                });
                appDbContext.SaveChanges();
                sut.ControllerContext = createContextForUser("cbernholdt");

                sut.Post(new NoteController.NotePostInput { Brewery = "Note Brew", Text = "I updated this note" });

                var note = FindNoteForBrewery(appDbContext, "Note Brew");
                note.Text.Should().Be("I updated this note");
            }
        }

        private static Microsoft.AspNetCore.Mvc.ControllerContext createContextForUser(string username)
        {
            return new Microsoft.AspNetCore.Mvc.ControllerContext() {
                HttpContext = new DefaultHttpContext() {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, username) })) 
                }
            };
        }

        private static  void MakeNoteForLoveShackScenario(NoteController sut)
        {
            var result =  sut.Post(new NoteController.NotePostInput
            {
                Brewery = MakeNoteForLoveShackScenarioData.BreweryName,
                Text = MakeNoteForLoveShackScenarioData.NoteText
            });
        }

        private static Features.Note.Note FindLoveShackNote(ApplicationDbContext appDbContext)
        {
            return FindNoteForBrewery(appDbContext, MakeNoteForLoveShackScenarioData.BreweryName);
        }

        private static Features.Note.Note FindNoteForBrewery(ApplicationDbContext appDbContext, string breweryName)
        {
            Features.Note.Note[] notes = appDbContext.Notes.ToArray();
            return notes.Single(n => n.Brewery.Equals(breweryName));
        }

        private ApplicationDbContext createUnitTestableDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase("Digital.BrewPub."+ Guid.NewGuid());
            ApplicationDbContext applicationDbContext = new ApplicationDbContext(options.Options);
            return applicationDbContext;
        }
    }
}
