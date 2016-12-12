using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Library.Models;
using LibraryMVC.Models;
using Microsoft.AspNet.Identity;
using File = Library.Models.File;

namespace LibraryMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        
        private void SetViewBag()
        {
            var listoptions = new List<string>() {"or", "and", "not"};
            
            ViewBag.Options = new SelectList(listoptions);
            ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name");
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "Name");
            IEnumerable<SelectListItem> writers = from w in db.Writers
                                                  select new SelectListItem
                                                  {
                                                      Value = w.WriterID.ToString(),
                                                      Text = w.Name + " " + w.Surname
                                                  };
            ViewBag.Writers = new SelectList(writers, "Value", "Text");
        }
        // GET: Books
        public ActionResult Index()
        {
            var svm = new SearchViewModel();
            var books = db.Books.ToList();
            svm.Books = books;
            SetViewBag();
            return View(svm);
        }
       
        public IEnumerable<Book> SearchTitle(string title, bool not)
        {
            IEnumerable<Book> books = db.Books.Where(b => b.Title.Contains(title)).ToList();
            if (not)
            {
                books = db.Books.ToList().Except(books).ToList();
            }
            return books;
        }

        public IEnumerable<Book> SearchCategory(int category, bool not)
        {
            IEnumerable<Book> books = db.Books.Where(b => b.CategoryID == category).ToList();
            if (not)
            {
                books = db.Books.ToList().Except(books).ToList();
            }
            return books;
        }

        public IEnumerable<Book> SearchWriters(int[] writers, string option)
        {
            IEnumerable<Book> books = new List<Book>();
            List<List<Book>> listofbooks = new List<List<Book>>();
            if (option == "or")
            {
                ICollection<Book> orBooks = new List<Book>();
                foreach (var searchwriter in writers)
                {
                    int writeritem = db.Writers.FirstOrDefault(w => w.WriterID == searchwriter).WriterID;
                    var correctbooks = db.Books.Where(b => b.Writers.Any(bw => bw.WriterID == writeritem));
                    foreach (var correctbook in correctbooks)
                    {
                        if (!books.Contains(correctbook))
                        {
                            orBooks.Add(correctbook);
                        }
                    }
                }
                books = orBooks;
            }
            else if (option == "and")
            {
                foreach (var searchwriter in writers)
                {
                    List<Book> localbooks = new List<Book>();
                    int writeritem = db.Writers.FirstOrDefault(w => w.WriterID == searchwriter).WriterID;
                    var correctbooks = db.Books.Where(b => b.Writers.Any(bw => bw.WriterID == writeritem));
                    foreach (var correctbook in correctbooks)
                    {
                        if (!books.Contains(correctbook))
                        {
                            localbooks.Add(correctbook);
                        }
                    }
                    listofbooks.Add(localbooks);
                }
                books = listofbooks [0];
                for (int i = 1; i < listofbooks.Count; i++)
                {
                        books = books.Intersect(listofbooks [i]).ToList();
                }

            }
            else //not
            {
                ICollection<Book> notBooks = new List<Book>();
                foreach (var searchwriter in writers)
                {
                    int writeritem = db.Writers.FirstOrDefault(w => w.WriterID == searchwriter).WriterID;
                    var correctbooks = db.Books.Where(b => b.Writers.Any(bw => bw.WriterID == writeritem));
                    foreach (var correctbook in correctbooks)
                    {
                        if (!books.Contains(correctbook))
                        {
                            notBooks.Add(correctbook);
                        }
                    }
                }
                books = db.Books.ToList().Except(notBooks).ToList();
            }
            return books;
        }

        public IEnumerable<Book> SearchLabels(int [] labels, string option)
        {
            IEnumerable<Book> books = new List<Book>();
            List<List<Book>> listofbooks = new List<List<Book>>();
            if (option == "or")
            {
                ICollection<Book> orBooks = new List<Book>();
                foreach (var searchlabel in labels)
                {
                    int labelitem = db.Labels.FirstOrDefault(l => l.LabelID == searchlabel).LabelID;
                    var correctbooks = db.Books.Where(b => b.Labels.Any(bw => bw.Label.LabelID == labelitem));
                    foreach (var correctbook in correctbooks)
                    {
                        if (!books.Contains(correctbook))
                        {
                            orBooks.Add(correctbook);
                        }
                    }
                }
                books = orBooks;
            }
            else if (option == "and")
            {
                foreach (var searchlabel in labels)
                {
                    List<Book> localbooks = new List<Book>();
                    int labelitem = db.Labels.FirstOrDefault(l => l.LabelID == searchlabel).LabelID;
                    var correctbooks = db.Books.Where(b => b.Labels.Any(bw => bw.Label.LabelID == labelitem));
                    foreach (var correctbook in correctbooks)
                    {
                        if (!localbooks.Contains(correctbook))
                        {
                            localbooks.Add(correctbook);
                        }
                    }
                    listofbooks.Add(localbooks);
                }
                books = listofbooks [0];
                for (int i = 1; i < listofbooks.Count; i++)
                {
                        books = books.Intersect(listofbooks [i]).ToList();
                }
            }
            else //not
            {
                ICollection<Book> notBooks = new List<Book>();
                foreach (var searchlabel in labels)
                {
                    int labelitem = db.Labels.FirstOrDefault(l => l.LabelID == searchlabel).LabelID;
                    var correctbooks = db.Books.Where(b => b.Labels.Any(bw => bw.Label.LabelID == labelitem));
                    foreach (var correctbook in correctbooks)
                    {
                        if (!books.Contains(correctbook))
                        {
                            notBooks.Add(correctbook);
                        }
                    }
                }
                books = db.Books.ToList().Except(notBooks).ToList();
            }

            return books;
        }

        public IEnumerable<Book> SearchIsbn(string isbn, bool not)
        {
            IEnumerable<Book> books = db.Books.Where(b => b.ISBN == isbn).ToList();
            if (not)
            {
                books = db.Books.ToList().Except(books).ToList();
            }
            return books;
        }

        public IEnumerable<Book> SearchYear(int year, bool not)
        {
            IEnumerable<Book> books = db.Books.Where(b => b.Year == year).ToList();
            if (not)
            {
                books = db.Books.ToList().Except(books).ToList();
            }
            var svm = new SearchViewModel();
            return books;
        }

        public List<IEnumerable<Book>> SearchAll(SearchViewModel svm)
        {
            IEnumerable<Book> titleBooks=null;
            IEnumerable<Book> categoryBooks=null;
            IEnumerable<Book> writersBooks=null;
            IEnumerable<Book> labelsBooks=null;
            IEnumerable<Book> yearBooks=null;
            IEnumerable<Book> isbnBooks=null;
            if (!string.IsNullOrEmpty(svm.SelectedTitle))
            {
                titleBooks = SearchTitle(svm.SelectedTitle, svm.TitleOption);
            }
            if (svm.SelectedCategory != 0)
            {
                categoryBooks = SearchCategory(svm.SelectedCategory, svm.CategoryOption);
            }
            if (svm.SelectedLabels != null)
            {
                labelsBooks = SearchLabels(svm.SelectedLabels, svm.LabelsOption);
            }
            if (svm.SelectedWriters != null)
            {
                writersBooks = SearchWriters(svm.SelectedWriters, svm.WritersOption);
            }
            if (svm.SelectedYear != 0)
            {
                yearBooks = SearchYear(svm.SelectedYear, svm.YearOption);
            }
            if (!string.IsNullOrEmpty(svm.SelectedISBN))
            {
                isbnBooks = SearchIsbn(svm.SelectedISBN, svm.ISBNOption);
            }
            return new List<IEnumerable<Book>>() {titleBooks,categoryBooks,writersBooks,labelsBooks,yearBooks,isbnBooks};
            
        }

        private IEnumerable<Book> AllOr(List<IEnumerable<Book>> listofbooks)
        {
            IEnumerable<Book> books = null;
            for (int i = 0; i < listofbooks.Count; i++)
            {
                if (listofbooks[i]!=null)
                {
                    books = listofbooks[i];
                    break;
                }
            }
            if (books!=null)
            {
                for (int i = 1; i < listofbooks.Count; i++)
                {
                    if (listofbooks[i]!=null)
                    {
                        books = books.Union(listofbooks [i]).ToList();
                    }
                }
            }
            
            return books;
        }
        private IEnumerable<Book> AllAnd(List<IEnumerable<Book>> listofbooks)
        {
            IEnumerable<Book> books=null;
            foreach (var listofbook in listofbooks)
            {
                if (listofbook!=null)
                {
                    books= listofbook;
                    break;
                }
            }
            if (books!=null)
            {
                foreach (IEnumerable<Book> listofbook in listofbooks)
                {
                    if (listofbook != null)
                    {
                        books = books.Intersect(listofbook).ToList();
                    }
                }
            }
            return books;
        }
        private IEnumerable<Book> AllNot(List<IEnumerable<Book>> listofbooks)
        {
            IEnumerable<Book> books = db.Books.ToList();
            for (int i = 0; i < listofbooks.Count; i++)
            {
                if (listofbooks[i]!=null)
                {
                    books = books.Except(listofbooks [i]).ToList();
                }
            }
            return books;
        }
        //Post: Books
        [HttpPost]
        public ActionResult Index(SearchViewModel svm)
        {
            SetViewBag();
            if (Request.Form ["Title"]!=null)
            {
                svm.Books = SearchTitle(svm.SelectedTitle, svm.TitleOption);
                return View("Index", svm);
            }
            if (Request.Form ["Category"] != null)
            {
                svm.Books = SearchCategory(svm.SelectedCategory, svm.CategoryOption);
                return View("Index", svm);
            }
            if (Request.Form ["Writers"] != null)
            {
                svm.Books = SearchWriters(svm.SelectedWriters, svm.WritersOption);
                return View("Index", svm);
            }
            if (Request.Form ["Labels"] != null)
            {
                svm.Books = SearchLabels(svm.SelectedLabels, svm.LabelsOption);
                return View("Index", svm);
            }
            if (Request.Form ["Year"] != null)
            {
                svm.Books = SearchYear(svm.SelectedYear,svm.YearOption);
                return View("Index", svm);
            }
            if (Request.Form ["ISBN"] != null)
            {
                svm.Books =  SearchIsbn(svm.SelectedISBN, svm.ISBNOption);
                return View("Index", svm);
            }
            if(Request.Form ["All"] != null)
            {
                var listofbooks = SearchAll(svm);
                switch (svm.AllOption)
                {
                    case "or":
                        svm.Books = AllOr(listofbooks); 
                        break;
                    case "and":
                        svm.Books = AllAnd(listofbooks);
                        break;
                    default:
                    case "not":
                        svm.Books = AllNot(listofbooks);
                        break;
                }
                return View("Index", svm);
            }
            if (Request.Form ["Save"] != null)
            {
                Save(svm);
                svm.Books = db.Books.ToList();
            }
            return View(svm);
        }
        [Authorize]
        public void Save(SearchViewModel svm)
        {
            //json
            var serializer = new JavaScriptSerializer();
            var serializerobject = serializer.Serialize(svm);
            var sh = new SearchHistory();
            sh.Name = svm.SaveName;
            sh.ReaderID = User.Identity.GetUserId();
            sh.URL = serializerobject;
            db.SearchHistories.Add(sh);
            db.SaveChanges();
        }
        [Authorize]
        public ActionResult Load(int id)
        {
            var searchHistory = db.SearchHistories.FirstOrDefault(s => s.SearchHistoryID == id);
            var serializer = new JavaScriptSerializer();
            var svm = serializer.Deserialize<SearchViewModel>(searchHistory.URL);
            svm.Books = db.Books.ToList();

            var listofbooks = SearchAll(svm);
            switch (svm.AllOption)
            {
                case "or":
                    svm.Books = AllOr(listofbooks);
                    break;
                case "and":
                    svm.Books = AllAnd(listofbooks);
                    break;
                default:
                    svm.Books = AllNot(listofbooks);
                    break;
            }
            SetViewBag();
            return View("Index", svm);
        }
        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var book = db.Books.Find(id);
            var directory = Path.Combine(Server.MapPath("~/App_Data/uploads"), book.BookID.ToString());
            ViewBag.Content = System.IO.File.ReadAllText(Path.Combine(directory,book.Contents), Encoding.Default);
            if (book == null)
                return HttpNotFound();
            return View(book);
        }
        [Authorize(Roles = "Worker")]
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
        [Authorize(Roles = "Worker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookViewModel bvm)
        {
            
            if (ModelState.IsValid)
            {
                var extension = Path.GetExtension(bvm.Contents.FileName);
                
                Book book = new Book()
                {
                    AddDate = DateTime.Now,
                    Amount = bvm.Amount,
                    CategoryID = bvm.CategoryID,
                    Year = bvm.Year,
                    Title = bvm.Title,
                    ISBN = bvm.ISBN,
                    Description = bvm.Description,
                    Writers = new List<BookWriter>(),
                    Labels = new List<BookLabel>()
                    
                };
                db.Books.Add(book);
                db.SaveChanges();
                var filename = book.BookID + "_contents" + extension;
                book.Contents = filename;
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
        [Authorize(Roles = "Worker")]
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
        [Authorize(Roles = "Worker")]
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

        [Authorize(Roles = "Worker")]
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
        [Authorize(Roles = "Worker")]
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
        [Authorize]
        [ActionName("Download")]
        public void Download(string filename,string bookid)
        {
            var directory = Path.Combine(Path.Combine("/App_Data/uploads", bookid));
            var filepath = Path.Combine(directory, filename);
            System.Web.HttpContext.Current.Response.ContentType = "APPLICATION/OCTET-STREAM";
            var header = "Attachment; Filename=" + filename;
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", header);
            var dfile = new FileInfo(System.Web.HttpContext.Current.Server.MapPath(filepath));
            System.Web.HttpContext.Current.Response.WriteFile(dfile.FullName);
            System.Web.HttpContext.Current.Response.End();
        }
        [Authorize]
        [ActionName("AddToBasket")]
        public ActionResult AddToBasket(string bookid)
        {
            int id = Int32.Parse(bookid);
            var book = db.Books.FirstOrDefault(b => b.BookID == id);
            Basket basket;
            if (Session["basket"]==null)
            {
                basket= new Basket();
                Session["basket"] = basket;
            }
            else
            {
                basket = (Basket)Session["basket"];
            }
            bool contains = basket.Books.Any(b => b.BookID == book.BookID);
            if (!contains)
            {
                basket.Books.Add(book);
            }
            return RedirectToAction("Index", "Basket");
        }
    }
}