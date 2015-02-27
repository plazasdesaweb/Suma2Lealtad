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
    public class BloqueoController : Controller
    {
        private SumaEntities db = new SumaEntities();

        //
        // GET: /Bloqueo/

        public ActionResult Index()
        {
            return View(db.RazonBloqueos.ToList());
        }

        //
        // GET: /Bloqueo/Details/5

        public ActionResult Details(byte id = 0)
        {
            RazonBloqueo razonbloqueo = db.RazonBloqueos.Find(id);
            if (razonbloqueo == null)
            {
                return HttpNotFound();
            }
            return View(razonbloqueo);
        }

        //
        // GET: /Bloqueo/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Bloqueo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RazonBloqueo razonbloqueo)
        {
            if (ModelState.IsValid)
            {
                db.RazonBloqueos.Add(razonbloqueo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(razonbloqueo);
        }

        //
        // GET: /Bloqueo/Edit/5

        public ActionResult Edit(byte id = 0)
        {
            RazonBloqueo razonbloqueo = db.RazonBloqueos.Find(id);
            if (razonbloqueo == null)
            {
                return HttpNotFound();
            }
            return View(razonbloqueo);
        }

        //
        // POST: /Bloqueo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RazonBloqueo razonbloqueo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(razonbloqueo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(razonbloqueo);
        }

        //
        // GET: /Bloqueo/Delete/5

        public ActionResult Delete(byte id = 0)
        {
            RazonBloqueo razonbloqueo = db.RazonBloqueos.Find(id);
            if (razonbloqueo == null)
            {
                return HttpNotFound();
            }
            return View(razonbloqueo);
        }

        //
        // POST: /Bloqueo/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            RazonBloqueo razonbloqueo = db.RazonBloqueos.Find(id);
            db.RazonBloqueos.Remove(razonbloqueo);
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