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
        public string ReaderID { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:dd/mm/yy}")]
        public DateTime BorrowDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:dd\/MM\/yyyy}")]
        public DateTime ReturnDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:dd\/MM\/yyyy}")]
        public DateTime Deadline { get; set; }
        public string Status { get; set; }

        public virtual User Reader { get; set; }
        public virtual Book Book { get; set; }
    }
}