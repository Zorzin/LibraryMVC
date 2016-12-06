using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Models;
using LibraryMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LibraryMVC.Controllers
{
    [Authorize]
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
                    return View("Error");
                }
                //is user currently borrow this book
                if (!BorrowLogic.IsCurrentlyBorrow(book.BookID,userid))
                {
                    return View("Error");
                }
                //Adding borrows to table
                
                var borrow = new Borrow()
                {
                    BookID = book.BookID,
                    ReaderID = userid,
                    BorrowDate = DateTime.Today,
                    Deadline = DateTime.Today.AddDays(30),
                    ReturnDate = DateTime.Today.AddDays(-1),
                    Status = "Book in store"
                };
                db.Borrows.Add(borrow);
                db.SaveChanges();
            }
            
            Session["basket"] = null;
            return View();
        }



        public ActionResult DeleteFromBasket(string bookid)
        {
            var id = Int32.Parse(bookid);
            var basket = (Basket)Session ["basket"];
            var book = basket.Books.FirstOrDefault(b => b.BookID == id);
            basket.Books.Remove(book);
            return RedirectToAction("Index");
        }

        public int NumberOfItemsInBasket()
        {
            Basket basket;
            if (Session ["basket"] == null)
            {
                basket = new Basket();
                Session ["basket"] = basket;
            }
            else
            {
                basket = (Basket)Session ["basket"];
            }
            return basket.Books.Count();
        }
       
    }
}
