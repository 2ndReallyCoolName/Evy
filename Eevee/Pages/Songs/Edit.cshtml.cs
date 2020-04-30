﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eevee.Data;
using Eevee.Models;
using Vspace = NaturalLanguage.vector.VectorSpace;

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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            User user = _context.User.FirstOrDefault(u => u.Username == User.Identity.Name);

            Artist artist = _context.Artist.FirstOrDefault(a => a.ArtistID == user.UserID);

            Genre genre = Song.Genre;

            Song.WordVec = Vspace.ConvertToString(
                Vspace.Add(Vspace.Add(_textprocessor.PredictText(Song.Lyrics), Vspace.Scale(artist_contrib, Vspace.ToArray(artist.WordVec))),
                    Vspace.Scale(genre_contrib, Vspace.ToArray(genre.WordVec))));


            _context.Attach(Song).State = EntityState.Modified;

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
