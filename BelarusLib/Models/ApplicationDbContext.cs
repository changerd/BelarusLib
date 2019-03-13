using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BelarusLib.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("ApplicationDbContext") {}

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<TypeComposition> TypeCompositions { get; set; }
    }
}