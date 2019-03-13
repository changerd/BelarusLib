using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BelarusLib.Models
{
    public class TypeComposition
    {
        public Guid TypeCompositionId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Composition> Compositions { get; set; }
        public TypeComposition()
        {
            Compositions = new List<Composition>();
        }
    }
}