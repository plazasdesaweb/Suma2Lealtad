using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SumaLealtad.Controllers
{
    public class NacionalidadesController : Controller
    {
        private SumaEntities db = new SumaEntities();

        //
        // GET: /Nacionalidades/

        public ActionResult Index()
        {
            return View(db.Nacionalidads.ToList());
        }

        //
        // GET: /Nacionalidades/Details/5

        public ActionResult Details(byte id = 0)
        {
            Nacionalidad nacionalidad = db.Nacionalidads.Find(id);
            if (nacionalidad == null)
            {
                return HttpNotFound();
            }
            return View(nacionalidad);
        }

        //
        // GET: /Nacionalidades/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Nacionalidades/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Nacionalidad nacionalidad)
        {
            if (ModelState.IsValid)
            {
                db.Nacionalidads.Add(nacionalidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nacionalidad);
        }

        //
        // GET: /Nacionalidades/Edit/5

        public ActionResult Edit(byte id = 0)
        {
            Nacionalidad nacionalidad = db.Nacionalidads.Find(id);
            if (nacionalidad == null)
            {
                return HttpNotFound();
            }
            return View(nacionalidad);
        }

        //
        // POST: /Nacionalidades/Edit/5

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
        // GET: /Nacionalidades/Delete/5

        public ActionResult Delete(byte id = 0)
        {
            Nacionalidad nacionalidad = db.Nacionalidads.Find(id);
            if (nacionalidad == null)
            {
                return HttpNotFound();
            }
            return View(nacionalidad);
        }

        //
        // POST: /Nacionalidades/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Nacionalidad nacionalidad = db.Nacionalidads.Find(id);
            db.Nacionalidads.Remove(nacionalidad);
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