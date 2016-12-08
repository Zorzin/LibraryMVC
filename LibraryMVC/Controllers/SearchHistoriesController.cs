using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Library.Models;
using LibraryMVC.Models;

namespace LibraryMVC.Controllers
{
    [Authorize]
    public class SearchHistoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SearchHistories
        [Authorize(Roles = "Worker")]
        public ActionResult Index()
        {
            var searchHistories = db.SearchHistories.Include(s => s.Reader);
            return View(searchHistories.ToList());
        }
        public ActionResult UserIndex(string userid)
        {
            var searchHistories = db.SearchHistories.Where(s=>s.ReaderID==userid);
            return View(searchHistories.ToList());
        }


        // GET: SearchHistories/Details/5
        [Authorize(Roles = "Worker")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearchHistory searchHistory = db.SearchHistories.Find(id);
            if (searchHistory == null)
            {
                return HttpNotFound();
            }
            return View(searchHistory);
        }

        // GET: SearchHistories/Create
        [Authorize(Roles = "Worker")]
        public ActionResult Create()
        {
            ViewBag.ReaderID = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: SearchHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Worker")]
        public ActionResult Create([Bind(Include = "SearchHistoryID,ReaderID,Name,URL")] SearchHistory searchHistory)
        {
            if (ModelState.IsValid)
            {
                db.SearchHistories.Add(searchHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReaderID = new SelectList(db.Users, "Id", "Name", searchHistory.ReaderID);
            return View(searchHistory);
        }

        // GET: SearchHistories/Edit/5
        [Authorize(Roles = "Worker")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearchHistory searchHistory = db.SearchHistories.Find(id);
            if (searchHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReaderID = new SelectList(db.Users, "Id", "Name", searchHistory.ReaderID);
            return View(searchHistory);
        }

        // POST: SearchHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Worker")]
        public ActionResult Edit([Bind(Include = "SearchHistoryID,ReaderID,Name,URL")] SearchHistory searchHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(searchHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReaderID = new SelectList(db.Users, "Id", "Name", searchHistory.ReaderID);
            return View(searchHistory);
        }

        // GET: SearchHistories/Delete/5
        [Authorize(Roles = "Worker")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearchHistory searchHistory = db.SearchHistories.Find(id);
            if (searchHistory == null)
            {
                return HttpNotFound();
            }
            return View(searchHistory);
        }

        // POST: SearchHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Worker")]
        public ActionResult DeleteConfirmed(int id)
        {
            SearchHistory searchHistory = db.SearchHistories.Find(id);
            db.SearchHistories.Remove(searchHistory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
