using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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