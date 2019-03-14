using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BelarusLib.Models
{
    public class Author
    {        
        public int AuthorId { get; set; }
        [Required]
        [Display(Name = "Імя")]
        public string AuthorFullName { get; set; }
        [Required]
        [Display(Name = "Фотаздымак")]
        public byte[] AuthorPhoto { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата нараджэння")]
        public DateTime AuthorBirthDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата смерці")]
        public DateTime? AuthorBirthDeath { get; set; }
        [Required]
        [Display(Name = "Месца нараджэння")]
        public string AuthorBirthPlace { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Біяграфія")]
        public string AuthorBiography { get; set; }
        public virtual ICollection<Composition> Compositions { get; set; }
        public Author()
        {
            Compositions = new List<Composition>();
        }
    }
}