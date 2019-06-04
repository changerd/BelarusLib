using System.ComponentModel.DataAnnotations;

namespace BelarusLib.Models
{
    public class Photography
    {
        public int PhotographyId { get; set; }
        [Required]
        [Display(Name = "Фотаздымак")]
        public byte[] PhotographyPhoto { get; set; }
        [Required]
        [Display(Name = "Аўтар")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}