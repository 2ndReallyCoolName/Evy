using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Eevee.Data;
using Eevee.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eevee.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public readonly EeveeContext _context;

        public IndexModel(ILogger<IndexModel> logger, EeveeContext context)
        {
            _logger = logger;
            _context = context;
        }

        public string msg { get; set; }

        public IList<Song> Song { get; set; }

        public IList<Playlist> Playlists { get; set; }

        public Playlist Playlist { get; set; }

        public async Task OnGetAsync()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                int id = Int32.Parse(HttpContext.User.Claims.Where(c => c.Type == "UserID").Select(c => c.Value).FirstOrDefault());

                Playlists = await _context.Playlist.Where(p => p.User.UserID == id).ToListAsync();

                Playlist = Playlists.Where(p => p.Name == "Main").FirstOrDefault();

                Song = await _context.PlaylistSongAssignment.Where(p => p.Playlist.PlaylistID == Playlist.PlaylistID)
                    .Include(x => x.Song).ThenInclude(x => x.Genre)
                    .Include(x => x.Song).ThenInclude(x => x.Album).ThenInclude(x => x.Artist)
                    .Select(s => s.Song).ToListAsync();
            }
        }

        public JsonResult OnPostSelectPL(string pl_id)
        {
            int id = Int32.Parse(HttpContext.User.Claims.Where(c => c.Type == "UserID").Select(c => c.Value).FirstOrDefault());

            Playlist = _context.Playlist.Where(p => p.User.UserID == id && p.PlaylistID == Int32.Parse(pl_id)).FirstOrDefault();

            Song =  _context.PlaylistSongAssignment.Where(p => p.Playlist.PlaylistID == Playlist.PlaylistID)
                .Include(x => x.Song).ThenInclude(x => x.Genre)
                .Include(x => x.Song).ThenInclude(x => x.Album).ThenInclude(x => x.Artist)
                .Select(s => s.Song).ToList();

            return new JsonResult("changed playlist");
        }

        public JsonResult OnPostRemoveSongFromPlaylist(string song_id, string pl_id)
        {

            PlaylistSongAssignment assignment = _context.PlaylistSongAssignment
                .Where(x => x.Song.SongID == Int32.Parse(song_id) && x.Playlist.PlaylistID == Int32.Parse(pl_id)).FirstOrDefault();

            if (assignment != null)
            {
                _context.PlaylistSongAssignment.Remove(assignment);
                _context.SaveChanges();
            }
            return new JsonResult("removed song from playlist");
        }
    }
}
