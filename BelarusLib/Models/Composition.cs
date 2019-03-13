using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BelarusLib.Models
{
    public class Composition
    {
        public Guid CompositionId { get; set; }
        public string CompositionName { get; set; }
        public string CompositionDescription { get; set; }
        public string CompositionLink { get; set; }
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
        public Guid TypeCompositionId { get; set; }
        public TypeComposition TypeComposition { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public Composition()
        {
            Genres = new List<Genre>();
        }
            

    } 
}