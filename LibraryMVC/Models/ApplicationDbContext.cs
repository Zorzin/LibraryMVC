using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Library.Models;
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
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}