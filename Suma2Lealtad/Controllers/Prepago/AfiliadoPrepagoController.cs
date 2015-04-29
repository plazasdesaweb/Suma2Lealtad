using Suma2Lealtad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers.Prepago
{
    public class AfiliadoPrepagoController : Controller
    {
        private AfiliadoRepository rep = new AfiliadoRepository();

        public ActionResult Edit()
        {
            //obtengo el id del prepago y se lo paso al controlador afiliado
            return RedirectToAction("Edit", "Afiliado", new { id = 1 });
        }

    }
}
