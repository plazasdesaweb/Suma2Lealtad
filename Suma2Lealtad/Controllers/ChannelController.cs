﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Suma2Lealtad.Models;

namespace Suma2Lealtad.Controllers
{
    public class ChannelController : Controller
    {
        private LealtadEntities db = new LealtadEntities();

        //
        // GET: /Channel/

        public ActionResult Index()
        {
            return View(db.Channels.ToList());
        }

        //
        // GET: /Channel/Details/5

        public ActionResult Details(int id = 0)
        {
            Channel channel = db.Channels.Find(id);
            if (channel == null)
            {
                return HttpNotFound();
            }
            return View(channel);
        }

        //
        // GET: /Channel/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Channel/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Channel channel)
        {
            if (ModelState.IsValid)
            {
                channel.id = db.Channels.Max(c => c.id) + 1;
                db.Channels.Add(channel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(channel);
        }

        //
        // GET: /Channel/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Channel channel = db.Channels.Find(id);
            if (channel == null)
            {
                return HttpNotFound();
            }
            return View(channel);
        }

        //
        // POST: /Channel/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Channel channel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(channel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(channel);
        }

        //
        // GET: /Channel/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Channel channel = db.Channels.Find(id);
            if (channel == null)
            {
                return HttpNotFound();
            }
            return View(channel);
        }

        //
        // POST: /Channel/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Channel channel = db.Channels.Find(id);
            db.Channels.Remove(channel);
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