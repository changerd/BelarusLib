using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BelarusLib.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        [Required]
        [Display(Name = "Назва")]
        public string QuizName { get; set; }
        [Required]
        [Display(Name = "Працягласць")]
        public TimeSpan QuizDuration { get; set; }
        [Display(Name = "Апісанне")]
        public string QuizDescription { get; set; }
        [Required]
        [Display(Name = "Закрыты?")]
        public bool QuizIsPrivate { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Result> Results { get; set; }        
        public Quiz()
        {
            Questions = new List<Question>();
            Results = new List<Result>();            
        }
    }
}