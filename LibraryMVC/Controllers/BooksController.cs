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
            SearchVieModel svm = new SearchVieModel();
            var books = db.Books.ToList();
            svm.Books = books;
            ViewBag.Labels = new SelectList(db.Labels,"LabelID","Name");
            ViewBag.Categories = new SelectList(db.Categories,"CategoryID","Name");
            IEnumerable<SelectListItem> writers = from w in db.Writers
                                                  select new SelectListItem
                                                  {
                                                      Value = w.WriterID.ToString(),
                                                      Text = w.Name + " " + w.Surname
                                                  };
            ViewBag.Writers = new SelectList(writers, "Value", "Text");
            return View(svm);
        }

        // Post: Books
        [HttpPost]
        public ActionResult Index(SearchVieModel svm)
        {
            ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name");
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "Name");
            IEnumerable<SelectListItem> writers = from w in db.Writers
                                                  select new SelectListItem
                                                  {
                                                      Value = w.WriterID.ToString(),
                                                      Text = w.Name + " " + w.Surname
                                                  };
            ViewBag.Writers = new SelectList(writers, "Value", "Text");
            IEnumerable<Book> books = db.Books.ToList();

            if (!string.IsNullOrEmpty(svm.SelectedTitle))
            {
                books = books.Where(b => b.Title.Contains(svm.SelectedTitle)).ToList();
            }
            if (svm.SelectedCategory!=0)
            {
                books = books.Where(b => b.CategoryID == svm.SelectedCategory).ToList();
            }
            if (svm.SelectedLabels!=null)
            {
                ICollection<Book> selectedlabels = new List<Book>();
                foreach (var searchlabel in svm.SelectedLabels)
                {
                    int labelitem = db.Labels.FirstOrDefault(l => l.LabelID == searchlabel).LabelID;
                    var correctbooks = db.Books.Where(b => b.Labels.Any(bw => bw.Label.LabelID == labelitem));
                    foreach (var correctbook in correctbooks)
                    {
                        if (!selectedlabels.Contains(correctbook))
                        {
                            selectedlabels.Add(correctbook);
                        }
                    }
                }
                books = books.Intersect(selectedlabels);
            }
            if (svm.SelectedWriters!=null)
            {
                ICollection<Book> selectedwriters = new List<Book>();
                foreach (var searchwriter in svm.SelectedWriters)
                {
                    int writeritem = db.Writers.FirstOrDefault(w => w.WriterID == searchwriter).WriterID;
                    var correctbooks = db.Books.Where(b => b.Writers.Any(bw => bw.WriterID == writeritem));
                    foreach (var correctbook in correctbooks)
                    {
                        if (!selectedwriters.Contains(correctbook))
                        {
                            selectedwriters.Add(correctbook);
                        }
                    }
                }
                books = books.Intersect(selectedwriters);
            }
            if (!string.IsNullOrEmpty(svm.SelectedISBN))
            {
                books = books.Where(b => b.ISBN == svm.SelectedISBN).ToList();
            }
            if (svm.SelectedYear!=0)
            {
                books = books.Where(b => b.Year == svm.SelectedYear).ToList();
            }
            svm.Books = books;
            return View(svm);
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
        public ActionResult Create(BookViewModel bvm)
        {
            if (ModelState.IsValid)
            {
                Book book = new Book()
                {
                    AddDate = DateTime.Now,
                    Amount = bvm.Amount,
                    CategoryID = bvm.CategoryID,
                    Year = bvm.Year,
                    Title = bvm.Title,
                    Contents = bvm.Contents,
                    ISBN = bvm.ISBN,
                    Description = bvm.Description,
                    Writers = new List<BookWriter>(),
                    Labels = new List<BookLabel>()
                    
                };
                db.Books.Add(book);
                db.SaveChanges();
                for (int i = 0; i < bvm.SelectedWriters.Length; i++)
                {
                    var id = bvm.SelectedWriters[i];
                    var writer = db.Writers.FirstOrDefault(x => x.WriterID == id);
                    if (writer!=null)
                    {
                        var bw = new BookWriter()
                        {
                            BookID = book.BookID,
                            WriterID = writer.WriterID
                        };
                        db.BookWriters.Add(bw);
                        db.SaveChanges();
                    }
                }
                for (int i = 0; i < bvm.SelectedLabels.Length; i++)
                {
                    var id = bvm.SelectedLabels[i];
                    var label = db.Labels.FirstOrDefault(x => x.LabelID == id);
                    if (label!=null)
                    {
                        var bl = new BookLabel()
                        {
                            BookID = book.BookID,
                            LabelID = label.LabelID
                        };
                        db.BookLabels.Add(bl);
                        db.SaveChanges();
                    }
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
            return View(bvm);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var book = db.Books
                    .Include(b => b.Writers)
                    .Include(b => b.Labels)
                    .Include(b => b.Category)
                    .Single(b => b.BookID == id);

            var bvm = new BookViewModel
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
            var label = db.Labels
                .Include(l => l.BookLabels)
                .Single(l=>l.LabelID==1);
            bvm.SelectedLabels = new int[db.Labels.Count()];
            bvm.SelectedWriters = new int[db.Writers.Count()];
            for (int i = 0; i < book.Writers.Count; i++)
            {
                bvm.SelectedWriters[i] = book.Writers.ElementAt(i).WriterID;
            }
            for (int i = 0; i < book.Labels.Count; i++)
            {
                bvm.SelectedLabels[i] = book.Labels.ElementAt(i).LabelID;
            }
            
            if (book == null)
                return HttpNotFound();
            IEnumerable<SelectListItem> writers = from w in db.Writers
                select new SelectListItem
                {
                    Value = w.WriterID.ToString(),
                    Text = w.Name + " " + w.Surname
                };
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name",bvm.CategoryID);
            ViewBag.Writers = new SelectList(writers, "Value", "Text",bvm.SelectedWriters);
            ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name",bvm.SelectedLabels);
            return View(bvm);
        }
        
        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookViewModel bvm)
        {
            if (ModelState.IsValid)
            {
                var book = db.Books
                    .Include(b => b.Writers)
                    .Include(b => b.Labels)
                    .Include(b => b.Category)
                    .Single(b => b.BookID == bvm.BookID);
                book.Amount = bvm.Amount;
                book.Contents = bvm.Contents;
                book.Title = bvm.Title;
                book.ISBN = bvm.ISBN;
                book.Year = bvm.Year;
                book.Description = bvm.Description;
                book.Contents = bvm.Contents;
                book.CategoryID = bvm.CategoryID;
                book.Category = db.Categories.FirstOrDefault(x => x.CategoryID == bvm.CategoryID);
                var selectedwriterslist = new List<Writer>();
                var selectedlabelslist = new List<Label>();

                for (int i = 0; i < bvm.SelectedWriters.Length; i++)
                {
                    var id = bvm.SelectedWriters[i];
                    var writer = db.Writers.FirstOrDefault(x => x.WriterID == id);
                    if (writer!=null)
                    {
                        selectedwriterslist.Add(writer);
                    }
                }
                db.SaveChanges();
                for (int i = 0; i < bvm.SelectedLabels.Length; i++)
                {
                    var id = bvm.SelectedLabels[i];
                    var label = db.Labels.FirstOrDefault(x => x.LabelID == id);
                    if (label!=null)
                    {
                        selectedlabelslist.Add(label);
                    }
                }
                var actualwriters = db.Writers.Where(w => w.BookWriters.Any(b => b.BookID == bvm.BookID)).ToList();
                db.SaveChanges();
                foreach (var dbWriter in db.Writers.ToList())
                {
                    if (selectedwriterslist.Contains(dbWriter))
                    {
                        if (!actualwriters.Contains(dbWriter))
                        {
                            //book.Writers.Add(dbWriter);
                            var bw = new BookWriter()
                            {
                                WriterID = dbWriter.WriterID,
                                BookID = bvm.BookID
                            };
                            db.BookWriters.Add(bw);
                        }
                    }
                    else
                    {
                        if (actualwriters.Contains(dbWriter))
                        {
                            var bw =
                                db.BookWriters.FirstOrDefault(
                                    x => x.WriterID == dbWriter.WriterID && x.BookID == bvm.BookID);
                            db.BookWriters.Remove(bw);
                            //book.Writers.Remove(dbWriter);
                        }
                    }
                    db.SaveChanges();
                }
                var actuallabels = db.Labels.Where(w => w.BookLabels.Any(b => b.BookID == bvm.BookID)).ToList();
                db.SaveChanges();
                foreach (var dbLabel in db.Labels.ToList())
                {
                    if (selectedlabelslist.Contains(dbLabel))
                    {
                        if (!actuallabels.Contains(dbLabel))
                        {
                            var bl = new BookLabel()
                            {
                                BookID = bvm.BookID,
                                LabelID = dbLabel.LabelID
                            };
                            db.BookLabels.Add(bl);
                            //book.Labels.Add(dbLabel);
                        }

                    }
                    else
                    {
                        if (actuallabels.Contains(dbLabel))
                        {
                            var bl =
                                db.BookLabels.FirstOrDefault(x => x.LabelID == dbLabel.LabelID && x.BookID == bvm.BookID);
                            db.BookLabels.Remove(bl);
                            //book.Labels.Remove(dbLabel);
                        }
                    }
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
            return View(bvm);
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