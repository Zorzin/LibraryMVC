using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using LibraryMVC.Models;

namespace Library.Models
{
    public class BookLabel
    {
        public int BookLabelID { get; set; }
        public int BookID { get; set; }
        public int LabelID { get; set; }

        public virtual Book Book { get; set; }
        public virtual Label Label { get; set; }
    }
}