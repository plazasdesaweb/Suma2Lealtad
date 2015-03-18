using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Suma2Lealtad.Models;
using Suma2Lealtad.Modules;

namespace Suma2Lealtad.Controllers
{

    public class UserController : Controller
    {
        private LealtadEntities db = new LealtadEntities();

        //
        // GET: /User/

        public ActionResult Index()
        {
            var q = (from u in db.Users
	        select new Usuario()
	        {
                id = u.id,
                login = u.login,
                passw = u.passw,
                firstname = u.firstname,
                lastname = u.lastname,
                email = u.email,
                status = (u.status.Equals("1") ? "Activo":"Inactivo")
	        });

            return View(q.ToList());
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.passw = AppModule.EncryptStringAES(user.passw);
                user.status = "1"; //provisional
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //  
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            User user = db.Users.Find(id);

            foreach (var m in db.UserRols.Where(m => m.userid == id))
            {
                db.UserRols.Remove(m);
            }

            db.SaveChanges();

            db.Users.Remove(user);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}