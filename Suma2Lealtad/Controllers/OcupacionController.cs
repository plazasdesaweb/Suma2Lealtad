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
    public class OcupacionController : Controller
    {
        private SumaEntities db = new SumaEntities();

        //
        // GET: /Ocupacion/

        public ActionResult Index()
        {
            return View(db.Ocupaciones.ToList());
        }

        //
        // GET: /Ocupacion/Details/5

        public ActionResult Details(byte id = 0)
        {
            Ocupacion ocupacion = db.Ocupaciones.Find(id);
            if (ocupacion == null)
            {
                return HttpNotFound();
            }
            return View(ocupacion);
        }

        //
        // GET: /Ocupacion/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Ocupacion/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ocupacion ocupacion)
        {
            if (ModelState.IsValid)
            {
                db.Ocupaciones.Add(ocupacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ocupacion);
        }

        //
        // GET: /Ocupacion/Edit/5

        public ActionResult Edit(byte id = 0)
        {
            Ocupacion ocupacion = db.Ocupaciones.Find(id);
            if (ocupacion == null)
            {
                return HttpNotFound();
            }
            return View(ocupacion);
        }

        //
        // POST: /Ocupacion/Edit/5

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
        // GET: /Ocupacion/Delete/5

        public ActionResult Delete(byte id = 0)
        {
            Ocupacion ocupacion = db.Ocupaciones.Find(id);
            if (ocupacion == null)
            {
                return HttpNotFound();
            }
            return View(ocupacion);
        }

        //
        // POST: /Ocupacion/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Ocupacion ocupacion = db.Ocupaciones.Find(id);
            db.Ocupaciones.Remove(ocupacion);
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