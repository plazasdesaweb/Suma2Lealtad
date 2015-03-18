using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Suma2Lealtad.Models;
using Suma2Lealtad.Filters;

namespace Suma2Lealtad.Controllers
{
    [AuditingFilter]
    public class StatusController : Controller
    {
        private LealtadEntities db = new LealtadEntities();

        //
        // GET: /Status/

        public ActionResult Index()
        {
            return View(db.Status.ToList());
        }

        //
        // GET: /Status/Details/5

        public ActionResult Details(int id = 0)
        {
            Status status = db.Status.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        //
        // GET: /Status/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Status/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Status status)
        {
            if (ModelState.IsValid)
            {
                if (db.Status.Count() > 0)
                {
                    status.id = db.Status.Max(c => c.id) + 1;
                }
                else
                {
                    status.id = 1;
                }
                db.Status.Add(status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(status);
        }

        //
        // GET: /Status/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Status status = db.Status.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        //
        // POST: /Status/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Status status)
        {
            if (ModelState.IsValid)
            {
                db.Entry(status).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(status);
        }

        //
        // GET: /Status/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Status status = db.Status.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        //
        // POST: /Status/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Status status = db.Status.Find(id);
            db.Status.Remove(status);
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