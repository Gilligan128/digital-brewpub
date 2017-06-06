namespace Digital.BrewPub.Features.Brewery
{
    public struct NotesByBreweryResult
    {
        public Note[] Notes { get; set; }

        public class Note
        {
            public string Text { get; set; }
            public string Brewery { get; set; }
            public string AuthorId { get; set; }
        }
    }
}