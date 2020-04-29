using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Eevee.Data;
using Eevee.Models;

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

        public void OnGet()
        {

            int id = Int32.Parse(HttpContext.User.Claims.Where(c => c.Type == "UserID").Select(c => c.Value).SingleOrDefault());

            Playlists = _context.Playlist.Where(p => p.User.UserID == id).ToList();

            var playlist = Playlists.Where(p => p.Name == "Main").FirstOrDefault();
            
            Song =  _context.PlaylistSongAssignment.Where(p => p.Playlist.PlaylistID == playlist.PlaylistID).Select(s => s.Song).ToList();

        }

        public void OnGetPlayList(int pid)
        {

            int id = Int32.Parse(HttpContext.User.Claims.Where(c => c.Type == "UserID").Select(c => c.Value).SingleOrDefault());

            Playlists = _context.Playlist.Where(p => p.User.UserID == id).ToList();

            var playlist = Playlists.Where(p => p.PlaylistID == pid).FirstOrDefault();

            Song = _context.PlaylistSongAssignment.Where(p => p.Playlist.PlaylistID == playlist.PlaylistID).Select(s => s.Song).ToList();

        }

    }
}
