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
            //Los estados actuales para una persona son:
            //NOCLIENTE            (no registrado en WEBPLAZAS)
            //NOAFILIADO           (no afiliado en SUMAPLAZAS)
            //CLIENTE              (registrado en WEBPLAZAS)
            //AFILIADO             (afiliado en SUMAPLAZAS)
            //El estado deseado es:
            //AFILIADO/CLIENTE     (registrado en WEBPLAZAS y afiliado en SUMAPLAZAS) 
            //Existen 4 resultados posibles para esta búsqueda
            //NOCLIENTE/NOAFILIADO -> por definir acción para crear registro de CLIENTE y crear afiliación de AFILIADO => Redireccionar a GenericView con mensaje descriptivo
            //NOCLIENTE/AFILIADO   -> por definir acción para crear registro de CLIENTE => Redireccionar a GenericView con mensaje descriptivo
            //CLIENTE/NOAFILIADO   -> acción: editar registro de CLIENTE y crear afiliación de AFILIADO => CREAR AFILIACION (retornar vista Create)
            //CLIENTE/AFILIADO     -> acción: editar registro de CLIENTE y editar afiliación de AFILIADO => REVISAR AFILIACION (Redirecciónar a acción Index ó Edit)

            Afiliado afiliado = rep.Find(numdoc);

            if (afiliado == null)
            {
                //pendiente
                
                // registro no encontrado en WebPlazas.
                return RedirectToAction("GenericView", new { msg = "El Número del Documento no existe en el Modelo WebPlazas." });
                //return RedirectToAction("GenericView");
            }
            else if (afiliado.clientid == 0 && afiliado != null)
            {
                //pendiente
                return RedirectToAction("GenericView");
            }
            else if (afiliado.clientid != 0 && afiliado.id == 0)
            {
                return View("Create", afiliado);
            }
            else if (afiliado.clientid != 0 && afiliado.id != 0)
            {
                afiliado.name = (afiliado.name + " " + afiliado.lastname1).ToUpper();
                List<Afiliado> afiliados = new List<Afiliado> { afiliado };
                return View("Index", afiliados);
            }
            else
            {
                //pendiente
                return RedirectToAction("GenericView");
            }
        }

        public ActionResult GenericView(int idmensaje = 0, string msg = null)
        {

            ViewModel GenericModel = new ViewModel();

            GenericModel.Title = "Afiliado";
            GenericModel.ActionName = "Filter";
            GenericModel.ControllerName = "Afiliado";

            if ( msg != null)
            {
                GenericModel.Message = msg;
            }
            else if (idmensaje == 0)
            {
                //ViewBag.Mensaje = "Mensaje no establecido.";
                GenericModel.Message = "Mensaje no establecido.";
            }
            else if (idmensaje == 1)
            {
                //ViewBag.Mensaje = "Ocurrió un error al subir el archivo al servidor. Operación cancelada.";
                GenericModel.Message = "Ocurrió un error al subir el archivo al servidor. Operación cancelada.";
            }
            else if (idmensaje == 2)
            {
                //ViewBag.Mensaje = "Registro creado satisfactoriamente.";
                GenericModel.Message = "Registro creado satisfactoriamente.";
            }
            else if (idmensaje == 3)
            {
                //ViewBag.Mensaje = "No se pudo crear el registro. Operación cancelada.";
                GenericModel.Message = "No se pudo crear el registro. Operación cancelada.";
            }
            //return View();
            return View(GenericModel);

        }


        public ActionResult Create(Afiliado afiliado)
        {
            return View(afiliado);
        }

        [HttpPost]
        public ActionResult Create(Afiliado afiliado, HttpPostedFileBase file)
        {

            if (rep.Save(afiliado))
            {
                //Aqui debo subir el archivo escaneado al servidor
                if (file != null && file.ContentLength > 0)
                    try
                    {
                        //para serverpath viejo
                        //string path = System.IO.Path.Combine(Server.MapPath(AppModule.GetPathPicture()), System.IO.Path.GetFileName(file.FileName));
                        //para serverpath nuevo
                        string path = Server.MapPath(AppModule.GetPathPicture().Replace("@filename@", afiliado.docnumber));
                        file.SaveAs(path);
                    }
                    //catch (Exception ex)
                    catch
                    {
                        return RedirectToAction("GenericView", new { idmensaje = 1 });
                    }
                else
                {
                    //ViewBag.Message2 = "Debe seleccionar un archivo.";
                    //MessageBox.Show("Debe seleccionar un archivo");

                }

                //PENDIENTE: SI FALLA ALGUNA DE LAS ACTIVIDADES. HAY QUE DESHACER LAS ACTIVIDADES ANTERIORES EXITOSAS.                
                
                return RedirectToAction("GenericView", new { idmensaje = 2 });
            }
            else
            {
                return RedirectToAction("GenericView", new { idmensaje = 3 });
            }
        }

        [HttpPost]
        public ActionResult Aprobar(string numdoc, string monto)
        {
            //metodo de repositorio que actualiza estatus y crea registro en cards
            return View("GenericView");
        }

        public ActionResult FilterReview()
        {
            return View();
        }


        public ActionResult Index(string numdoc)
        {
            List<Afiliado> afiliado = rep.FindSuma(numdoc, "", "");
            return View(afiliado);
        }

        [HttpPost]
        public ActionResult Index(string numdoc, string name, string email)
        {
            List<Afiliado> afiliado = rep.FindSuma(numdoc, name, email);
            return View(afiliado);
        }

        public ActionResult Edit(int id = 0)
        {
            Afiliado afiliado = rep.FindSuma(id);

            if (afiliado == null)
            {
                //return HttpNotFound();
                return RedirectToAction("GenericView");
            }
            return View(afiliado);

        }

        [HttpPost]
        public ActionResult Edit(Afiliado afiliado)
        {

            if (!rep.SaveChanges(afiliado))
            {
                return RedirectToAction("GenericView", new { msg = "Existen campos que son requeridos para procesar el formulario." });
                //return RedirectToAction("FilterReview");
            }

            //Aqui debo llamar a los servicios de actualización

            return RedirectToAction("GenericView", new { msg = "La información del afiliado ha sido actualizada satisfactoriamente." });
            //return RedirectToAction("FilterReview");
            //return View(afiliado);

        }

        public ActionResult ImprimirTarjeta(int id)
        {
            Afiliado afiliado = rep.FindSuma(id);
            return View("ImpresoraImprimirTarjeta", afiliado);
        }

        public ActionResult CrearPin(int id)
        {
            Afiliado afiliado = rep.FindSuma(id);
            return View("PinPadCrearPin", afiliado);
        }

        public ActionResult CambiarPin(int id)
        {
            Afiliado afiliado = rep.FindSuma(id);
            return View("PinPadCambiarPin", afiliado);
        }

        public ActionResult ReiniciarPin(int id)
        {
            Afiliado afiliado = rep.FindSuma(id);
            return View("PinPadReiniciarPin", afiliado);
        }

        public ActionResult SaldosMovimientos(int id)
        {
            SaldosMovimientos SaldosMovimientos = rep.FindSaldosMovimientos(id);
            return View(SaldosMovimientos);
        }

        public ActionResult Acreditar(int id)
        {
            Afiliado afiliado = rep.FindSuma(id);
            return View(afiliado);
        }

        [HttpPost]
        public ActionResult Acreditar(string numdoc, string monto)
        {
            RespuestaCards RespuestaCards = rep.Acreditar(numdoc, monto);
            return View("GenericView");
        }

        public ActionResult BloquearTarjeta(int id)
        {
            return View("GenericView");
        }

        public ActionResult SuspenderTarjeta(int id)
        {
            return View("GenericView");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}