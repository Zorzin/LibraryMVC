using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using LibraryMVC.Models;

namespace Library.Models
{
    public class SearchHistory
    {
        public int SearchHistoryID { get; set; }
        public int ReaderID { get; set; }
        public int BookID { get; set; }
        public string Name { get; set; }

        public virtual User Reader { get; set; }
        public virtual Book Book { get; set; }

    }
}