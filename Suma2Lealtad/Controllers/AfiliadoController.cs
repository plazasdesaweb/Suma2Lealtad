using Newtonsoft.Json;
using Suma2Lealtad.Filters;
using Suma2Lealtad.Models;
using Suma2Lealtad.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers
{
    [AuditingFilter]
    public class AfiliadoController : Controller
    {
        private AfiliadoRepository rep = new AfiliadoRepository();

        public ActionResult Filter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Find(string numdoc)
        {
            Afiliado afiliado = rep.Find(numdoc);
            if (afiliado.clientid == 0 && afiliado.id == 0)
            {
                //NOCLIENTE/NOAFILIADO
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Buscar Cliente:";
                viewmodel.Message = "El Cliente no esta registrado en WebPlazas. (Escenario //NOCLIENTE/NOAFILIADO)";
                viewmodel.ControllerName = "Afiliado";
                viewmodel.ActionName = "Filter";
                return RedirectToAction("GenericView", viewmodel);
            }
            else if (afiliado.clientid == 0 && afiliado.id != 0)
            {
                //NOCLIENTE/AFILIADO
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Buscar Cliente:";
                viewmodel.Message = "El Cliente no esta registrado en WebPlazas. (Escenario //NOCLIENTE/AFILIADO)";
                viewmodel.ControllerName = "Afiliado";
                viewmodel.ActionName = "Filter";
                return RedirectToAction("GenericView", viewmodel);
            }
            else if (afiliado.clientid != 0 && afiliado.id == 0)
            {
                //CLIENTE/NOAFILIADO
                return View("Create", afiliado);
            }
            else if (afiliado.clientid != 0 && afiliado.id != 0)
            {
                //CLIENTE/AFILIADO
                afiliado.name = (afiliado.name + " " + afiliado.lastname1).ToUpper();
                List<Afiliado> afiliados = new List<Afiliado> { afiliado };
                return View("Index", afiliados);
            }
            else
            {
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Buscar Cliente:";
                viewmodel.Message = "Ha ocurrido un error en la aplicación";
                viewmodel.ControllerName = "Afiliado";
                viewmodel.ActionName = "Filter";
                return RedirectToAction("GenericView", viewmodel);
            }
        }

        public ActionResult Create(int id)
        {
            Afiliado afiliado = rep.Find(id);
            return View(afiliado);
        }

        [HttpPost]
        public ActionResult Create(Afiliado afiliado, HttpPostedFileBase file)
        {
            ViewModel viewmodel = new ViewModel();
            if (rep.Save(afiliado))
            {
                if (file != null && file.ContentLength > 0)
                    try
                    {
                        string path = Server.MapPath(AppModule.GetPathPicture().Replace("@filename@", afiliado.docnumber));
                        file.SaveAs(path);
                    }
                    catch (Exception ex)
                    {
                        viewmodel.Title = "Crear Afiliación:";
                        viewmodel.Message = "Error de aplicacion: No se pudo cargar archivo (" + ex.Message + ")";
                        viewmodel.ControllerName = "Afiliado";
                        viewmodel.ActionName = "Filter";
                        //viewmodel.RouteValues = afiliado.docnumber;
                    }
                else
                {
                    //ViewBag.Message2 = "Debe seleccionar un archivo.";
                    //MessageBox.Show("Debe seleccionar un archivo");
                }
                //PENDIENTE: SI FALLA ALGUNA DE LAS ACTIVIDADES. HAY QUE DESHACER LAS ACTIVIDADES ANTERIORES EXITOSAS.                
                viewmodel.Title = "Crear Afiliación:";
                viewmodel.Message = "Solicitud de afiliación creada exitosamente.";
                viewmodel.ControllerName = "Afiliado";
                viewmodel.ActionName = "Index";
                viewmodel.RouteValues = afiliado.docnumber;
            }
            else
            {
                viewmodel.Title = "Crear Afiliación:";
                viewmodel.Message = "Error de aplicacion: No se pudo crear solicitud de afiliación.";
                viewmodel.ControllerName = "Afiliado";
                viewmodel.ActionName = "Filter";
                //viewmodel.RouteValues = afiliado.docnumber;
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult FilterReview()
        {
            return View();
        }

        //public ActionResult Index(string numdoc)
        //{
        //    List<Afiliado> afiliado = rep.FindSuma(numdoc, "", "");
        //    return View(afiliado);
        //}

        [HttpPost]
        public ActionResult Index(string numdoc, string name, string email)
        {
            List<Afiliado> afiliado = rep.Find(numdoc, name, email);
            return View(afiliado);
        }

        public ActionResult Aprobar(int id)
        {
            ViewModel viewmodel = new ViewModel();
            Afiliado afiliado = rep.Find(id);
            if (rep.Aprobar(afiliado))
            {
                viewmodel.Title = "Aprobar Afiliación:";
                viewmodel.Message = "Afiliación Aprobada.";
                viewmodel.ControllerName = "Afiliado";
                viewmodel.ActionName = "Index";
                viewmodel.RouteValues = afiliado.docnumber;
            }
            else
            {
                viewmodel.Title = "Aprobar Afiliación:";
                viewmodel.Message = "Aprobación Fallida.";
                viewmodel.ControllerName = "Afiliado";
                viewmodel.ActionName = "Index";
                viewmodel.RouteValues = afiliado.docnumber;
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        //public ActionResult Edit(int id = 0)
        public ActionResult Edit(int id)
        {
            Afiliado afiliado = rep.Find(id);
            return View(afiliado);
        }

        [HttpPost]
        public ActionResult Edit(Afiliado afiliado)
        {
            ViewModel viewmodel = new ViewModel();
            if (!rep.SaveChanges(afiliado))
            {
                //return RedirectToAction("GenericView", new { msg = "Existen campos que son requeridos para procesar el formulario." });
                //return RedirectToAction("FilterReview");
                viewmodel.Title = "Revisar Afiliación:";
                viewmodel.Message = "Existen campos que son requeridos para procesar el formulario.";
                viewmodel.ControllerName = "Afiliado";
                viewmodel.ActionName = "Index";
                viewmodel.RouteValues = afiliado.docnumber;
            }
            else
            {
                //Aqui debo llamar a los servicios de actualización
                //return RedirectToAction("GenericView", new { msg = "La información del afiliado ha sido actualizada satisfactoriamente." });
                //return RedirectToAction("FilterReview");
                //return View(afiliado);
                viewmodel.Title = "Revisar Afiliación:";
                viewmodel.Message = "La información del afiliado ha sido actualizada satisfactoriamente.";
                viewmodel.ControllerName = "Afiliado";
                viewmodel.ActionName = "Index";
                viewmodel.RouteValues = afiliado.docnumber;
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult ImprimirTarjeta(int id)
        {
            Afiliado afiliado = rep.Find(id);
            return View("ImpresoraImprimirTarjeta", afiliado);
        }

        public ActionResult CrearPin(int id)
        {
            Afiliado afiliado = rep.Find(id);
            return View("PinPadCrearPin", afiliado);
        }

        public ActionResult CambiarPin(int id)
        {
            Afiliado afiliado = rep.Find(id);
            return View("PinPadCambiarPin", afiliado);
        }

        public ActionResult ReiniciarPin(int id)
        {
            Afiliado afiliado = rep.Find(id);
            return View("PinPadReiniciarPin", afiliado);
        }

        public ActionResult SaldosMovimientos(int id)
        {
            SaldosMovimientos SaldosMovimientos = rep.FindSaldosMovimientos(id);
            return View(SaldosMovimientos);
        }

        public ActionResult Acreditar(int id)
        {
            Afiliado afiliado = rep.Find(id);
            return View(afiliado);
        }

        [HttpPost]
        public ActionResult Acreditar(string docnumber, string monto)
        {
            ViewModel viewmodel = new ViewModel();
            RespuestaCards RespuestaCards = rep.Acreditar(docnumber, monto);
            viewmodel.Title = "Acreditar";
            viewmodel.Message = "Código de respuesta (" + RespuestaCards.code + "): " + RespuestaCards.detail;
            viewmodel.ControllerName = "Afiliado";
            viewmodel.ActionName = "Index";
            viewmodel.RouteValues = docnumber;
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult BloquearTarjeta(int id)
        {
            ViewModel viewmodel = new ViewModel();
            viewmodel.Title = "";
            viewmodel.Message = "";
            viewmodel.ControllerName = "";
            viewmodel.ActionName = "";
            viewmodel.RouteValues = "";
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult SuspenderTarjeta(int id)
        {
            ViewModel viewmodel = new ViewModel();
            viewmodel.Title = "";
            viewmodel.Message = "";
            viewmodel.ControllerName = "";
            viewmodel.ActionName = "";
            viewmodel.RouteValues = "";
            return RedirectToAction("GenericView", viewmodel);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public ActionResult GenericView(ViewModel viewmodel)
        {
            return View(viewmodel);
        }

    }
}