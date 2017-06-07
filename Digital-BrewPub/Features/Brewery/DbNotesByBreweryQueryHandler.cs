using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digital.BrewPub.Data;

namespace Digital.BrewPub.Features.Brewery
{
    public class DbNotesByBreweryQueryHandler : HandleQuery<NotesByBreweryQuery, NotesByBreweryResult>
    {
        private ApplicationDbContext dbContext;

        public DbNotesByBreweryQueryHandler(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<NotesByBreweryResult> HandleAsync(NotesByBreweryQuery query)
        {
                var notes = dbContext.Notes.Where(note => query.BreweryNames.Contains(note.Brewery))
                    .Select(note => new NotesByBreweryResult.Note
                    {
                        Brewery = note.Brewery,
                        AuthorId = note.AuthorId,
                        Text = note.Text
                    });
               
                return Task.FromResult(new NotesByBreweryResult { Notes = notes.ToArray() }); //not sure how to asynchronously query the db yet.
        }
    }
}
