using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Digital.BrewPub.Features.Note;
using System.Threading.Tasks;
using System.Linq;
using FluentAssertions;
using Xunit.Abstractions;

namespace Digital.BrewPub.Test.Slow.Note
{
    [Collection("1")]
    public class NoteTest
    {

        [Fact]
        public async Task EnthusiastsMakeNotes()
        {
            using (var fixture = new FunctionalTestFixture())
            {
                IEnumerable<KeyValuePair<string, string>> formValues = new Dictionary<string, string>()
                {
                    {nameof(NoteController.NotePostInput.Text), "I love it" }
                };
                System.Net.Http.HttpContent content = new FormUrlEncodedContent(formValues);

                var response = await fixture.Client.PostAsync("/Brewery/Note/Love Shack", content);
                var responseContent = await response.Content.ReadAsStringAsync();


                fixture.WithinDbContext(dbContext =>
                {
                    Features.Note.Note[] notes = dbContext.Notes.ToArray();
                    var note = notes.First(n => n.Brewery.Equals("Love Shack"));
                    note.Text.Should().Be("I love it");
                });
            }
        }
    }
}
