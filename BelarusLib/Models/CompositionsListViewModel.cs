using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BelarusLib.Models
{
    public class CompositionsListViewModel
    {
        public IEnumerable<Composition> Compositions { get; set; }
        public PagingInfo PagingInfo { get; set; }        
    }
}