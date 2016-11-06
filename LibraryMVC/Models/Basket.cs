using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryMVC.Models
{
    public class Basket
    {
        public int BasketID { get; set; }

        public int ReaderID { get; set; }
        public int BookID { get; set; }

        public virtual User Reader { get; set; }
        public virtual Book Book { get; set; }
    }
}