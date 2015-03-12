using Suma2Lealtad.Models;
using Suma2Lealtad.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers
{

    public class HomeController : Controller
    {

        [HttpPost]
        public ActionResult Index(LoginModel Model)
        {

            AppSession app = new AppSession();

            if ( app.Login(Model.UserName, Model.Password) )
            {

                HttpContext.Session["username"] = app.UserName;
                Session["login"] = app.UserLogin;
                Session["username"] = app.UserName;
                Session["userid"] = app.UserID;
                Session["menu"] = app.MenuList;
                Session["appdate"] = app.AppDate;

                ViewBag.AppDate = app.AppDate;
                ViewBag.Menu = app.MenuList;

                return View();

            }

            return RedirectToAction("Login");

        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {

            HttpContext.Session.Clear();
            HttpContext.Session.Abandon();
            HttpContext.User = null;

            return RedirectToAction("Login");

        }

    }
}


#region trash

//using Suma2Lealtad.Models;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace Suma2Lealtad.Controllers
//{
//    public class HomeController : Controller
//    {

//        public ActionResult Index(string login, string clave)
//        {
//            return View();
//            //using (SumaEntities context = new SumaEntities())
//            //{

//            //    var result = context.Usuarios.Where(p => p.Login == login && p.Clave == clave).Any();

//            //    if ( result )
//            //    {
//            //        ViewBag.Message = "Bienvenido Daniel";
//            //        return View();
//            //    }
//            //    else
//            //    {
//            //        return RedirectToAction("Login","Home");
//            //    }

//            //}

//        }

//        public ActionResult Login()
//        {
//            ViewBag.Message = "";

//            return View();
//        }

//    }

//}

#endregion