using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BelarusLib.Models
{
    public class TypeComposition
    {        
        public int TypeCompositionId { get; set; }
        [Required]
        [Display(Name ="Назва")]
        public string TypeCompositionName { get; set; }
        [Required]
        [Display(Name = "Апісанне")]
        public string TypeCompositionDescription { get; set; }
        public virtual ICollection<Composition> Compositions { get; set; }
        public TypeComposition()
        {
            Compositions = new List<Composition>();
        }
    }
}