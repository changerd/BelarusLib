using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BelarusLib.Models
{
    public class Fact
    {
        public int FactId { get; set; }
        [Required]
        [Display(Name = "Факт")]
        public string FactText { get; set; }
        [Required]
        [Display(Name = "Аўтар")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}