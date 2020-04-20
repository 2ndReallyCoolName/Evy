using System.ComponentModel.DataAnnotations;


namespace Eevee.Models
{
    public class PlaylistSongAssignment
    {
        public int PlaylistSongAssignmentID { get; set; }

        [Required]
        public Playlist Playlist { get; set; }

        [Required]
         public Song Song { get; set; }
    }
}
