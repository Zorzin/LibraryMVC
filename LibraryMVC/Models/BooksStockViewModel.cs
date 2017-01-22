using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryMVC.Models
{
    public class BooksStockViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Amount { get; set; }
        public int Borrowed { get; set; }
        public string ISBN { get; set; }
    }
}