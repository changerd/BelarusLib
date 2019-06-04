using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace BelarusLib.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("ApplicationDbContext") {}

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Audio> Audios { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<Fact> Facts { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Photography> Photographies { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Video> Videos { get; set; }
    }
}