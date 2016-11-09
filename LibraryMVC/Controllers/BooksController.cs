using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Library.Models;
using LibraryMVC.Models;

namespace LibraryMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var book = db.Books.Find(id);
            if (book == null)
                return HttpNotFound();
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
                var book = new Book()
                {
                    AddDate = DateTime.Today,
                    Amount = bookviewModel.Amount,
                    Contents = bookviewModel.Contents,
                    Title = bookviewModel.Title,
                    ISBN = bookviewModel.ISBN,
                    Year = bookviewModel.Year,
                    Description = bookviewModel.Description,
                    CategoryID = bookviewModel.CategoryID,
                    Category = db.Categories.FirstOrDefault(x => x.CategoryID == bookviewModel.CategoryID)
                };
                for (int i = 0; i < bookviewModel.SelectedLabels.Length; i++)
                {
                    var id = bookviewModel.SelectedLabels[i];
                    var label = db.Labels.FirstOrDefault(x => x.LabelID == id);
                    bookviewModel.Labels.Add(label);
                }
                for (int i = 0; i < bookviewModel.SelectedWriters.Length; i++)
                {
                    var id = bookviewModel.SelectedWriters[i];
                    var writer = db.Writers.FirstOrDefault(x => x.WriterID == id);
                    bookviewModel.Writers.Add(writer);
                }
                var writercounter = 0;
                foreach (var bookviewModelWriter in bookviewModel.Writers)
                {
                    var bookWriter = new BookWriter();
                    bookWriter.Book = book;
                    bookWriter.Writer = bookviewModelWriter;
                    bookWriter.Position = ++writercounter;
                    db.BookWriters.Add(bookWriter);
                }
                foreach (var bookviewModelLabel in bookviewModel.Labels)
                {
                    var bookLabel = new BookLabel();
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var book = db.Books
                    .Include(b => b.BookWriters)
                    .Include(b => b.BookLabels)
                    .Include(b => b.Category)
                    .Single(b => b.BookID == id);

            var bookViewModel = new BookViewModel
            {
                Amount = book.Amount,
                BookID = book.BookID,
                CategoryID = book.CategoryID,
                Contents = book.Contents,
                Description = book.Description,
                ISBN = book.ISBN,
                Title = book.Title,
                Year = book.Year
            };
            bookViewModel.SelectedLabels = new int[db.Labels.Count()];
            bookViewModel.SelectedWriters = new int[db.Writers.Count()];
            int i = 0;
            foreach (var bookBookWriter in book.BookWriters)
            {
                var writer = db.Writers.FirstOrDefault(x => x.WriterID == bookBookWriter.WriterID);
                bookViewModel.Writers.Add(writer);
                bookViewModel.SelectedWriters[i] = writer.WriterID;
                i++;
            }
            i = 0;
            foreach (var bookBookLabel in book.BookLabels)
            {
                var label = db.Labels.FirstOrDefault(x => x.LabelID == bookBookLabel.LabelID);
                bookViewModel.Labels.Add(label);
                bookViewModel.SelectedLabels[i] = label.LabelID;
                i++;
            }
            if (book == null)
                return HttpNotFound();
            IEnumerable<SelectListItem> writers = from w in db.Writers
                select new SelectListItem
                {
                    Value = w.WriterID.ToString(),
                    Text = w.Name + " " + w.Surname
                };
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name",bookViewModel.CategoryID);
            ViewBag.Writers = new SelectList(writers, "Value", "Text",bookViewModel.SelectedWriters);
            ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name",bookViewModel.SelectedLabels);
            return View(bookViewModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookViewModel bookviewModel)
        {
            if (ModelState.IsValid)
            {
                var book = db.Books
                    .Include(b => b.BookWriters)
                    .Include(b => b.BookLabels)
                    .Include(b => b.Category)
                    .Single(b => b.BookID == bookviewModel.BookID);

                var writerslist = new List<Writer>();
                for (int i = 0; i < bookviewModel.SelectedLabels.Length; i++)
                {
                    var id = bookviewModel.SelectedLabels[i];
                    var label = db.Labels.FirstOrDefault(x => x.LabelID == id);
                    bookviewModel.Labels.Add(label);
                }
                for (int i = 0; i < bookviewModel.SelectedWriters.Length; i++)
                {
                    var id = bookviewModel.SelectedWriters[i];
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
                var writercounter = 0;
                foreach (var bookviewModelWriter in bookviewModel.Writers)
                {
                    var bookWriter = new BookWriter();
                    bookWriter.Book = book;
                    bookWriter.Writer = bookviewModelWriter;
                    bookWriter.Position = ++writercounter;
                    db.BookWriters.Add(bookWriter);
                    db.SaveChanges();
                }
                foreach (var bookviewModelLabel in bookviewModel.Labels)
                {
                    var bookLabel = new BookLabel();
                    bookLabel.Book = book;
                    bookLabel.Label = bookviewModelLabel;
                    db.BookLabels.Add(bookLabel);
                    db.SaveChanges();
                }

                db.Entry(book).State = EntityState.Modified;
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

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var book = db.Books.Find(id);
            if (book == null)
                return HttpNotFound();
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}