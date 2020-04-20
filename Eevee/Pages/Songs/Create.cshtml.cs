using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Eevee.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using VSpace = NaturalLanguage.vector.VectorSpace;
using System.Globalization;

namespace Eevee.Pages.Songs
{
    public class CreateModel : PageModel
    {
        private readonly Eevee.Data.EeveeContext _context;

        private readonly NaturalLanguage.NN.INN _textprocessor;


        private IWebHostEnvironment _environment;


        private readonly float artist_contrib = 0.3f;
        private readonly float genre_contrib = 0.3f;

        public string error = "";

        public CreateModel(Eevee.Data.EeveeContext context, NaturalLanguage.NN.INN textprocessor, IWebHostEnvironment environment)
        {
            _context = context;
            _textprocessor = textprocessor;
            _environment = environment;

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Song Song { get; set; }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        private string file = "";


        public async Task<IActionResult> OnPostAsync(int? id)
        {
            file = Path.Combine(_environment.ContentRootPath, "wwwroot", "music", UploadedFile.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await UploadedFile.CopyToAsync(fileStream);
            }

            Song.Filepath = "~/music/" + UploadedFile.FileName;

            User user = _context.User.FirstOrDefault(u => u.Username == User.Identity.Name);

            Artist artist =  _context.Artist.FirstOrDefault(a => a.ArtistID == id);

            Genre genre = _context.Genre.FirstOrDefault(g => g.Name == Song.Genre.Name);

            if (genre == null)
            {
                genre = new Genre
                {
                    Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Song.Genre.Name.ToLower()),
                    WordVec = VSpace.ConvertToString(_textprocessor.PredictText(Song.Genre.Name))
                };
            }

            _context.Genre.Add(genre);
            Song.Genre = genre;
            
            var album = _context.Album.FirstOrDefault(a => a.Name == Song.Album.Name && a.Artist.ArtistID == id);

            if (album == null)
            {
                Album Album = new Album
                {
                    Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Song.Album.Name),
                    Artist = artist
                };
                Song.Album = Album;
                _context.Album.Add(Album);
            }
            else
            {
                Song.Album = album;
            }

            _textprocessor.PredictText(Song.Lyrics);

            Song.Listens = 1;

            NaturalLanguage.vector.VectorSpace.ToArray(artist.WordVec);

            NaturalLanguage.vector.VectorSpace.ToArray(genre.WordVec);

            NaturalLanguage.vector.VectorSpace.Scale(genre_contrib, NaturalLanguage.vector.VectorSpace.ToArray(genre.WordVec));

            Song.WordVec = NaturalLanguage.vector.VectorSpace.ConvertToString(
                NaturalLanguage.vector.VectorSpace.Add(
                    NaturalLanguage.vector.VectorSpace.Add(_textprocessor.PredictText(Song.Lyrics), 
                    NaturalLanguage.vector.VectorSpace.Scale(artist_contrib, NaturalLanguage.vector.VectorSpace.ToArray(artist.WordVec))),
                    NaturalLanguage.vector.VectorSpace.Scale(genre_contrib, NaturalLanguage.vector.VectorSpace.ToArray(genre.WordVec))));

            _context.Song.Add(Song);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
