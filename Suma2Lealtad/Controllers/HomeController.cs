using Suma2Lealtad.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index(string login, string clave)
        {
            return View();
            //using (SumaEntities context = new SumaEntities())
            //{

            //    var result = context.Usuarios.Where(p => p.Login == login && p.Clave == clave).Any();

            //    if ( result )
            //    {
            //        ViewBag.Message = "Bienvenido Daniel";
            //        return View();
            //    }
            //    else
            //    {
            //        return RedirectToAction("Login","Home");
            //    }

            //}

        }

        public ActionResult Login()
        {
            ViewBag.Message = "";

            return View();
        }

    }

}