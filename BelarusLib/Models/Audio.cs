using System.ComponentModel.DataAnnotations;

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
        [DataType(DataType.MultilineText)]
        public string AudioLink { get; set; }
        [Required]
        [Display(Name = "Аўтар")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}