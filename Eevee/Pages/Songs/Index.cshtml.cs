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

        public string similarity = "";

        User _User { get; set; }

        private float[] preference_vector { get; set; }

        public IList<Playlist> Playlists { get; set; }

        public async Task OnGetAsync(int? id)
        {
            preference_vector = Vspace.Ones(10);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                int _id = Int32.Parse(HttpContext.User.Claims.Where(c => c.Type == "UserID").Select(c => c.Value).SingleOrDefault());
                Playlists = _context.Playlist.Where(p => p.User.UserID == _id).ToList();
                _User = _context.User.Find(_id);
                preference_vector = Vspace.ToArray(_User.PreferenceVector);
            }

            if (id != null)
            {
                Song = await _context.Song.Where(s => s.Album.Artist.ArtistID == id).Include(x => x.Genre).Include(x => x.Album).ThenInclude(x => x.Artist).ToListAsync();
            }
            else
            {
                List<Song> songs = _context.Song.Include(x => x.Genre).Include(x => x.Album).ThenInclude(x => x.Artist).ToList();

                if (SearchString.Length > 0)
                {
                    SearchString = SearchString.ToLower();

                    if (!string.IsNullOrEmpty(SearchString))
                    {

                        Song s = _context.Song.Where(x=>x.Name.ToLower() == SearchString).FirstOrDefault();

                        if (s != null)
                        {
                            int[] f = AudioAnalysis.Compare.ToArray(s.FreqVec);
                            songs = songs.OrderByDescending(x => AudioAnalysis.Compare.Similarity(f, AudioAnalysis.Compare.ToArray(x.FreqVec)) + .01*x.Rating/(double)x.Listens).ToList();
                            foreach (var song in songs)
                            {
                                msg += song.Name + ":" + AudioAnalysis.Compare.Similarity(f, AudioAnalysis.Compare.ToArray(song.FreqVec)) + "; ";
                            }
                        }
                        else
                        {
                            Artist a = _context.Artist.Where(x => x.Name.ToLower() == SearchString).FirstOrDefault();

                            if (a != null)
                            {
                                songs = _context.Song.Where(s => s.Album.Artist.ArtistID == a.ArtistID).ToList();
                                songs = songs.OrderByDescending(x => .01 * x.Rating / (double)x.Listens).ToList();
                            }
                            else
                            {
                                Genre g = _context.Genre.Where(x => x.Name.ToLower() == SearchString).FirstOrDefault();

                                if (g != null)
                                {
                                    songs = _context.Song.Where(s => s.Genre.GenreID == g.GenreID).ToList();
                                    songs = songs.OrderByDescending(x => .01 * x.Rating / (double)x.Listens).ToList();
                                }
                                else
                                {
                                    var word_vector = _textprocessor.PredictText(SearchString);

                                    
                                    songs.Sort((a, b) => 
                                    (Vspace.Loss(word_vector, Vspace.ToArray(a.WordVec)).CompareTo(Vspace.Loss(word_vector, Vspace.ToArray(b.WordVec)))));
                                }
                            }
                        }

                    }
                }
                Song = songs;
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
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                int _id = Int32.Parse(HttpContext.User.Claims.Where(c => c.Type == "UserID").Select(c => c.Value).SingleOrDefault());
                _User = _context.User.Find(_id);
            }
                if (_User != null)
            {
                History history = _context.History.Where(x => x.User.UserID == _User.UserID && x.Song.SongID == Int32.Parse(song_id)).FirstOrDefault();

                Song song = _context.Song.Where(s => s.SongID == Int32.Parse(song_id)).Include(x => x.Album).ThenInclude(x => x.Artist).FirstOrDefault();
                if (history == null)
                {
                    _User.PreferenceVector = Vspace.ConvertToString(
                        Vspace.Normalize(Vspace.Add(Vspace.Scale(0.05f, Vspace.ToArray(song.WordVec)), Vspace.ToArray(_User.PreferenceVector))));
                    
                    song.Listens += 1;
                    Artist artist = _context.Artist.Where(a => a.ArtistID == song.Album.Artist.ArtistID).FirstOrDefault();
                    artist.Listens += 1;

                    History new_history = new History() { User = _User, Song = song, Progress = 0, Liked = 0 };
                    _context.User.Update(_User);
                    _context.Song.Update(song);
                    _context.Artist.Update(artist);
                    _context.History.Add(new_history);
                    _context.SaveChanges();
                    return new JsonResult("Listen1");
                }
                return new JsonResult("Listen2");
            }
            return new JsonResult("Clicked play " + song_id);
        }

        public JsonResult OnPostLike(string song_id, string val)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                int _id = Int32.Parse(HttpContext.User.Claims.Where(c => c.Type == "UserID").Select(c => c.Value).SingleOrDefault());
                _User = _context.User.Find(_id);
                preference_vector = Vspace.ToArray(_User.PreferenceVector);
            }

            int like = Int32.Parse(val);
            if (_User != null)
            {
                History history = _context.History.Where(x => x.User.UserID == _User.UserID && x.Song.SongID == Int32.Parse(song_id)).FirstOrDefault();
                
                Song song = _context.Song.Where(s => s.SongID == Int32.Parse(song_id)).Include(x => x.Album).ThenInclude(x => x.Artist).FirstOrDefault();

                if (history == null)
                {
                    history = new History() { User = _User, Song = song, Progress = 0, Liked = like };
                    _context.History.Add(history);
                    _context.SaveChanges();
                    return new JsonResult("updated 1");
                }

                if(history.Liked != like)
                {
                    song.Rating += like;

                    Artist artist = _context.Artist.Where(a => a.ArtistID == song.Album.Artist.ArtistID).FirstOrDefault();

                    artist.Rating += like;

                    history.Liked = like;

                    _User.PreferenceVector = Vspace.ConvertToString(
                        Vspace.Normalize(Vspace.Add(Vspace.Scale(like, Vspace.ToArray(song.WordVec)), Vspace.ToArray(_User.PreferenceVector))));

                    _context.Song.Update(song);
                    _context.Artist.Update(artist);
                    _context.History.Update(history);
                    _context.User.Update(_User);
                    _context.SaveChanges();
                }
                return new JsonResult("Like1");
            }
            return new JsonResult("updated");
        }
    }
}
