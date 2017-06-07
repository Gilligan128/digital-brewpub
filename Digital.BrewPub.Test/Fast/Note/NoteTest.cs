using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Digital.BrewPub.Data;
using Digital.BrewPub.Features.Note;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Digital.BrewPub.Test.Fast.Note
{
    public class NoteTest
    {
        [Fact]
        public async Task EnthusiastCanMakeNewNoteWithTextForBrewery()
        {
            using (ApplicationDbContext appDbContext = createUnitTestableDbContext())
            {
                var sut = new NoteController(appDbContext);
                const string noteText = "I love it!";
                const string breweryName = "Love Shack";

                var result = await sut.Make(new NoteController.MakeInput
                {
                    Id = breweryName,
                    Text = noteText
                });

                var note = appDbContext.Notes.First(n => n.Brewery.Equals(breweryName));
                note.Text.Should().Be(noteText);
            }
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
