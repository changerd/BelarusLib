using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BelarusLib.Models
{
    public class Genre
    {
        public Guid GenreId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Composition> Compositions { get; set; }
        public Genre()
        {
            Compositions = new List<Composition>();
        }
    }
}