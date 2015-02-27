using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SumaLealtad.Controllers
{
    public class OcupacionesController : Controller
    {
        private SumaEntities db = new SumaEntities();

        //
        // GET: /Ocupaciones/

        public ActionResult Index()
        {
            return View(db.Ocupacions.ToList());
        }

        //
        // GET: /Ocupaciones/Details/5

        public ActionResult Details(byte id = 0)
        {
            Ocupacion ocupacion = db.Ocupacions.Find(id);
            if (ocupacion == null)
            {
                return HttpNotFound();
            }
            return View(ocupacion);
        }

        //
        // GET: /Ocupaciones/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Ocupaciones/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ocupacion ocupacion)
        {
            if (ModelState.IsValid)
            {
                db.Ocupacions.Add(ocupacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ocupacion);
        }

        //
        // GET: /Ocupaciones/Edit/5

        public ActionResult Edit(byte id = 0)
        {
            Ocupacion ocupacion = db.Ocupacions.Find(id);
            if (ocupacion == null)
            {
                return HttpNotFound();
            }
            return View(ocupacion);
        }

        //
        // POST: /Ocupaciones/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ocupacion ocupacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ocupacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ocupacion);
        }

        //
        // GET: /Ocupaciones/Delete/5

        public ActionResult Delete(byte id = 0)
        {
            Ocupacion ocupacion = db.Ocupacions.Find(id);
            if (ocupacion == null)
            {
                return HttpNotFound();
            }
            return View(ocupacion);
        }

        //
        // POST: /Ocupaciones/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Ocupacion ocupacion = db.Ocupacions.Find(id);
            db.Ocupacions.Remove(ocupacion);
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