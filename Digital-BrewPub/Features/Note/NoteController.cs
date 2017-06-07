using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digital.BrewPub.Data;
using Microsoft.AspNetCore.Mvc;

namespace Digital.BrewPub.Features.Note
{
    public class NoteController : Controller
    {
        private ApplicationDbContext appDbContext;

        public NoteController(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IActionResult> Make(MakeInput form)
        {
            appDbContext.Notes.Add(new Note
            {
                Id = Guid.NewGuid(),
                Brewery = form.Id,
                AuthorId = User?.Identity?.Name ?? "system",
                Text = form.Text
            });
            appDbContext.SaveChanges();

            return await Task.FromResult(Redirect("/"));
        }

        public class MakeInput
        {
            public string Text { get; set; }
            public string Id { get; set; }
        }
    }
}