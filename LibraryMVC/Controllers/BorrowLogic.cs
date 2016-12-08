using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryMVC.Models;

namespace LibraryMVC.Controllers
{
    public static class BorrowLogic
    {
        private static readonly ApplicationDbContext db = new ApplicationDbContext();
        public static ICollection<string> BorrowStatus = new List<string>() {"Book in store","Book ready to receive","Borrowed"};
        public static bool IsCurrentlyBorrow(int bookid, string userid)
        {
            var borrow = db.Borrows.Where(b => b.ReaderID == userid && b.BookID == bookid).ToList();
            if (borrow.Count > 0)
            {
                return false;
            }
            return true;
        }

        public static bool CanBookBeBorrow(int bookid)
        {
            var bookamount = db.Books.FirstOrDefault(b => b.BookID == bookid).Amount;
            var borrowbooks = db.Borrows.Where(b=>b.BookID == bookid && b.ReturnDate<b.BorrowDate).ToList();
            if (borrowbooks.Count < bookamount)
            {
                return true;
            }
            return false;
        }
    }
}