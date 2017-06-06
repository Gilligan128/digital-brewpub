using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digital.BrewPub.Features.Brewery;

namespace Digital.BrewPub.Test.Fast.Brewery
{
    internal class StubbedNotesQueryHandler : HandleQuery<NotesByBreweryQuery, NotesByBreweryResult>
    {
        ISet<NotesByBreweryResult.Note> notes = new HashSet<NotesByBreweryResult.Note>();

        public StubbedNotesQueryHandler()
        {
        }

        public Task<NotesByBreweryResult> HandleAsync(NotesByBreweryQuery query)
        {
            return Task.FromResult(new NotesByBreweryResult
            {
                Notes = new List<NotesByBreweryResult.Note>(notes).ToArray()
            });
        }

        internal void AddNote(NotesByBreweryResult.Note note)
        {
            notes.Add(note);
        }
    }
}