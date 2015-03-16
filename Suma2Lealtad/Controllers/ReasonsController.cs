﻿using System;
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
    public class ReasonsController : Controller
    {
        private LealtadEntities db = new LealtadEntities();

        //
        // GET: /Reasons/

        public ActionResult Index()
        {
            return View(db.Reasons.Where(c => c.type == "Rechazo").ToList());
        }

        //
        // GET: /Reasons/Details/5

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
        // GET: /Reasons/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Reasons/Create

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
        // GET: /Reasons/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Reason reason = db.Reasons.Find(id);
            if (reason == null)
            {
                return HttpNotFound();
            }
            return View(reason);
        }

        //
        // POST: /Reasons/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reason reason)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reason).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reason);
        }

        //
        // GET: /Reasons/Delete/5

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
        // POST: /Reasons/Delete/5

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