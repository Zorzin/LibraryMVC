using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LibraryMVC.Models;

namespace Library.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Over Category")]
        [Required]
        public int OverCategoryID { get; set; }

        public virtual ICollection<Book> Books { get; set; }
        public virtual Category OverCategory { get; set; }
    }
}