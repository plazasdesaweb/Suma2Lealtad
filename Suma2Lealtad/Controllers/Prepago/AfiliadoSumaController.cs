﻿using Suma2Lealtad.Filters;
using Suma2Lealtad.Models;
using Suma2Lealtad.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers.Prepago
{
    [AuditingFilter]
    [HandleError]
    public class AfiliadoSumaController : Controller
    {
        private const int ID_TYPE_SUMA = 1;
        private AfiliadoSumaRepository repAfiliado = new AfiliadoSumaRepository();

        public ActionResult Filter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Filter(string numdoc)
        {
            AfiliadoSuma afiliado = repAfiliado.Find(numdoc);
            if (afiliado.id == 0)
            {
                //NO ESTA EN SUMA
                afiliado.typeid = ID_TYPE_SUMA;
                afiliado.type = "Suma";
                //CARGO VALOR POR DEFECTO EN LISTA DE ESTADOS
                afiliado.ListaEstados.Insert(0, new ESTADO { COD_ESTADO = " ", DESCRIPC_ESTADO = "Seleccione un Estado" });
                return View("Create", afiliado);
            }
            else
            {
                //ESTA EN SUMA
                afiliado = repAfiliado.Find(afiliado.id);
                return View("Edit", afiliado);
            }
        }

        [HttpPost]
        public ActionResult Create(AfiliadoSuma afiliado, HttpPostedFileBase file)
        {
            ViewModel viewmodel = new ViewModel();
            if (repAfiliado.Save(afiliado, file))
            {
                viewmodel.Title = "Afiliado / Crear Afiliación";
                viewmodel.Message = "Solicitud de afiliación creada exitosamente.";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            else
            {
                viewmodel.Title = "Afiliado / Crear Afiliación";
                viewmodel.Message = "Error de aplicacion: No se pudo crear solicitud de afiliación.";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult FilterReview()
        {
            AfiliadoSuma afiliado = new AfiliadoSuma();
            return View(afiliado);
        }

        [HttpPost]
        public ActionResult FilterReview(string numdoc, string name, string email, string estadoAfiliacion, string estadoTarjeta)
        {
            List<AfiliadoSumaIndex> afiliados = repAfiliado.Find(numdoc, name, email, estadoAfiliacion, estadoTarjeta);
            return View("Index",afiliados);                        
        }

        public ActionResult Edit(int id)
        {
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            return View(afiliado);
        }

        [HttpPost]
        public ActionResult Edit(AfiliadoSuma afiliado, HttpPostedFileBase fileNoValidado)
        {
            ViewModel viewmodel = new ViewModel();
            if (!repAfiliado.SaveChanges(afiliado, fileNoValidado))
            {
                viewmodel.Title = "Afiliado / Revisar Afiliación";
                viewmodel.Message = "Error de aplicacion: No se pudo actualizar afiliación.";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            else
            {
                viewmodel.Title = "Afiliado / Revisar Afiliación";
                viewmodel.Message = "La información del afiliado ha sido actualizada satisfactoriamente.";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public FileContentResult GetImage(int id)
        {
            Photos_Affiliate Photo = repAfiliado.Find(id).picture;
            if (Photo != null)
            {
                return File(Photo.photo, Photo.photo_type);
            }
            else
            {
                return null;
            }
        }

        public ActionResult Aprobar(int id)
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            if (repAfiliado.Aprobar(afiliado))
            {
                viewmodel.Title = "Afiliado / Aprobar Afiliación:";
                viewmodel.Message = "Afiliación Aprobada.";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            else
            {
                viewmodel.Title = "Afiliado / Aprobar Afiliación:";
                viewmodel.Message = "Error de aplicacion: No se pudo aprobar afiliación.";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult CrearPin(int id)
        {
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            return View("PinPadCrearPin", afiliado);
        }

        public ActionResult CambiarPin(int id)
        {
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            return View("PinPadCambiarPin", afiliado);
        }

        public ActionResult ReiniciarPin(int id)
        {
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            return View("PinPadReiniciarPin", afiliado);
        }

        public ActionResult ImprimirTarjeta(int id)
        {
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            afiliado.trackI = Tarjeta.ConstruirTrackI(afiliado.pan);
            afiliado.trackII = Tarjeta.ConstruirTrackII(afiliado.pan);            
            return View("ImpresoraImprimirTarjeta", afiliado);
        }

        public ActionResult ReImprimirTarjeta(int id)
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            if (repAfiliado.ImprimirTarjeta(afiliado))
            {
                if (repAfiliado.BloquearTarjeta(afiliado))
                {
                    afiliado.trackI = Tarjeta.ConstruirTrackI(afiliado.pan);
                    afiliado.trackII = Tarjeta.ConstruirTrackII(afiliado.pan);                        
                    return View("ImpresoraImprimirTarjeta", afiliado);
                }
                else
                {
                    viewmodel.Title = "Afiliado / ReImprimir Tarjeta";
                    viewmodel.Message = "Falló el proceso de bloqueo de la tarjeta previa";
                    viewmodel.ControllerName = "AfiliadoSuma";
                    viewmodel.ActionName = "FilterReview";
                    return RedirectToAction("GenericView", viewmodel);
                }
            }
            else
            {
                viewmodel.Title = "Afiliado / ReImprimir Tarjeta";
                viewmodel.Message = "Falló el proceso de generación de nueva tarjeta";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
                return RedirectToAction("GenericView", viewmodel);
            }
        }

        [HttpPost]
        public ActionResult ImprimirTarjeta(int id, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            //afiliado.trackI = Tarjeta.ConstruirTrackI(afiliado.pan);
            //afiliado.trackII = Tarjeta.ConstruirTrackII(afiliado.pan);
            if (repAfiliado.ImprimirTarjeta(afiliado))
            {
                viewmodel.Title = "Afiliado / Operaciones con la Impresora / Imprimir Tarjeta";
                viewmodel.Message = "Tarjeta impresa y activada correctamente";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            else
            {
                viewmodel.Title = "Afiliado / Operaciones con la Impresora / Imprimir Tarjeta";
                viewmodel.Message = "Falló el proceso de impresión y activación de la Tarjeta";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult BloquearTarjeta(int id)
        {
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            return View(afiliado);
        }

        [HttpPost]
        public ActionResult BloquearTarjeta(int id, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            if (repAfiliado.BloquearTarjeta(afiliado))
            {
                viewmodel.Title = "Afiliado / Bloquear Tarjeta";
                viewmodel.Message = "Tarjeta bloqueada correctamente";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            else
            {
                viewmodel.Title = "Afiliado / Bloquear Tarjeta";
                viewmodel.Message = "Falló el proceso de bloqueo de la Tarjeta";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult SuspenderTarjeta(int id)
        {
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            return View(afiliado);
        }

        [HttpPost]
        public ActionResult SuspenderTarjeta(int id, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            if (repAfiliado.SuspenderTarjeta(afiliado))
            {
                viewmodel.Title = "Afiliado / Suspender Tarjeta";
                viewmodel.Message = "Tarjeta suspendida correctamente";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            else
            {
                viewmodel.Title = "Afiliado / Suspender Tarjeta";
                viewmodel.Message = "Falló el proceso de suspensión de la Tarjeta";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult ReactivarTarjeta(int id)
        {
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            return View(afiliado);
        }

        [HttpPost]
        public ActionResult ReactivarTarjeta(int id, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            if (repAfiliado.ReactivarTarjeta(afiliado))
            {
                viewmodel.Title = "Afiliado / Reactivar Tarjeta";
                viewmodel.Message = "Tarjeta reactivada correctamente";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            else
            {
                viewmodel.Title = "Afiliado / Reactivar Tarjeta";
                viewmodel.Message = "Falló el proceso de reactivación de la Tarjeta";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult SaldosMovimientos(int id)
        {
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            SaldosMovimientos SaldosMovimientos = repAfiliado.FindSaldosMovimientos(afiliado);
            if (SaldosMovimientos != null)
            {

                AfiliadoSumaSaldosMovimientos AfiliadoSaldosMovimientos = new AfiliadoSumaSaldosMovimientos()
                {
                    denominacionPrepago = "Bs.",
                    denominacionSuma = "Más.",
                    Afiliado = afiliado,
                    SaldosMovimientos = SaldosMovimientos
                };
                return View(AfiliadoSaldosMovimientos);
            }
            else
            {
                //ERROR EN METODO FIND
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Afiliado / Saldos y Movimientos";
                viewmodel.Message = "Ha ocurrido un error de aplicación (FindSaldosMovimientos). Notifique al Administrador.";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
                return RedirectToAction("GenericView", viewmodel);
            }
        }

        public ActionResult Acreditar(int id)
        {
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            return View(afiliado);
        }

        [HttpPost]
        public ActionResult Acreditar(int id, string monto)
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            string respuesta = repAfiliado.Acreditar(afiliado, monto);
            if (respuesta != null)
            {
                viewmodel.Title = "Afiliado / Acreditar";
                viewmodel.Message = "Acreditación exitosa. Clave de aprobación: " + respuesta;
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            else
            {
                viewmodel.Title = "Afiliado / Acreditar ";
                viewmodel.Message = "Falló el proceso de acreditación";
                viewmodel.ControllerName = "AfiliadoSuma";
                viewmodel.ActionName = "FilterReview";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult GenericView(ViewModel viewmodel)
        {
            return View(viewmodel);
        }

        public JsonResult CiudadList(string id)
        {
            List<CIUDAD> ciudades = repAfiliado.GetCiudades(id);

            return Json(new SelectList(ciudades, "COD_CIUDAD", "DESCRIPC_CIUDAD"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult MunicipioList(string id)
        {
            List<MUNICIPIO> municipios = repAfiliado.GetMunicipios(id);

            return Json(new SelectList(municipios, "COD_MUNICIPIO", "DESCRIPC_MUNICIPIO"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ParroquiaList(string id)
        {
            List<PARROQUIA> parroquias = repAfiliado.GetParroquias(id);

            return Json(new SelectList(parroquias, "COD_PARROQUIA", "DESCRIPC_PARROQUIA"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UrbanizacionList(string id)
        {
            List<URBANIZACION> urb = repAfiliado.GetUrbanizaciones(id);

            return Json(new SelectList(urb, "COD_URBANIZACION", "DESCRIPC_URBANIZACION"), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
      
    }
}
