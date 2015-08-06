using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers.Prepago
{
    public class OrdenRecargaPrepagoController : Controller
    {
        public ActionResult FilterReview()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FilterReview(int id)
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetalleOrden()
        {
            return View();
        }

        public ActionResult AprobarOrden()
        {
            return View();
        }

        public ActionResult ProcesarOrden()
        {
            return View();
        }

        public ActionResult RecargaIndividual()
        {
            return View();
        }

        public ActionResult RecargaMasiva()
        {
            return View();
        }

        public ActionResult ResultadoOrden()
        {
            return View();
        }
    }
}
