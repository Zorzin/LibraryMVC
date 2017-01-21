using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryMVC.Models;

namespace LibraryMVC.Controllers
{
    public class LibraryValuesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
     
        // GET: LibraryValues/Edit/5
        public ActionResult Edit()
        {
            LibraryValue libraryValue = db.LibraryValues.Find(1);
            if (libraryValue == null)
            {
                return HttpNotFound();
            }
            return View(libraryValue);
        }

        // POST: LibraryValues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MaxBorrows,MaxBorrowTime,TimeToCollectBook,MaxBooksToCollect")] LibraryValue libraryValue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(libraryValue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","WorkerPanel");
            }
            return View(libraryValue);
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
