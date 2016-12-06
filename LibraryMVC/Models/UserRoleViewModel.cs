using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LibraryMVC.Models
{
    public class UserRoleViewModel
    {
        public string userid { get; set; }
        public bool[] CheckedRoles { get; set; }
        public List<string> AllRoles { get; set; }
    }
}