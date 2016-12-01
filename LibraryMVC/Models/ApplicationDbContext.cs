using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Library.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LibraryMVC.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("CS", throwIfV1Schema: false)
        {
        }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookLabel> BookLabels { get; set; }
        public DbSet<BookWriter> BookWriters { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }
        public DbSet<File> Files { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public static class IdentityManager
    {
        public static RoleManager<IdentityRole> LocalRoleManager
        {
            get
            {
                return new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            }
        }


        public static UserManager<User> LocalUserManager
        {
            get
            {
                return new UserManager<User>(new UserStore<User>(new ApplicationDbContext()));
            }
        }

        public static User GetUserById(string id)
        {
            User user = null;

            var usermanager = LocalUserManager;
            user = usermanager.FindById(id);

            return user;
        }
    }
}