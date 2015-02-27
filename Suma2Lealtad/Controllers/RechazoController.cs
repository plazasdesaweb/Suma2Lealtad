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
    public class RechazoController : Controller
    {
        private SumaEntities db = new SumaEntities();

        //
        // GET: /Rechazo/

        public ActionResult Index()
        {
            return View(db.RazonRechazos.ToList());
        }

        //
        // GET: /Rechazo/Details/5

        public ActionResult Details(byte id = 0)
        {
            RazonRechazo razonrechazo = db.RazonRechazos.Find(id);
            if (razonrechazo == null)
            {
                return HttpNotFound();
            }
            return View(razonrechazo);
        }

        //
        // GET: /Rechazo/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Rechazo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RazonRechazo razonrechazo)
        {
            if (ModelState.IsValid)
            {
                db.RazonRechazos.Add(razonrechazo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(razonrechazo);
        }

        //
        // GET: /Rechazo/Edit/5

        public ActionResult Edit(byte id = 0)
        {
            RazonRechazo razonrechazo = db.RazonRechazos.Find(id);
            if (razonrechazo == null)
            {
                return HttpNotFound();
            }
            return View(razonrechazo);
        }

        //
        // POST: /Rechazo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RazonRechazo razonrechazo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(razonrechazo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(razonrechazo);
        }

        //
        // GET: /Rechazo/Delete/5

        public ActionResult Delete(byte id = 0)
        {
            RazonRechazo razonrechazo = db.RazonRechazos.Find(id);
            if (razonrechazo == null)
            {
                return HttpNotFound();
            }
            return View(razonrechazo);
        }

        //
        // POST: /Rechazo/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            RazonRechazo razonrechazo = db.RazonRechazos.Find(id);
            db.RazonRechazos.Remove(razonrechazo);
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