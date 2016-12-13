using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Library.Models;
using System.Globalization;

namespace LibraryMVC.Models
{
    
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int Year { get; set; }
        public int Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime AddDate { get; set; }
        public string Description { get; set; }
        public string Contents { get; set; }
        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<BookLabel> Labels { get; set; }
        public virtual ICollection<BookWriter> Writers { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Borrow> Borrows { get; set; }
    }
    
}