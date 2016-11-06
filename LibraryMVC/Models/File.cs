using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Library.Models;
using LibraryMVC.Models;

namespace Library.Models
{
    public class File
    {
        public int FileID { get; set; }
        public int BookID { get; set; }
        public string Source { get; set; }
        public string Name { get; set; }

        public virtual Book Book { get; set; }
    }
}