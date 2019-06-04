using System.ComponentModel.DataAnnotations;

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