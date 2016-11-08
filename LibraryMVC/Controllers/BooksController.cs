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
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Books
        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.Category);
            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            IEnumerable<SelectListItem> writers = from w in db.Writers
                                                  select new SelectListItem
                                                  {
                                                      Value = w.WriterID.ToString(),
                                                      Text = w.Name + " " + w.Surname
                                                  };
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Writers = new SelectList(writers, "Value", "Text");
            ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookViewModel bookviewModel)
        {
            if (ModelState.IsValid)
            {
                Book book = new Book();
                var writerslist = new List<Writer>();
                foreach (var bookSelectedLabel in bookviewModel.SelectedLabels)
                {
                    var id = bookSelectedLabel;
                    var label = db.Labels.FirstOrDefault(x => x.LabelID == id);
                    bookviewModel.Labels.Add(label);
                }
                foreach (var bookSelectedWriter in bookviewModel.SelectedWriters)
                {
                    var id = bookSelectedWriter;
                    var writer = db.Writers.FirstOrDefault(x => x.WriterID == id);
                    bookviewModel.Writers.Add(writer);
                }

                book.AddDate = DateTime.Today;
                book.Amount = bookviewModel.Amount;
                book.Contents = bookviewModel.Contents;
                book.Title = bookviewModel.Title;
                book.ISBN = bookviewModel.ISBN;
                book.Year = bookviewModel.Year;
                book.Description = bookviewModel.Description;
                book.Contents = bookviewModel.Contents;
                book.CategoryID = bookviewModel.CategoryID;
                book.Category = db.Categories.FirstOrDefault(x => x.CategoryID == bookviewModel.CategoryID);
                int writercounter = 0;
                foreach (var bookviewModelWriter in bookviewModel.Writers)
                {
                    BookWriter bookWriter = new BookWriter();
                    bookWriter.Book = book;
                    bookWriter.Writer = bookviewModelWriter;
                    bookWriter.Position = ++writercounter;
                    db.BookWriters.Add(bookWriter);
                }
                foreach (var bookviewModelLabel in bookviewModel.Labels)
                {
                    BookLabel bookLabel = new BookLabel();
                    bookLabel.Book = book;
                    bookLabel.Label = bookviewModelLabel;
                    db.BookLabels.Add(bookLabel);
                }
                db.Books.Add(book);
                db.SaveChanges();

                return RedirectToAction("Index");
            }


            IEnumerable<SelectListItem> writers = from w in db.Writers
                                                  select new SelectListItem
                                                  {
                                                      Value = w.WriterID.ToString(),
                                                      Text = w.Name + " " + w.Surname
                                                  };
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Writers = new SelectList(writers, "Value", "Text");
            ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name");
            return View(bookviewModel);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", book.CategoryID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,Title,ISBN,Year,Amount,AddDate,Description,Contents,CategoryID")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", book.CategoryID);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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
