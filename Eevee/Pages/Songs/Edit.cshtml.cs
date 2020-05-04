using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eevee.Data;
using Eevee.Models;
using VSpace = NaturalLanguage.vector.VectorSpace;
using System.Globalization;

namespace Eevee.Pages.Songs
{
    public class EditModel : PageModel
    {
        private readonly Eevee.Data.EeveeContext _context;

        private readonly NaturalLanguage.NN.INN _textprocessor;

        private readonly float artist_contrib = 0.3f;
        private readonly float genre_contrib = 0.3f;

        public EditModel(Eevee.Data.EeveeContext context, NaturalLanguage.NN.INN textprocessor)
        {
            _context = context;
            _textprocessor = textprocessor;
        }

        [BindProperty]
        public Song Song { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Song = await _context.Song.FirstOrDefaultAsync(m => m.SongID == id);

            if (Song == null)
            {
                return NotFound();
            }
            return Page();
        }

        public string error { get; set; }

        public string msg;
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            Song s = _context.Song.Where(x => x.SongID == Song.SongID).FirstOrDefault();



            User user = _context.User.FirstOrDefault(u => u.Username == User.Identity.Name);

            Artist artist = _context.Artist.FirstOrDefault(a => a.ArtistID == user.UserID);

            Album a = _context.Album.Where(x => x.Name == Song.Album.Name && x.Artist.ArtistID == user.UserID).FirstOrDefault();
            Genre g = _context.Genre.Where(x => x.Name == Song.Genre.Name).FirstOrDefault();

            //if (a == null)
            //{
            //    Album Album = new Album
            //    {
            //        Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Song.Album.Name),
            //        Artist = artist
            //    };
            //    Song.Album = Album;
            //    _context.Album.Add(Album);
            //}
            //else
            //{
            //    Song.Album = a;
            //}


            //if (g == null)
            //{
            //    g = new Genre
            //    {
            //        Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Song.Genre.Name.ToLower()),
            //        WordVec = Vspace.ConvertToString(_textprocessor.PredictText(Song.Genre.Name))
            //    };
            //    _context.Genre.Add(g);
            //}

            //Song.Genre = g;

            //Song.Filepath = ;

            Song.WordVec = VSpace.ConvertToString(
                VSpace.Normalize(VSpace.Add(_textprocessor.PredictText(Song.Lyrics), _textprocessor.PredictText(Song.Name))));


            _context.Song.Update(s);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(Song.SongID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }


        private bool SongExists(int id)
        {
            return _context.Song.Any(e => e.SongID == id);
        }
    }
}
