using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BelarusLib.Models
{
    public class Author
    {        
        public int AuthorId { get; set; }
        [Required]
        [Display(Name = "Імя")]
        public string AuthorFullName { get; set; }
        [Required]
        [Display(Name = "Фотаздымак")]
        public byte[] AuthorPhoto { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата нараджэння")]
        public DateTime AuthorBirthDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата смерці")]
        public DateTime? AuthorBirthDeath { get; set; }
        [Required]
        [Display(Name = "Месца нараджэння")]
        public string AuthorBirthPlace { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Біяграфія")]
        public string AuthorBiography { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Кароткае апісанне")]
        public string AuthorShortDescription { get; set; }
        public virtual ICollection<Composition> Compositions { get; set; }
        public virtual ICollection<Audio> Audios { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
        public virtual ICollection<Fact> Facts { get; set; }
        public virtual ICollection<Photography> Photographies { get; set; }
        public Author()
        {
            Compositions = new List<Composition>();
            Audios = new List<Audio>();
            Videos = new List<Video>();
            Facts = new List<Fact>();
            Photographies = new List<Photography>();
        }
    }
}