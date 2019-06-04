using System;
using System.ComponentModel.DataAnnotations;

namespace BelarusLib.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        [Display (Name = "Адзнака")]
        public int ResultScore { get; set; }
        [Display(Name = "Дата")]
        public DateTime ResultDate { get; set; }
        public int QuizId { get; set; }
        public string UserId { get; set; }
        public Quiz Quiz { get; set; }
        public ApplicationUser User { get; set; }

    }
}