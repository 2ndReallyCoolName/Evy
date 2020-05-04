using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Eevee.Models;
using Vspace = NaturalLanguage.vector.VectorSpace;

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
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                int _id = Int32.Parse(HttpContext.User.Claims.Where(c => c.Type == "UserID").Select(c => c.Value).SingleOrDefault());
                Playlists = _context.Playlist.Where(p => p.User.UserID == _id).ToList();
            }

            if (id != null)
            {
                Song = await _context.Song.Where(s => s.Album.Artist.ArtistID == id).Include(x => x.Genre).Include(x => x.Album).ThenInclude(x => x.Artist).ToListAsync();

            }
            else
            {
                if (SearchString.Length > 0)
                {
                    SearchString = SearchString.ToLower();

                    List<Song> songs = _context.Song.Include(x => x.Genre).Include(x => x.Album).ThenInclude(x => x.Artist).ToList();


                    if (!string.IsNullOrEmpty(SearchString))
                    {

                        Song s = _context.Song.Where(x=>x.Name.ToLower() == SearchString).FirstOrDefault();

                        if (s != null)
                        {
                            int[] f = AudioAnalysis.Compare.ToArray(s.FreqVec);
                            songs = songs.OrderByDescending(x => AudioAnalysis.Compare.Similarity(f, AudioAnalysis.Compare.ToArray(x.FreqVec)) + 25*x.Rating/(double)x.Listens).ToList();
                        }
                        else
                        {
                            Artist a = _context.Artist.Where(x => x.Name.ToLower() == SearchString).FirstOrDefault();

                            if (a != null)
                            {
                                songs = _context.Song.Where(s => s.Album.Artist.ArtistID == a.ArtistID).ToList();
                                songs = songs.OrderByDescending(x => x.Rating / (double)x.Listens).ToList();
                            }
                            else
                            {
                                Genre g = _context.Genre.Where(x => x.Name.ToLower() == SearchString).FirstOrDefault();

                                if (g != null)
                                {
                                    songs = _context.Song.Where(s => s.Genre.GenreID == g.GenreID).ToList();
                                    songs = songs.OrderByDescending(x => x.Rating / (double)x.Listens).ToList();
                                }
                                else
                                {
                                    var word_vector = _textprocessor.PredictText(SearchString);

                                    songs.Sort((a, b) => 
                                    (Vspace.Loss(word_vector, Vspace.ToArray(a.WordVec)) - (a.Rating/Math.Pow(Math.E, -a.Listens)))
                                    .CompareTo(Vspace.Loss(word_vector, NaturalLanguage.vector.VectorSpace.ToArray(b.WordVec)) - (a.Rating / Math.Pow(Math.E, -a.Listens))));
                                }
                            }
                        }

                    }
                    Song = songs;
                }
                else
                {
                    Song = await _context.Song.Include(x => x.Genre).Include(x => x.Album).ThenInclude(x => x.Artist).ToListAsync();                     
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
