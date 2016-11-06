using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using LibraryMVC.Models;

namespace Library.Models
{
    public class BookWriter
    {
        public int BookWriterID { get; set; }
        public int WriterID { get; set; }
        public int BookID { get; set; }
        public int Position { get; set; }
        
        public virtual Writer Writer { get; set; }
        public virtual Book Book { get; set; }

    }
}