using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Eevee.Models;

namespace Eevee.Pages.Songs
{
    public class IndexModel : PageModel
    {
        public readonly Eevee.Data.EeveeContext _context;

        private readonly NaturalLanguage.NN.INN _textprocessor;

        private readonly int like_weight = 1;

        public IndexModel(Eevee.Data.EeveeContext context, NaturalLanguage.NN.INN textprocessor)
        {
            _context = context;

            _textprocessor = textprocessor;
        }

        public IList<Song> Song { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; } = "";

        public string msg = "";

        public string lv = "";

        public IList<Playlist> Playlists { get; set; }

        public async Task OnGetAsync(int? id)
        {

            int _id = Int32.Parse(HttpContext.User.Claims.Where(c => c.Type == "UserID").Select(c => c.Value).SingleOrDefault());
            Playlists = _context.Playlist.Where(p => p.User.UserID == _id).ToList();

            if (id != null)
            {
                Song = await _context.Song.Where(s => s.Album.Artist.ArtistID == id).Include(x => x.Genre).Include(x => x.Album).ThenInclude(x => x.Artist).ToListAsync();
                msg = Song.Count.ToString();

                //Song = await _context.Song.Where(s => s.Album.Artist.ArtistID == id)
            }
            else
            {
                if (SearchString.Length > 0)
                {
                    List<Song> songs = _context.Song.ToList();

                    var word_vector = _textprocessor.PredictText(SearchString);
                    lv = NaturalLanguage.vector.VectorSpace.ConvertToString(word_vector);

                    if (!string.IsNullOrEmpty(SearchString))
                    {
                        foreach (var song in songs)
                        {
                            msg += song.Name + ": " + NaturalLanguage.vector.VectorSpace.Loss(word_vector, NaturalLanguage.vector.VectorSpace.ToArray(song.WordVec)) + ", ";
                        }

                        songs.Sort((a, b) => NaturalLanguage.vector.VectorSpace.Loss(word_vector,
                            NaturalLanguage.vector.VectorSpace.ToArray(a.WordVec)).CompareTo(NaturalLanguage.vector.VectorSpace.Loss(word_vector, NaturalLanguage.vector.VectorSpace.ToArray(b.WordVec))));
                    }
                    Song = songs;
                }
                else
                {
                    Song = await _context.Song.Include(x => x.Genre).Include(x => x.Album).ThenInclude(x => x.Artist).ToListAsync(); 
                    msg = Song.Count.ToString();
                }
            }
        }

        public JsonResult OnPostAddSong(string song_id, string pl_id)
        {
          
            var playlist =_context.Playlist.Where(p=>p.PlaylistID == Int32.Parse(pl_id)).FirstOrDefault();
            var song = _context.Song.Where(s => s.SongID == Int32.Parse(song_id)).FirstOrDefault();
            PlaylistSongAssignment playlistSongAssignment = new PlaylistSongAssignment() { Playlist = playlist, Song = song };
            _context.PlaylistSongAssignment.Add(playlistSongAssignment);
            _context.SaveChanges();
            return new JsonResult("Added song to playlist");
        }

        public JsonResult OnPostIncreaseListen(string  song_id)
        {
            Song song = _context.Song.Where(s => s.SongID == Int32.Parse(song_id)).Include(x => x.Album).ThenInclude(x => x.Artist).FirstOrDefault();
            song.Listens += 1;
            Artist artist = _context.Artist.Where(a => a.ArtistID == song.Album.Artist.ArtistID).FirstOrDefault();
            artist.Listens += 1;
            _context.Song.Update(song);
            _context.Artist.Update(artist);
            _context.SaveChanges();
            return new JsonResult("Clicked play " + song_id);
        }

        public JsonResult OnPostLike(string song_id)
        {
            Song song = _context.Song.Where(s => s.SongID == Int32.Parse(song_id)).Include(x => x.Album).ThenInclude(x => x.Artist).FirstOrDefault();
            
            song.Rating += like_weight;

            Artist artist = _context.Artist.Where(a => a.ArtistID == song.Album.Artist.ArtistID).FirstOrDefault();

            artist.Rating += like_weight;

            _context.Song.Update(song);
            _context.Artist.Update(artist);
            _context.SaveChanges();
            return new JsonResult("Liked " + song_id);
        }

        public JsonResult OnPostDislike(string song_id)
        {
            Song song = _context.Song.Where(s => s.SongID == Int32.Parse(song_id)).Include(x => x.Album).ThenInclude(x => x.Artist).FirstOrDefault();
            
            song.Rating = song.Rating - like_weight;

            Artist artist = _context.Artist.Where(a => a.ArtistID == song.Album.Artist.ArtistID).FirstOrDefault();

            artist.Rating -= like_weight;

            _context.Song.Update(song);
            _context.Artist.Update(artist);
            _context.SaveChanges();
            return new JsonResult("Disliked " + song_id);
        }
    }
}
