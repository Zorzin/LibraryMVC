using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryMVC.Models
{
    public class LibraryValue
    {
        public int Id { get; set; }
        [Range(0,Int32.MaxValue)]
        public int MaxBorrows { get; set; }
        [Range(0, Int32.MaxValue)]
        public int MaxBorrowTime { get; set; }
        [Range(0, Int32.MaxValue)]
        public int TimeToCollectBook { get; set; }
        [Range(0, Int32.MaxValue)]
        public int MaxBooksToCollect { get; set; }
    }
}