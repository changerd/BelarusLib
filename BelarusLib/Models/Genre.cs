using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BelarusLib.Models
{
    public class Genre
    {               
        public int  GenreId { get; set; }
        [Required]
        [Display(Name = "Назва")]
        public string GenreName { get; set; }
        [Required]
        [Display(Name = "Апісанне")]
        [DataType(DataType.MultilineText)]
        public string GenreDescription { get; set; }
        public virtual ICollection<Composition> Compositions { get; set; }
        public Genre()
        {
            Compositions = new List<Composition>();
        }
    }
}