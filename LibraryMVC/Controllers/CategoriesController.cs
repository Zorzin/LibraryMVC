using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Library.Models;
using LibraryMVC.Models;

namespace LibraryMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.OverCategories = new SelectList(db.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category();
                if (categoryViewModel.OverCategoryID != null)
                {
                    category.OverCategoryID = categoryViewModel.OverCategoryID.Value;
                    category.OverCategory = db.Categories.FirstOrDefault(x => x.CategoryID == category.OverCategoryID);
                }
                category.Name = categoryViewModel.Name;
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoryViewModel);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.CategoryID = category.CategoryID;
            categoryViewModel.Name = category.Name;
            categoryViewModel.OverCategoryID = category.OverCategoryID;
            ViewBag.OverCategories = new SelectList(db.Categories.Where(x=> x.CategoryID!=category.CategoryID), "CategoryID", "Name");
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(categoryViewModel);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var category = db.Categories.FirstOrDefault(x => x.CategoryID == categoryViewModel.CategoryID);
                if (categoryViewModel.OverCategoryID != null)
                {
                    category.OverCategoryID = categoryViewModel.OverCategoryID.Value;
                    category.OverCategory = db.Categories.FirstOrDefault(x => x.CategoryID == category.OverCategoryID);
                }
                category.Name = categoryViewModel.Name;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoryViewModel);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
