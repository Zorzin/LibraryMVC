using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LibraryMVC.Models;

namespace LibraryMVC.Controllers
{
    public class WorkerPanelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Worker")]
        // GET: AdminPanel
        public ActionResult Index()
        {
            var pendingusers = db.Users.Where(u => u.EmailConfirmed == false).ToList();
            return View(pendingusers);
        }
    }
}