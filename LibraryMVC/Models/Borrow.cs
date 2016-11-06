using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using LibraryMVC.Models;

namespace Library.Models
{
    public class Borrow
    {
        public int BorrowID { get; set; }
        public int BookID { get; set; }
        public int ReaderID { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }

        public virtual User Reader { get; set; }
        public virtual Book Book { get; set; }
    }
}