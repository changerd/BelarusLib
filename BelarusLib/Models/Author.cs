using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BelarusLib.Models
{
    public class Author
    {
        public Guid AuthorId { get; set; }
        public string AuthorFullName { get; set; }
        public byte[] AuthorPhoto { get; set; }
        public DateTime AuthorBirthDate { get; set; }
        public string AuthorBiography { get; set; }
        public virtual ICollection<Composition> Compositions { get; set; }
        public Author()
        {
            Compositions = new List<Composition>();
        }
    }
}