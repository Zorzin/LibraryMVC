using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryMVC.Models;
using Microsoft.AspNet.Identity;

namespace LibraryMVC.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Confirm(string id)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == id);
            return View(user);
        }
        [HttpPost]
        public ActionResult Confirm(User user)
        {
            var dbuser = db.Users.FirstOrDefault(x => x.Id == user.Id);
            if (Request.Form["delete"] != null)
            {
                db.Users.Remove(dbuser);
                db.SaveChanges();
            }
            else if(Request.Form ["confirm"] != null)
            {
                dbuser.EmailConfirmed = true;
                IdentityManager.AddUserToRoleById(User.Identity.GetUserId(),"User");
                db.SaveChanges();
            }
            return RedirectToAction("PendingRegistration");
        }

        public ActionResult AddToRole()
        {
            var users = db.Users.ToList();
            return View(users);
        }
        public ActionResult AddUserToRole(string id)
        {

            UserRoleViewModel urvm = new UserRoleViewModel();
            urvm.AllRoles = db.Roles.OrderBy(r => r.Name).Select(r=>r.Name).ToList();
            urvm.userid = id;
            urvm.CheckedRoles =  new bool[urvm.AllRoles.Count];
            for (int i = 0; i < urvm.AllRoles.Count; i++)
            {
                
                if (IdentityManager.IsUserInRoleById(id,urvm.AllRoles[i]))
                {
                    urvm.CheckedRoles[i] = true;
                }
                else
                {
                    urvm.CheckedRoles [i] = false;
                }
            }
            return View(urvm);
        }
        [HttpPost]
        public ActionResult AddUserToRole(UserRoleViewModel urvm)
        {
            urvm.AllRoles = db.Roles.OrderBy(r => r.Name).Select(r => r.Name).ToList();
            for (int i = 0; i < urvm.CheckedRoles.Length; i++)
            {
                if (urvm.CheckedRoles[i])
                {
                    IdentityManager.AddUserToRoleById(urvm.userid, urvm.AllRoles[i]);
                }
                else
                {
                    IdentityManager.DeleteUserFromRoleById(urvm.userid, urvm.AllRoles [i]);
                }
            }
            return RedirectToAction("AddToRole");
        }

        public ActionResult CreateRole()
        {
            var roles = db.Roles.OrderBy(r => r.Name).Select(r => r.Name).ToList();
            return View(roles);
        }

        public ActionResult CreateNewRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateNewRole(string name)
        {
            IdentityManager.CreateNewRoleByName(name);
            return RedirectToAction("CreateRole");
        }
        public ActionResult PendingRegistration()
        {
            ICollection<User> users = db.Users.Where(x => x.EmailConfirmed == false).ToList();
            return View(users);
        }

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
