using System.ComponentModel.DataAnnotations;


namespace Eevee.Models
{
    public class Note
    {
        public int NoteID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public InstrumentType InstrumentType { get; set; }

        [Required]
        public string Filepath { get; set; }
    }
}
