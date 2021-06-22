namespace application_development.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using application_development.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    internal sealed class Configuration : DbMigrationsConfiguration<application_development.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(application_development.Models.ApplicationDbContext context)
        {
            context.Roles.AddOrUpdate(x => x.Id, new IdentityRole("Admin"), new IdentityRole("Staff"), new IdentityRole("Trainer"), new IdentityRole("Trainee"));
        }
    }
}
