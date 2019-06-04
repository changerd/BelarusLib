using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BelarusLib.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        [Required]
        [Display(Name = "Тэкст пытання")]
        public string QuestionText { get; set; }
        [Display(Name = "Адказ")]
        public string QuestionAnswer { get; set; }
        [Display(Name = "Апісанне")]
        public string QuestionDescription { get; set; }
        [Display(Name = "Малюнак")]
        public byte[] QuestionImage { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }        
        public virtual ICollection<Choice> Choices { get; set; }
        public Question()
        {            
            Choices = new List<Choice>();
        }
    }
}