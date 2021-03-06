﻿using Suma2Lealtad.Filters;
using Suma2Lealtad.Models;
using Suma2Lealtad.Modules;
using System.Web;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers
{

    public class HomeController : Controller
    {

        public ActionResult Login()
        {
            Session["titulo"] = "Administrador SUMA / PREPAGO";
            Session["login"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel Model)
        {
            AppSession app = new AppSession();
            if (app.Login(Model.UserName, Model.Password))
            {
                if (app.UserLogin == "prepago" || app.UserLogin == "jmartinezprepago")
                {
                    Session["titulo"] = "Administrador PREPAGO";
                }
                else
                {
                    Session["titulo"] = "Administrador SUMA";
                }
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

        [AuditingFilter]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Abandon();
            HttpContext.User = null;
            return RedirectToAction("Login");
        }

    }
}