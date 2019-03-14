namespace BelarusLib.Migrations
{
    using BelarusLib.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BelarusLib.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BelarusLib.Models.ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));
            var role1 = new ApplicationRole { Name = "Admin" };
            var role2 = new ApplicationRole { Name = "User" };
            roleManager.Create(role1);
            roleManager.Create(role2);
            var admin = new ApplicationUser { UserName = "admin" };
            string password = "admin123";
            var result = userManager.Create(admin, password);
            if (result.Succeeded)
            {
                userManager.AddToRole(admin.Id, role1.Name);                
            }
            base.Seed(context);
        }
    }
}
