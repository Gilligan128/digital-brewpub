using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Digital.BrewPub.Features.Note;
using System.Threading.Tasks;
using System.Linq;
using FluentAssertions;

namespace Digital.BrewPub.Test.Slow.Note
{
    public class NoteTest
    {
        [Fact]
        public async Task EnthusiastsMakeNotes()
        {
            using(var fixture = new FunctionalTestFixture())
            {
                IEnumerable<KeyValuePair<string, string>> formValues = new Dictionary<string, string>()
                {
                    {nameof(NoteController.MakeInput.Text), "I love it" }
                };
                System.Net.Http.HttpContent content = new FormUrlEncodedContent(formValues);

                var response = await fixture.Client.PostAsync("/Note/Make/Love Shack", content);

                using (var tx = fixture.DbContext.Database.BeginTransaction())
                {
                    var note = fixture.DbContext.Notes.First(n => n.Brewery.Equals("Love Shack"));
                    note.Text.Should().Be("I love it");
                }
               
            }
        }
    }
}
