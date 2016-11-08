using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Models;

namespace LibraryMVC.Models
{
    public class BookViewModel
    {
        public BookViewModel()
        {
            Writers = new List<Writer>();
            Labels = new List<Label>();
        }

        [Required]
        public string Title { get; set; }
        [Required]
        public int ISBN { get; set; }
        [Required]
        public DateTime Year { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Contents { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public int[] SelectedWriters { get; set; }
        [Required]
        public int[] SelectedLabels { get; set; }
        public List<Writer> Writers { get; set; }
        public List<Label> Labels { get; set; }
    }
}