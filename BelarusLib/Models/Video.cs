using System.ComponentModel.DataAnnotations;

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
        [DataType(DataType.MultilineText)]
        public string VideoLink { get; set; }
        [Required]
        [Display(Name = "Аўтар")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}