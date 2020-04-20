using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Eevee.Data;
using Eevee.Models;

namespace Eevee.Pages.Artists
{
    public class DeleteModel : PageModel
    {
        private readonly Eevee.Data.EeveeContext _context;

        public DeleteModel(Eevee.Data.EeveeContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Artist Artist { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Artist = await _context.Artist.FirstOrDefaultAsync(m => m.ArtistID == id);

            if (Artist == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Artist = await _context.Artist.FindAsync(id);

            List<Album> albums = _context.Album.Where(a => a.Artist.ArtistID == Artist.ArtistID).ToList();

            List<Song> songs;
            foreach(var album in albums)
            {
                songs = _context.Song.Where(s => s.Album.AlbumID == album.AlbumID).ToList();
                foreach(var song in songs)
                {
                    var pa = _context.PlaylistSongAssignment.Where(p => p.Song.SongID == song.SongID).ToArray();
                    var sa = _context.SongInstrumentAssignment.Where(s => s.SongID == song.SongID).ToArray();

                    foreach(var p in pa)
                    {
                        _context.PlaylistSongAssignment.Remove(p);   
                    }

                    foreach (var s in sa)
                    {
                        _context.SongInstrumentAssignment.Remove(s);
                    }

                    _context.Song.Remove(song);
                }
                _context.Album.Remove(album);
            }


            if (Artist != null)
            {
                _context.Artist.Remove(Artist);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
