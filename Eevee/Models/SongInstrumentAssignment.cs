using System.ComponentModel.DataAnnotations;

namespace Eevee.Models
{
    public class SongInstrumentAssignment
    {
        public int SongInstrumentAssignmentID { get; set; }

        [Required]
        public int SongID { get; set; }

        [Required]
        public int InstrumentID { get; set; }
    }
}
