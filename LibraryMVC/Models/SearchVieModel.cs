using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryMVC.Models
{
    public class SearchVieModel
    {
        public SearchVieModel()
        {
            SelectedCategory = 0;
            SelectedYear = 0;
        }
        public Book Book { get; set; }
        public IEnumerable<Book> Books { get; set; }
        public int[] SelectedLabels { get; set; }
        public int[] SelectedWriters { get; set; }
        public int SelectedCategory { get; set; }
        public string SelectedTitle { get; set; }
        public int SelectedYear { get; set; }
        public string SelectedISBN { get; set; }
    }
}