using System.ComponentModel.DataAnnotations;


namespace Eevee.Models
{
    public class History
    {
        public int HistoryID {get; set;}

        [Required]
        public User User { get; set; }

        [Required]
        public Song Song { get; set; }

        [Required]
        public float Progress { get; set; }

        public int Liked { get; set; }
    }
}
