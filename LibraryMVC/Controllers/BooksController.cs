using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Library.Models;
using LibraryMVC.Models;
using File = Library.Models.File;

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
                var extension = Path.GetExtension(bvm.Contents.FileName);
                var filename = bvm.BookID + "_contents" + extension;
                Book book = new Book()
                {
                    AddDate = DateTime.Now,
                    Amount = bvm.Amount,
                    CategoryID = bvm.CategoryID,
                    Year = bvm.Year,
                    Title = bvm.Title,
                    Contents = filename,
                    ISBN = bvm.ISBN,
                    Description = bvm.Description,
                    Writers = new List<BookWriter>(),
                    Labels = new List<BookLabel>()
                    
                };
                db.Books.Add(book);
                db.SaveChanges();
                var directory =Path.Combine(Server.MapPath("~/App_Data/uploads"),book.BookID.ToString());
                Directory.CreateDirectory(directory);
                var path = Path.Combine(directory, filename);
                bvm.Contents.SaveAs(path);
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

                for (var i=0;i<bvm.Files.Count;i++) 
                {
                    if (bvm.Files[i] != null)
                    {
                        File file = new File();
                        file.BookID = book.BookID;
                        file.Book = book;
                        file.Name = bvm.FilesNames[i];
                        file.Source =bvm.Files[i].FileName;
                        bvm.Files[i].SaveAs(Path.Combine(directory, bvm.Files [i].FileName));
                        db.Files.Add(file);
                        db.SaveChanges();
                    }
                }


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

            var fileslist = db.Files.Where(f => f.BookID == book.BookID);
            var filessourcelist = fileslist.Select(f => f.Source).ToList();
            var filestextlist = fileslist.Select(f => f.Name).ToList();

            var bvm = new BookEditViewModel
            {
                BookViewModel=new BookViewModel()
                {
                    Amount = book.Amount,
                    BookID = book.BookID,
                    CategoryID = book.CategoryID,
                    Description = book.Description,
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Year = book.Year,
                    Files = new List<HttpPostedFileBase>(),
                    
                },
                OldContent = book.Contents,
                OldFiles = filessourcelist,
                OldFilesText = filestextlist
            };
            bvm.BookViewModel.SelectedLabels = new int[db.Labels.Count()];
            bvm.BookViewModel.SelectedWriters = new int[db.Writers.Count()];
            for (int i = 0; i < book.Writers.Count; i++)
            {
                bvm.BookViewModel.SelectedWriters[i] = book.Writers.ElementAt(i).WriterID;
            }
            for (int i = 0; i < book.Labels.Count; i++)
            {
                bvm.BookViewModel.SelectedLabels[i] = book.Labels.ElementAt(i).LabelID;
            }
            
            if (book == null)
                return HttpNotFound();
            IEnumerable<SelectListItem> writers = from w in db.Writers
                select new SelectListItem
                {
                    Value = w.WriterID.ToString(),
                    Text = w.Name + " " + w.Surname
                };
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name",bvm.BookViewModel.CategoryID);
            ViewBag.Writers = new SelectList(writers, "Value", "Text",bvm.BookViewModel.SelectedWriters);
            ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name",bvm.BookViewModel.SelectedLabels);
            return View(bvm);
        }
        
        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookEditViewModel bevm)
        {
            if (bevm.BookViewModel.Contents==null && !string.IsNullOrEmpty(bevm.OldContent))
            {
                ModelState["BookViewModel.Contents"].Errors.Clear();
            }
            if (ModelState.IsValid)
            {
                
                var book = db.Books
                    .Include(b => b.Writers)
                    .Include(b => b.Labels)
                    .Include(b => b.Category)
                    .Single(b => b.BookID == bevm.BookViewModel.BookID);

                var directory =Path.Combine(Server.MapPath("~/App_Data/uploads"),book.BookID.ToString());
                string filename;
                if (bevm.BookViewModel.Contents!=null)
                {
                    var oldfilepath = Path.Combine(directory, book.Contents);
                    if (System.IO.File.Exists(oldfilepath))
                    {
                        System.IO.File.Delete(oldfilepath);
                    }
                    var extension = Path.GetExtension(bevm.BookViewModel.Contents.FileName);
                    filename = bevm.BookViewModel.BookID + "_contents" + extension;
                    bevm.BookViewModel.Contents.SaveAs(Path.Combine(directory, filename));
                }
                else
                {
                    filename = bevm.OldContent;
                }
                
                book.Amount = bevm.BookViewModel.Amount;
                book.Contents = filename;
                book.Title = bevm.BookViewModel.Title;
                book.ISBN = bevm.BookViewModel.ISBN;
                book.Year = bevm.BookViewModel.Year;
                book.Description = bevm.BookViewModel.Description;
                book.CategoryID = bevm.BookViewModel.CategoryID;
                book.Category = db.Categories.FirstOrDefault(x => x.CategoryID == bevm.BookViewModel.CategoryID);
                var selectedwriterslist = new List<Writer>();
                var selectedlabelslist = new List<Label>();

                for (int i = 0; i < bevm.BookViewModel.SelectedWriters.Length; i++)
                {
                    var id = bevm.BookViewModel.SelectedWriters[i];
                    var writer = db.Writers.FirstOrDefault(x => x.WriterID == id);
                    if (writer!=null)
                    {
                        selectedwriterslist.Add(writer);
                    }
                }
                db.SaveChanges();
                for (int i = 0; i < bevm.BookViewModel.SelectedLabels.Length; i++)
                {
                    var id = bevm.BookViewModel.SelectedLabels[i];
                    var label = db.Labels.FirstOrDefault(x => x.LabelID == id);
                    if (label!=null)
                    {
                        selectedlabelslist.Add(label);
                    }
                }

                //Check Writers
                var actualwriters = db.Writers.Where(w => w.BookWriters.Any(b => b.BookID == bevm.BookViewModel.BookID)).ToList();
                db.SaveChanges();
                foreach (var dbWriter in db.Writers.ToList())
                {
                    if (selectedwriterslist.Contains(dbWriter))
                    {
                        if (!actualwriters.Contains(dbWriter))
                        {
                            var bw = new BookWriter()
                            {
                                WriterID = dbWriter.WriterID,
                                BookID = bevm.BookViewModel.BookID
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
                                    x => x.WriterID == dbWriter.WriterID && x.BookID == bevm.BookViewModel.BookID);
                            db.BookWriters.Remove(bw);
                        }
                    }
                    db.SaveChanges();
                }

                //Check Labels
                var actuallabels = db.Labels.Where(w => w.BookLabels.Any(b => b.BookID == bevm.BookViewModel.BookID)).ToList();
                db.SaveChanges();
                foreach (var dbLabel in db.Labels.ToList())
                {
                    if (selectedlabelslist.Contains(dbLabel))
                    {
                        if (!actuallabels.Contains(dbLabel))
                        {
                            var bl = new BookLabel()
                            {
                                BookID = bevm.BookViewModel.BookID,
                                LabelID = dbLabel.LabelID
                            };
                            db.BookLabels.Add(bl);
                        }

                    }
                    else
                    {
                        if (actuallabels.Contains(dbLabel))
                        {
                            var bl =
                                db.BookLabels.FirstOrDefault(x => x.LabelID == dbLabel.LabelID && x.BookID == bevm.BookViewModel.BookID);
                            db.BookLabels.Remove(bl);
                        }
                    }
                    db.SaveChanges();
                }

                //Check Files
                List<string> editedfilessource;
                List<string> editedfilestext;
                if (bevm.OldFiles!=null)
                {
                    editedfilessource = bevm.OldFiles;
                }
                else
                {
                    editedfilessource = new List<string>();
                }
                if (bevm.OldFilesText!=null)
                {
                     editedfilestext = bevm.OldFilesText;
                }
                else
                {
                    editedfilestext = new List<string>();
                }
                var allfiles = db.Files.Where(f => f.BookID == bevm.BookViewModel.BookID).ToList();
                var allfilessource = allfiles.Select(f => f.Source).ToList();

                //Add new files to list of sources and disc
                if (bevm.BookViewModel.Files!=null)
                {
                    for (var i = 0; i < bevm.BookViewModel.Files.Count; i++)
                    {
                        if (bevm.BookViewModel.Files[i]!=null)
                        {
                            var source = bevm.BookViewModel.Files[i].FileName;
                            var path = Path.Combine(directory, source);
                            bevm.BookViewModel.Files [i].SaveAs(path);
                            editedfilessource.Add(source);
                            editedfilestext.Add(bevm.BookViewModel.FilesNames [i]);
                        }
                    }
                }

                foreach (var dbfile in allfiles)
                {
                    //if in base exist file that we remove while editing, then remove it from db and from disc
                    if (!editedfilessource.Contains(dbfile.Source))
                    {
                        if (System.IO.File.Exists(Path.Combine(directory,dbfile.Source)))
                        {
                            System.IO.File.Delete(Path.Combine(directory, dbfile.Source));
                        }
                        
                        db.Files.Remove(dbfile);
                    }
                }

                if (editedfilessource.Count>0)
                {
                    for (var i = 0; i < editedfilessource.Count; i++)
                    {
                        //if file that we edited is not in db, which means it's new file, add it to db
                        if (!allfilessource.Contains(editedfilessource [i]))
                        {
                            File file = new File()
                            {
                                BookID = bevm.BookViewModel.BookID,
                                Name = editedfilestext[i],
                                Source = editedfilessource[i]
                            };
                            db.Files.Add(file);
                            db.SaveChanges();
                        }
                        else
                        {
                            //find file text in db
                            var source = editedfilessource[i];
                            var filetext =
                            db.Files.FirstOrDefault(
                                f => f.BookID == bevm.BookViewModel.BookID && f.Source == source);

                            //if it is in db, check if text is still the same
                            //if it's not, change it
                            if (editedfilestext != null)
                            {
                                if (editedfilestext [i] != filetext.Name)
                                {
                                    filetext.Name = editedfilestext [i];
                                    db.SaveChanges();
                                }
                            }
                        }
                        //if its already in db and text still the same, nothing to do
                        db.SaveChanges();
                    }
                }
                

                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //Not valid
            IEnumerable<SelectListItem> writers = from w in db.Writers
                select new SelectListItem
                {
                    Value = w.WriterID.ToString(),
                    Text = w.Name + " " + w.Surname
                };
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Writers = new SelectList(writers, "Value", "Text");
            ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name");
            return View(bevm);
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