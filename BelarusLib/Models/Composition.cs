using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BelarusLib.Models
{
    public class Composition
    {        
        public int CompositionId { get; set; }
        [Required]
        public string CompositionName { get; set; }
        [Required]
        public byte[] CompositionCover { get; set; }
        [Required]
        public string CompositionDescription { get; set; }
        [Required]
        public string CompositionLink { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        [Required]
        public int TypeCompositionId { get; set; }
        public TypeComposition TypeComposition { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public Composition()
        {
            Genres = new List<Genre>();
        }
    } 
}