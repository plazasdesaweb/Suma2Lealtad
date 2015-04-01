using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Suma2Lealtad.Models;
using Suma2Lealtad.Modules;
using Newtonsoft.Json;
using Suma2Lealtad.Filters;

namespace Suma2Lealtad.Controllers
{
    [AuditingFilter]
    public class AfiliadoController : Controller
    {

        private AfiliadoRepository rep = new AfiliadoRepository();

        //
        // GET: /Afiliado/Filter

        public ActionResult Filter()
        {
            return View();
        }


        //
        // GET: /Afiliado/GenericView

        public ActionResult GenericView()
        {
            return View();
        }


        //
        // POST: /Afiliado/Find

        [HttpPost]
        public ActionResult Find(string numdoc)
        {

            Afiliado afiliado = rep.Find(numdoc);

            if (afiliado == null)
            {
                ViewBag.GenericView = "Registro No Encontrado.";
                return RedirectToAction("GenericView", "Afiliado");
            }

            TempData["AfiliadoModel"] = afiliado;
            return RedirectToAction("Create", "Afiliado");

        }


        //
        // GET: /Afiliado/Create

        public ActionResult Create()
        {
            var model = TempData["AfiliadoModel"] as Afiliado;
            return View(model);
        }


        //
        // POST: /Afiliado/Create

        [HttpPost]
        public ActionResult Create(Afiliado afiliado)
        {

            if (rep.Save(afiliado))
            {
                ViewBag.GenericView = "Registro creado satisfactoriamente.";
            }
            else
            {
                ViewBag.GenericView = "Ha ocurrido una excepción, revise los valores e intente nuevamente.";
            }

            return RedirectToAction("GenericView", "Afiliado");

        }


        //
        // GET : /Afiliado/FilterReview

        public ActionResult FilterReview()
        {
            return View();
        }


        //
        // GET : /Afiliado/Index

        [HttpPost]
        public ActionResult Index(string numdoc, string name, string email)
        {

            List<Afiliado> afiliado = rep.FindSuma(numdoc, name, email);

            return View(afiliado);

        }


        //
        // GET : /Afiliado/Edit/1

        public ActionResult Edit(int id = 0)
        {

            Afiliado afiliado = rep.FindSuma(id);

            if (afiliado == null)
            {
                //return HttpNotFound();
                return RedirectToAction("GenericView", "Afiliado");
            }
            return View(afiliado);

        }

        //
        // POST : /Afiliado/Edit/

        [HttpPost]
        public ActionResult Edit(Afiliado afiliado)
        {

            if (!rep.SaveChanges(afiliado))
            {
                return RedirectToAction("GenericView", "Afiliado");
                //return RedirectToAction("FilterReview");
            }
            return RedirectToAction("FilterReview");
            //return View( afiliado );

            //Aqui debo llamar a los servicios de actualización

        }

        public ActionResult OperacionesImpresora()
        {
            return View();
        }

        public ActionResult OperacionesPinPad()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }

}