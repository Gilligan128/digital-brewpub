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
            return appDbContext.Notes.First(n => n.Brewery.Equals(MakeNoteForLoveShackScenarioData.BreweryName));
        }

        private ApplicationDbContext createUnitTestableDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(nameof(EnthusiastCanMakeNewNoteWithTextForBrewery));
            ApplicationDbContext applicationDbContext = new ApplicationDbContext(options.Options);
            return applicationDbContext;
        }
    }
}
