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
    public class AuditoriaController : Controller
    {
        private SumaEntities db = new SumaEntities();

        //
        // GET: /Auditoria/

        public ActionResult Index()
        {
            var auditorias = db.Auditorias.Include(a => a.ObjetoAuditoria).Include(a => a.OperacionAuditoria).Include(a => a.Usuario);
            return View(auditorias.ToList());
        }

        //
        // GET: /Auditoria/Details/5

        public ActionResult Details(short id = 0)
        {
            Auditoria auditoria = db.Auditorias.Find(id);
            if (auditoria == null)
            {
                return HttpNotFound();
            }
            return View(auditoria);
        }

        //
        // GET: /Auditoria/Create

        public ActionResult Create()
        {
            ViewBag.IdObjeto = new SelectList(db.ObjetoAuditorias, "Id", "Nombre");
            ViewBag.IdOperacion = new SelectList(db.OperacionAuditorias, "Id", "Nombre");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Login");
            return View();
        }

        //
        // POST: /Auditoria/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Auditoria auditoria)
        {
            if (ModelState.IsValid)
            {
                db.Auditorias.Add(auditoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdObjeto = new SelectList(db.ObjetoAuditorias, "Id", "Nombre", auditoria.IdObjeto);
            ViewBag.IdOperacion = new SelectList(db.OperacionAuditorias, "Id", "Nombre", auditoria.IdOperacion);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Login", auditoria.IdUsuario);
            return View(auditoria);
        }

        //
        // GET: /Auditoria/Edit/5

        public ActionResult Edit(short id = 0)
        {
            Auditoria auditoria = db.Auditorias.Find(id);
            if (auditoria == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdObjeto = new SelectList(db.ObjetoAuditorias, "Id", "Nombre", auditoria.IdObjeto);
            ViewBag.IdOperacion = new SelectList(db.OperacionAuditorias, "Id", "Nombre", auditoria.IdOperacion);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Login", auditoria.IdUsuario);
            return View(auditoria);
        }

        //
        // POST: /Auditoria/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Auditoria auditoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auditoria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdObjeto = new SelectList(db.ObjetoAuditorias, "Id", "Nombre", auditoria.IdObjeto);
            ViewBag.IdOperacion = new SelectList(db.OperacionAuditorias, "Id", "Nombre", auditoria.IdOperacion);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Login", auditoria.IdUsuario);
            return View(auditoria);
        }

        //
        // GET: /Auditoria/Delete/5

        public ActionResult Delete(short id = 0)
        {
            Auditoria auditoria = db.Auditorias.Find(id);
            if (auditoria == null)
            {
                return HttpNotFound();
            }
            return View(auditoria);
        }

        //
        // POST: /Auditoria/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Auditoria auditoria = db.Auditorias.Find(id);
            db.Auditorias.Remove(auditoria);
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