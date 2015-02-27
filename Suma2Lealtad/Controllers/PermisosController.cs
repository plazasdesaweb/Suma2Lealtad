using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SumaLealtad.Controllers
{
    public class PermisosController : Controller
    {
        private SumaEntities db = new SumaEntities();

        //
        // GET: /Permisos/

        public ActionResult Index()
        {
            var rolpermisoes = db.RolPermisoes.Include(r => r.Menu).Include(r => r.Rol);
            return View(rolpermisoes.ToList());
        }

        //
        // GET: /Permisos/Details/5

        public ActionResult Details(short id = 0)
        {
            RolPermiso rolpermiso = db.RolPermisoes.Find(id);
            if (rolpermiso == null)
            {
                return HttpNotFound();
            }
            return View(rolpermiso);
        }

        //
        // GET: /Permisos/Create

        public ActionResult Create()
        {
            ViewBag.IdMenu = new SelectList(db.Menus, "Id", "Texto");
            ViewBag.IdRol = new SelectList(db.Rols, "Id", "Nombre");
            return View();
        }

        //
        // POST: /Permisos/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RolPermiso rolpermiso)
        {
            if (ModelState.IsValid)
            {
                db.RolPermisoes.Add(rolpermiso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdMenu = new SelectList(db.Menus, "Id", "Texto", rolpermiso.IdMenu);
            ViewBag.IdRol = new SelectList(db.Rols, "Id", "Nombre", rolpermiso.IdRol);
            return View(rolpermiso);
        }

        //
        // GET: /Permisos/Edit/5

        public ActionResult Edit(short id = 0)
        {
            RolPermiso rolpermiso = db.RolPermisoes.Find(id);
            if (rolpermiso == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdMenu = new SelectList(db.Menus, "Id", "Texto", rolpermiso.IdMenu);
            ViewBag.IdRol = new SelectList(db.Rols, "Id", "Nombre", rolpermiso.IdRol);
            return View(rolpermiso);
        }

        //
        // POST: /Permisos/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RolPermiso rolpermiso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolpermiso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdMenu = new SelectList(db.Menus, "Id", "Texto", rolpermiso.IdMenu);
            ViewBag.IdRol = new SelectList(db.Rols, "Id", "Nombre", rolpermiso.IdRol);
            return View(rolpermiso);
        }

        //
        // GET: /Permisos/Delete/5

        public ActionResult Delete(short id = 0)
        {
            RolPermiso rolpermiso = db.RolPermisoes.Find(id);
            if (rolpermiso == null)
            {
                return HttpNotFound();
            }
            return View(rolpermiso);
        }

        //
        // POST: /Permisos/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            RolPermiso rolpermiso = db.RolPermisoes.Find(id);
            db.RolPermisoes.Remove(rolpermiso);
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