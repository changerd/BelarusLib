using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BelarusLib.Models
{
    public class Video
    {
        public int VideoId { get; set; }
        [Required]
        [Display(Name = "Назва")]
        public string VideoName { get; set; }
        [Required]
        [Display(Name = "Спасылка")]
        public string VideoLink { get; set; }
        [Required]
        [Display(Name = "Аўтар")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}