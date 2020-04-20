using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eevee.Models
{
    public class Artist
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ArtistID { get; set; }
        [Required]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        public string Description { get; set; }

        public int Listens { get; set; } = 0;

        public float Rating { get; set; } = 1.0f;

        public string WordVec { get; set; }

    }
}
