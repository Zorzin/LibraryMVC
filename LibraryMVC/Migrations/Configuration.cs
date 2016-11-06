using LibraryMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LibraryMVC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LibraryMVC.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LibraryMVC.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            SeedRoles(context);
            SeedAdmin(context);
        }

        private void SeedRoles(LibraryMVC.Models.ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var role = roleManager.FindByName("Admin");
            if (role == null)
            {
                role = new IdentityRole("Admin");
                roleManager.Create(role);

            }
        }

        private void SeedAdmin(LibraryMVC.Models.ApplicationDbContext context)
        {
            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);

            var adminUser = userManager.FindByName("admin");
            if (adminUser == null)
            {
                var admin = new User()
                {
                    UserName = "admin",
                    Name = "Admin",
                    Surname = "Adminowski",
                    Email = "admin@aa.aa",
                    PhoneNumber = "111222333",
                    LockoutEnabled = false,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PasswordHash = new PasswordHasher().HashPassword("admin")
                };
                var adminResult = userManager.Create(admin);
                if (adminResult.Succeeded)
                {
                    userManager.AddToRole(admin.Id, "Admin");

                }
            }

        }
    }
}
