using Eevee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eevee.Data
{
    public class ArtistDataAccess
    {
        private readonly EeveeContext _context;

        public ArtistDataAccess(EeveeContext context)
        {
            _context = context;
        }

        public IQueryable<Artist> Get(string name)
        {
            return from a in _context.Artist where a.Name.Contains(name) select a;
        }

        public IQueryable<Artist> Get(int id)
        {
            return from a in _context.Artist where a.ArtistID == id select a;
        }

        public void Attach(Microsoft.EntityFrameworkCore.EntityState state, Artist artist)
        {
            _context.Attach(artist).State = state;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool Exists(int id)
        {
            return _context.Artist.Any(e => e.ArtistID == id);
        }

        public async Task<Artist> Find(int id)
        {
            return await _context.Artist.FindAsync(id);
        }

        public async Task Delete(int id)
        {
            Artist Artist = await Find(id);

            List<Album> albums = _context.Album.Where(a => a.Artist.ArtistID == Artist.ArtistID).ToList();

            List<Song> songs;
            foreach (var album in albums)
            {
                songs = _context.Song.Where(s => s.Album.AlbumID == album.AlbumID).ToList();
                foreach (var song in songs)
                {
                    var pa = _context.PlaylistSongAssignment.Where(p => p.Song.SongID == song.SongID).ToArray();
                    var sa = _context.SongInstrumentAssignment.Where(s => s.SongID == song.SongID).ToArray();

                    foreach (var p in pa)
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
        }

        public async Task Create(int id, Artist Artist)
        {

            _context.Artist.Add(Artist);

            UserAccountTypeAssignment uata = _context.UserAccountTypeAssignment.Where(x => x.User.UserID == id).FirstOrDefault();

            AccountType accountType = _context.AccountType.Single(a => a.AccountTypeID == 1);

            uata.AccountType = accountType;

            //_context.UserAccountTypeAssignment.Update(uata);

            await _context.SaveChangesAsync();
        }

    }
}
