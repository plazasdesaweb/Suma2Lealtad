﻿using Suma2Lealtad.Filters;
using Suma2Lealtad.Models;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers
{
    [AuditingFilter]
    [HandleError]
    public class ReasonsBlockController : Controller
    {
        private LealtadEntities db = new LealtadEntities();

        //
        // GET: /ReasonsBlock/

        public ActionResult Index()
        {
            return View(db.Reasons.Where(c => c.type == "Bloqueo" && c.id > 1).ToList());
        }

        //
        // GET: /ReasonsBlock/Details/5

        public ActionResult Details(int id = 0)
        {
            Reason reason = db.Reasons.Find(id);
            if (reason == null)
            {
                return HttpNotFound();
            }
            return View(reason);
        }

        //
        // GET: /ReasonsBlock/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ReasonsBlock/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reason reason)
        {
            if (ModelState.IsValid)
            {
                db.Reasons.Add(reason);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reason);
        }

        //
        // GET: /ReasonsBlock/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Suma2Lealtad.Models.Reason reason = db.Reasons.Find(id);
            if (reason == null)
            {
                return HttpNotFound();
            }
            return View(reason);
        }

        //
        // POST: /ReasonsBlock/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suma2Lealtad.Models.Reason reason)
        {
            if (ModelState.IsValid)
            {
                reason.type = "Bloqueo";
                db.Entry(reason).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reason);
        }

        //
        // GET: /ReasonsBlock/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Reason reason = db.Reasons.Find(id);
            if (reason == null)
            {
                return HttpNotFound();
            }
            return View(reason);
        }

        //
        // POST: /ReasonsBlock/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reason reason = db.Reasons.Find(id);
            db.Reasons.Remove(reason);
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