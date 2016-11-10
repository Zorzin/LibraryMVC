using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryMVC.Models
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }
        [Required]
        public string Name { get; set; }
        
        public int? OverCategoryID { get; set; }
    }
}