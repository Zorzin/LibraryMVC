using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryMVC.Models
{
    public class Basket
    {
        public Basket()
        {
            Books = new List<Book>();
        }
        public int BasketID { get; set; }
        public virtual ICollection<Book> Books { get; set; }

    }
}