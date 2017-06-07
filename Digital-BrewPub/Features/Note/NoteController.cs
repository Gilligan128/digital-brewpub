using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digital.BrewPub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Digital.BrewPub.Features.Note
{
    public class NoteController : Controller
    {
        private ApplicationDbContext appDbContext;

        public NoteController(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [Route("Brewery/Note/{brewery}")]
        [HttpPost]
        public IActionResult Post(NotePostInput form)
        {
            appDbContext.Notes.Add(new Note
            {
                Id = Guid.NewGuid(),
                Brewery = form.Brewery,
                AuthorId = User?.Identity?.Name ?? "system",
                Text = form.Text
            });
            appDbContext.SaveChanges();
            return  Redirect("/");
        }

        public class NotePostInput
        {
            public string Text { get; set; }
            public string Brewery { get; set; }
        }
    }
}