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
        [Display (Name = "Назва")]
        public string CompositionName { get; set; }
        [Required]
        [Display(Name = "Вокладка")]
        public byte[] CompositionCover { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Апісанне")]
        public string CompositionDescription { get; set; }
        [Required]
        [Display(Name = "Спасылка")]
        public string CompositionLink { get; set; }
        [Required]
        [Display(Name = "Аўтар")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }        
        public virtual ICollection<Genre> Genres { get; set; }
        public Composition()
        {
            Genres = new List<Genre>();
        }
    } 
}