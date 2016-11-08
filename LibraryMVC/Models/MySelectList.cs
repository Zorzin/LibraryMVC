using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryMVC.Models
{
    public class MySelectList : SelectList
    {
        public MySelectList() : base(new SelectListItem [] { })
        {

        }
    }
}