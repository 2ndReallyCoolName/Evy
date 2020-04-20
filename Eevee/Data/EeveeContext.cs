using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Eevee.Models;

namespace Eevee.Data
{
    public class EeveeContext : DbContext
    {
        public EeveeContext (DbContextOptions<EeveeContext> options)
            : base(options)
        {
        }

        public DbSet<Eevee.Models.AccountType> AccountType{ get; set; }

        public DbSet<Eevee.Models.Artist> Artist { get; set; }

        public DbSet<Eevee.Models.AdvertiserType> AdvertiserType { get; set; }
        
        public DbSet<Eevee.Models.Album> Album{ get; set; }

        public DbSet<Eevee.Models.Genre> Genre{ get; set; }

        public DbSet<Eevee.Models.History> History{ get; set; }

        public DbSet<Eevee.Models.Instrument> Instrument { get; set; }

        public DbSet<Eevee.Models.InstrumentManufacturer> InstrumentManufacturer { get; set; }

        public DbSet<Eevee.Models.InstrumentType> InstrumentType { get; set; }

        public DbSet<Eevee.Models.Note> Note{ get; set; }

        public DbSet<Eevee.Models.Playlist> Playlist{ get; set; }

        public DbSet<Eevee.Models.PlaylistSongAssignment> PlaylistSongAssignment{ get; set; }

        public DbSet<Eevee.Models.Song> Song { get; set; }

        public DbSet<Eevee.Models.SongInstrumentAssignment> SongInstrumentAssignment{ get; set; }

        public DbSet<Eevee.Models.User> User { get; set; }

        public DbSet<Eevee.Models.UserAccountTypeAssignment> UserAccountTypeAssignment { get; set; }
    }
}
