﻿using Suma2Lealtad.Filters;
using Suma2Lealtad.Models;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers
{
    [AuditingFilter]
    [HandleError]
    public class CompanyController : Controller
    {
        private LealtadEntities db = new LealtadEntities();

        //
        // GET: /Company/

        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }

        //
        // GET: /Company/Details/5

        public ActionResult Details(int id = 0)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        //
        // GET: /Company/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Company/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                if (db.Companies.Count() > 0)
                {
                    company.id = db.Companies.Max(c => c.id) + 1;
                }
                else
                {
                    company.id = 1;
                }
                company.userid = 1; //provisional sesion
                company.creationdate = System.DateTime.Now;
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        //
        // GET: /Company/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        //
        // POST: /Company/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                company.userid = 1; //provisional sesion
                company.creationdate = System.DateTime.Now;
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        //
        // GET: /Company/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        //
        // POST: /Company/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
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