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
    public class NacionalidadController : Controller
    {
        private SumaEntities db = new SumaEntities();

        //
        // GET: /Nacionalidad/

        public ActionResult Index()
        {
            return View(db.Nacionalidades.ToList());
        }

        //
        // GET: /Nacionalidad/Details/5

        public ActionResult Details(byte id = 0)
        {
            Nacionalidad nacionalidad = db.Nacionalidades.Find(id);
            if (nacionalidad == null)
            {
                return HttpNotFound();
            }
            return View(nacionalidad);
        }

        //
        // GET: /Nacionalidad/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Nacionalidad/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Nacionalidad nacionalidad)
        {
            if (ModelState.IsValid)
            {
                db.Nacionalidades.Add(nacionalidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nacionalidad);
        }

        //
        // GET: /Nacionalidad/Edit/5

        public ActionResult Edit(byte id = 0)
        {
            Nacionalidad nacionalidad = db.Nacionalidades.Find(id);
            if (nacionalidad == null)
            {
                return HttpNotFound();
            }
            return View(nacionalidad);
        }

        //
        // POST: /Nacionalidad/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Nacionalidad nacionalidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nacionalidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nacionalidad);
        }

        //
        // GET: /Nacionalidad/Delete/5

        public ActionResult Delete(byte id = 0)
        {
            Nacionalidad nacionalidad = db.Nacionalidades.Find(id);
            if (nacionalidad == null)
            {
                return HttpNotFound();
            }
            return View(nacionalidad);
        }

        //
        // POST: /Nacionalidad/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Nacionalidad nacionalidad = db.Nacionalidades.Find(id);
            db.Nacionalidades.Remove(nacionalidad);
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