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

        //private LealtadEntities db = new LealtadEntities();

        public ActionResult Filter()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Find(string numdoc)
        {

            AfiliadoRepository afiliado = new AfiliadoRepository();

            if ( afiliado.IsRecordPlazasWeb(numdoc) )
            {

                TempData["ModelAfiliado"] = afiliado.Model;

                return RedirectToAction("Create", "Afiliado");

            } else {

                /* PENDIENTE: Incluir lógica. */

                return RedirectToAction("Filter", "Afiliado");

            }

        }

        //
        // GET: /Afiliado/Create

        public ActionResult Create()
        {
            var model = TempData["ModelAfiliado"] as Afiliado;
            return View(model);        
        }

        //
        // POST: /Afiliado/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Afiliado AfiliadoSuma)
        {
            /* PENDIENTE : Implementar Expresiones Reguladores */
            //if (ModelState.IsValid)
            //{
                AfiliadoRepository repositorio = new AfiliadoRepository();

                repositorio.Save(AfiliadoSuma);

                return RedirectToAction("Filter");
            //}

            //return View(AfiliadoSuma);
        }

    }

}