using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryMVC.Models;

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


        public ActionResult DeleteFromBasket(string bookid)
        {
            var id = Int32.Parse(bookid);
            var basket = (Basket)Session ["basket"];
            var book = basket.Books.FirstOrDefault(b => b.BookID == id);
            basket.Books.Remove(book);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Index(Basket basket)
        {
            foreach (var book in basket.Books)
            {

                //Checking if possible to borrow
                
                //Adding borrows to table
                var name = User.Identity.Name;
            }

            return RedirectToAction("Index","Home");
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
