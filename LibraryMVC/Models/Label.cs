using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class Label
    {
        public int LabelID { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<BookLabel> BookLabels { get; set; }
    }
}


