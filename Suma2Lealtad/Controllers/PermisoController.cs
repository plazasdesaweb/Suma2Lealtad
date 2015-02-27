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
    public class PermisoController : Controller
    {
        private SumaEntities db = new SumaEntities();

        //
        // GET: /Permiso/

        public ActionResult Index()
        {
            var rolpermisos = db.RolPermisos.Include(r => r.Menu).Include(r => r.Rol);
            return View(rolpermisos.ToList());
        }

        //
        // GET: /Permiso/Details/5

        public ActionResult Details(short id = 0)
        {
            RolPermiso rolpermiso = db.RolPermisos.Find(id);
            if (rolpermiso == null)
            {
                return HttpNotFound();
            }
            return View(rolpermiso);
        }

        //
        // GET: /Permiso/Create

        public ActionResult Create()
        {
            ViewBag.IdMenu = new SelectList(db.Menus, "Id", "Texto");
            ViewBag.IdRol = new SelectList(db.Roles, "Id", "Nombre");
            return View();
        }

        //
        // POST: /Permiso/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RolPermiso rolpermiso)
        {
            if (ModelState.IsValid)
            {
                db.RolPermisos.Add(rolpermiso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdMenu = new SelectList(db.Menus, "Id", "Texto", rolpermiso.IdMenu);
            ViewBag.IdRol = new SelectList(db.Roles, "Id", "Nombre", rolpermiso.IdRol);
            return View(rolpermiso);
        }

        //
        // GET: /Permiso/Edit/5

        public ActionResult Edit(short id = 0)
        {
            RolPermiso rolpermiso = db.RolPermisos.Find(id);
            if (rolpermiso == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdMenu = new SelectList(db.Menus, "Id", "Texto", rolpermiso.IdMenu);
            ViewBag.IdRol = new SelectList(db.Roles, "Id", "Nombre", rolpermiso.IdRol);
            return View(rolpermiso);
        }

        //
        // POST: /Permiso/Edit/5

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
            ViewBag.IdRol = new SelectList(db.Roles, "Id", "Nombre", rolpermiso.IdRol);
            return View(rolpermiso);
        }

        //
        // GET: /Permiso/Delete/5

        public ActionResult Delete(short id = 0)
        {
            RolPermiso rolpermiso = db.RolPermisos.Find(id);
            if (rolpermiso == null)
            {
                return HttpNotFound();
            }
            return View(rolpermiso);
        }

        //
        // POST: /Permiso/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            RolPermiso rolpermiso = db.RolPermisos.Find(id);
            db.RolPermisos.Remove(rolpermiso);
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