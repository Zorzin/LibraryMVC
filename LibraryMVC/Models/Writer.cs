﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class Writer
    {
        public int WriterID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<BookWriter> BookWriters { get; set; }
        
    }
}