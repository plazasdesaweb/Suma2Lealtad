﻿using System.Web.Mvc;

namespace Suma2Lealtad.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult Error404()
        {
            return View();
        }

        public ActionResult SessionExpired() 
        {
            return View();
        }

    }

}
