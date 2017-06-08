using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digital.BrewPub.Data;
using Microsoft.AspNetCore.Authorization;
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
            string currentUser = User?.Identity?.Name ?? "system";
            var existingNote = appDbContext.Notes.SingleOrDefault(x => x.Brewery.Equals(form.Brewery) && x.AuthorId.Equals(currentUser));
            if (existingNote == null)
            {
                appDbContext.Notes.Add(new Note
                {
                    Id = Guid.NewGuid(),
                    Brewery = form.Brewery,
                    AuthorId = currentUser,
                    Text = form.Text
                });
            }
            else
            {
                existingNote.Text = form.Text;
            }
            appDbContext.SaveChanges();
            return  Redirect(string.IsNullOrEmpty(form.ReturnUrl) ? "/" : form.ReturnUrl);
        }

        [Route("Brewery/Note/{brewery}")]
        [HttpGet]
        public IActionResult Post(string brewery)
        {
            return View(new NotePostInput { Brewery = brewery });
        }

        public class NotePostInput
        {
            public string Text { get; set; }
            public string Brewery { get; set; }
            public string ReturnUrl { get; set; }
        }
    }
}