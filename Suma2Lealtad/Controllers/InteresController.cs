using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Suma2Lealtad.Models;

namespace Suma2Lealtad.Controllers
{
    public class InteresController : Controller
    {
        private SumaEntities db = new SumaEntities();

        //
        // GET: /Interes/

        public ActionResult Index()
        {
            return View(db.Intereses.ToList());
        }

        //
        // GET: /Interes/Details/5

        public ActionResult Details(short id = 0)
        {
            Interes interes = db.Intereses.Find(id);
            if (interes == null)
            {
                return HttpNotFound();
            }
            return View(interes);
        }

        //
        // GET: /Interes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Interes/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Interes interes)
        {
            if (ModelState.IsValid)
            {
                db.Intereses.Add(interes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(interes);
        }

        //
        // GET: /Interes/Edit/5

        public ActionResult Edit(short id = 0)
        {
            Interes interes = db.Intereses.Find(id);
            if (interes == null)
            {
                return HttpNotFound();
            }
            return View(interes);
        }

        //
        // POST: /Interes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Interes interes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(interes);
        }

        //
        // GET: /Interes/Delete/5

        public ActionResult Delete(short id = 0)
        {
            Interes interes = db.Intereses.Find(id);
            if (interes == null)
            {
                return HttpNotFound();
            }
            return View(interes);
        }

        //
        // POST: /Interes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Interes interes = db.Intereses.Find(id);
            db.Intereses.Remove(interes);
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