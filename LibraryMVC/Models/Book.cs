using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Library.Models;

namespace LibraryMVC.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public int ISBN { get; set; }
        public DateTime Year { get; set; }
        public int Amount { get; set; }
        public DateTime AddDate { get; set; }
        public string Description { get; set; }
        public string Contents { get; set; }
        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<BookLabel> BookLabels { get; set; }
        public virtual ICollection<BookWriter> BookWriters { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Basket> Baskets { get; set; }
        public virtual ICollection<Borrow> Borrows { get; set; }
        public virtual ICollection<SearchHistory> SearchHistories { get; set; }
    }
}