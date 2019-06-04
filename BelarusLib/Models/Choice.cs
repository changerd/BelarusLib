using System.ComponentModel.DataAnnotations;

namespace BelarusLib.Models
{
    public class Choice
    {
        public int ChoiceId { get; set; }
        [Required]
        [Display(Name ="Назва")]
        public string ChoiceText { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}