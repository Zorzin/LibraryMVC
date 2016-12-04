using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryMVC.Controllers
{
    public class WorkerPanelController : Controller
    {
        [Authorize(Roles = "Worker")]
        // GET: AdminPanel
        public ActionResult Index()
        {
            return View();
        }
    }
}