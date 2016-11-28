using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Library.Models;
using Label = Library.Models.Label;

namespace LibraryMVC.Models
{
    public class BookViewModel
    {
        public BookViewModel()
        {
            Files = new List<HttpPostedFileBase>();
            FilesNames = new List<string>();
            Writers = new List<Writer>();
            Labels = new List<Label>();
        }
        public int BookID { get; set; }
        [Required]
        [MinLength(1)]
        public string Title { get; set; }
        [Required]
        [MaxLength(13)]
        [MinLength(13)]
        public string ISBN { get; set; }
        [Required]
        [Range(0,3000)]
        public int Year { get; set; }
        [Required]
        [Range(0,Int32.MaxValue)]
        public int Amount { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public HttpPostedFileBase Contents { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public int[] SelectedWriters { get; set; }
        [Required]
        public int[] SelectedLabels { get; set; }
        public List<Writer> Writers { get; set; }
        public List<Label> Labels { get; set; }
        public List<HttpPostedFileBase> Files { get; set; }
        public List<string> FilesNames { get; set; }
    }

    public class BookEditViewModel
    {
        public BookEditViewModel()
        {
            BookViewModel =new BookViewModel();
            OldFiles = new List<string>();
            OldFilesText = new List<string>();
        }
        public BookViewModel BookViewModel { get; set; }
        public List<string> OldFiles { get; set; }
        public List<string> OldFilesText { get; set; }
        public string OldContent { get; set; }
    }
}