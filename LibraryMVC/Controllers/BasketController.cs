using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Models;
using LibraryMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LibraryMVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        // GET: Basket
        public ActionResult Index()
        {
            Basket basket;
            if (Session["basket"]==null)
            {
                basket = new Basket();
                Session["basket"] = basket;
            }
            else
            {
                basket = (Basket) Session["basket"];
            }
            return View(basket);
        }
        
        public ActionResult Borrow()
        {
            var basket = (Basket) Session["basket"];
            foreach (var book in basket.Books)
            {
                var userid = IdentityManager.GetUserById(User.Identity.GetUserId()).Id;

                //Checking if possible to borrow
                //is there any book left to borrow
                if (!BorrowLogic.CanBookBeBorrow(book.BookID))
                {
                    continue;
                }
                //is user currently borrow this book
                if (!BorrowLogic.IsCurrentlyBorrow(book.BookID,userid))
                {
                    continue;
                }
                //Adding borrows to table
                
                var borrow = new Borrow()
                {
                    BookID = book.BookID,
                    ReaderID = userid,
                    BorrowDate = DateTime.Today,
                    Deadline = DateTime.Today.AddDays(30),
                    ReturnDate = DateTime.Today.AddDays(-1)
                };
                db.Borrows.Add(borrow);
                db.SaveChanges();
            }
            
            Session["basket"] = null;
            return RedirectToAction("Index", "Home");
        }



        public ActionResult DeleteFromBasket(string bookid)
        {
            var id = Int32.Parse(bookid);
            var basket = (Basket)Session ["basket"];
            var book = basket.Books.FirstOrDefault(b => b.BookID == id);
            basket.Books.Remove(book);
            return RedirectToAction("Index");
        }



        // GET: Basket/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Basket/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Basket/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Basket/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Basket/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Basket/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Basket/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
