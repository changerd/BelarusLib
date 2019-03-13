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
        public string AuthorFullName { get; set; }
        [Required]
        public byte[] AuthorPhoto { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime AuthorBirthDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? AuthorBirthDeath { get; set; }
        [Required]
        public string AuthorBirthPlace { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string AuthorBiography { get; set; }
        public virtual ICollection<Composition> Compositions { get; set; }
        public Author()
        {
            Compositions = new List<Composition>();
        }
    }
}