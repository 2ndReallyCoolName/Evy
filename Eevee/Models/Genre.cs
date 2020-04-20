﻿using System.ComponentModel.DataAnnotations;


namespace Eevee.Models
{
    public class Genre
    {
        public int GenreID { get; set; }
        [Required]
        public string Name { get; set; }

        public string WordVec { get; set; }
    }
}
