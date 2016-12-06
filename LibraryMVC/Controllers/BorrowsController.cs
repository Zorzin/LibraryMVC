using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Library.Models;
using LibraryMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LibraryMVC.Controllers
{
    [Authorize]
    public class BorrowsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult UserIndex(string userid)
        {
            var user = IdentityManager.GetUserById(userid);
            var borrows = db.Borrows.Where(b=>b.ReaderID == user.Id);
            return View(borrows.ToList());
        }


        // GET: Borrows
        public ActionResult Index()
        {
            var borrows = db.Borrows.Include(b => b.Book).Include(b => b.Reader);
            return View(borrows.ToList());
        }

        // GET: Borrows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }

        // GET: Borrows/Create
        [Authorize(Roles = "Worker")]
        public ActionResult Create()
        {
            ViewBag.BookID = new SelectList(db.Books, "BookID", "Title");
            ViewBag.ReaderID = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Borrows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Worker")]
        public ActionResult Create([Bind(Include = "BorrowID,BookID,ReaderID,BorrowDate,ReturnDate,Deadline,Status")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                db.Borrows.Add(borrow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookID = new SelectList(db.Books, "BookID", "Title", borrow.BookID);
            ViewBag.ReaderID = new SelectList(db.Users, "Id", "Name", borrow.ReaderID);
            return View(borrow);
        }

        // GET: Borrows/Edit/5
        [Authorize(Roles = "Worker")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookID = new SelectList(db.Books, "BookID", "Title", borrow.BookID);
            ViewBag.ReaderID = new SelectList(db.Users, "Id", "Name", borrow.ReaderID);
            ViewBag.StatusList = new SelectList(BorrowLogic.BorrowStatus);
            
            return View(borrow);
        }

        // POST: Borrows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult> Edit([Bind(Include = "BorrowID,BookID,ReaderID,BorrowDate,ReturnDate,Deadline,Status")] Borrow borrow)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(borrow).State = EntityState.Modified;
                db.SaveChanges();
                if (ViewBag.previuosstatus==null)
                {
                    await SendMail(borrow.ReaderID, borrow.Status);
                }
                else if (ViewBag.previuosstatus != borrow.Status)
                {
                    await SendMail(borrow.ReaderID, borrow.Status);
                }
                return RedirectToAction("Index");
            }
            ViewBag.BookID = new SelectList(db.Books, "BookID", "Title", borrow.BookID);
            ViewBag.ReaderID = new SelectList(db.Users, "Id", "Name", borrow.ReaderID);
            ViewBag.StatusList = new SelectList(BorrowLogic.BorrowStatus);
            ViewBag.previuosstatus = borrow.Status;
            return View(borrow);
        }

        private async Task SendMail(string userid, string status)
        {
            UserManager<User> userManager =
                        HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            await userManager.SendEmailAsync(userid, "Status of your borrow changed", "Your borrow have now status: " + status + ". \nHave a nice day, LibraryMVC team.");
        }
        // GET: Borrows/Delete/5
        [Authorize(Roles = "Worker")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }

        // POST: Borrows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Worker")]
        public ActionResult DeleteConfirmed(int id)
        {
            Borrow borrow = db.Borrows.Find(id);
            db.Borrows.Remove(borrow);
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
