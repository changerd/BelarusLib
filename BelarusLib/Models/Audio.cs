using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BelarusLib.Models
{
    public class Audio
    {
        public int AudioId { get; set; }
        [Required]
        [Display(Name = "Назва")]
        public string AudioName { get; set; }
        [Required]
        [Display(Name = "Спасылка")]
        public string AudioLink { get; set; }
        [Required]
        [Display(Name = "Аўтар")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}