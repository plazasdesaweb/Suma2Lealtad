using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers.Prepago
{
    public class BeneficiarioPrepagoController : Controller
    {
        public ActionResult Filter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Filter(int id)
        {
            return View();
        }

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

    }
}
