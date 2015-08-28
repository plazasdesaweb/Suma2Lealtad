using Suma2Lealtad.Models;
using Suma2Lealtad.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers.Prepago
{
    public class OrdenRecargaPrepagoController : Controller
    {
        private ClientePrepagoRepository repCliente = new ClientePrepagoRepository();
        private BeneficiarioPrepagoRepository repBeneficiario = new BeneficiarioPrepagoRepository();
        private OrdenRecargaRepository repOrden = new OrdenRecargaRepository();

        public ActionResult Filter()
        {
            OrdenRecargaPrepago orden = new OrdenRecargaPrepago()
            {
                Cliente = new ClientePrepago()
                {
                    nameCliente = ""
                },
                ListaClientes = repOrden.GetClientes()
            };
            orden.ListaClientes.Insert(0, new PrepaidCustomer { id = 0, name = "" });
            return View(orden);
        }

        public ActionResult Create(int idCliente)
        {
            ClientePrepago cliente = repCliente.Find(idCliente);
            List<BeneficiarioPrepago> beneficiarios = repBeneficiario.Find("", "", "", "", "").Where(b => b.Cliente.idCliente == idCliente).ToList();
            List<DetalleOrdenRecargaPrepago> detalleOrden = repOrden.DetalleParaOrden(cliente, beneficiarios.FindAll(b => b.Afiliado.estatus == "Activa"));
            return View(detalleOrden);
        }

        [HttpPost]
        public ActionResult Create(int idCliente, IList<DetalleOrdenRecargaPrepago> detalleOrden, decimal MontoTotalRecargas)
        {
            int idOrden = repOrden.CrearOrden(idCliente, detalleOrden.ToList(), MontoTotalRecargas);
            if (idOrden != 0)
            {
                //viewmodel.Title = "Prepago / Cliente / Ordenes de Recarga / Detalle de la Orden";
                //viewmodel.Message = "Orden Aprobada.";
                //viewmodel.ControllerName = "ClientePrepago";
                //viewmodel.ActionName = "FilterOrdenes";
                //viewmodel.RouteValues = id.ToString();
                return RedirectToAction("DetalleOrden", new { id = idCliente, idOrden = idOrden });
            }
            else
            {
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Prepago / Ordenes de Recarga / Crear Orden";
                viewmodel.Message = "Falló el proceso de creación de la Orden.";
                viewmodel.ControllerName = "OrdenRecargaPrepago";
                viewmodel.ActionName = "Filter";
                return RedirectToAction("GenericView", viewmodel);
            }
        }

        public ActionResult CargarArchivoRecarga(int idCliente)
        {
            ClientePrepago cliente = repCliente.Find(idCliente);
            List<DetalleOrdenRecargaPrepago> detalleOrden = new List<DetalleOrdenRecargaPrepago>();
            DetalleOrdenRecargaPrepago detalle = new DetalleOrdenRecargaPrepago()
            {
                idCliente = cliente.idCliente,
                nameCliente = cliente.nameCliente,
                rifCliente = cliente.rifCliente,
                phoneCliente = cliente.phoneCliente
            };
            detalleOrden.Add(detalle);
            return View(detalleOrden);
        }

        [HttpPost]
        public ActionResult CreateArchivo(int idCliente, HttpPostedFileBase file)
        {
            ViewModel viewmodel = new ViewModel();
            //subir y guardar archivo
            string path = "";
            string idtemp = System.Web.HttpContext.Current.Session.SessionID + ".xls";
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    path = Server.MapPath(AppModule.GetPathPicture().Replace("@filename@.jpg", idtemp));
                    file.SaveAs(path);
                }
                catch (Exception ex)
                {
                    viewmodel.Title = "Prepago / Ordenes de Recarga / Crear Orden desde Archivo";
                    viewmodel.Message = "Error de aplicacion: No se pudo cargar archivo (" + ex.Message + ")";
                    viewmodel.ControllerName = "OrdenRecargaPrepago";
                    viewmodel.ActionName = "Filter";
                    return RedirectToAction("GenericView", viewmodel);
                }
                ClientePrepago cliente = repCliente.Find(idCliente);
                List<BeneficiarioPrepago> beneficiarios = repBeneficiario.Find("", "", "", "", "").Where(b => b.Cliente.idCliente == idCliente).ToList();
                //leer el archivo
                List<DetalleOrdenRecargaPrepago> detalleOrdenArchivo = repOrden.GetBeneficiariosArchivo(path);
                //borrar el archivo
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                if (detalleOrdenArchivo == null)
                {
                    viewmodel.Title = "Prepago / Ordenes de Recarga / Crear Orden desde Archivo";
                    viewmodel.Message = "Error de aplicacion: No se pudo leer el archivo.";
                    viewmodel.ControllerName = "OrdenRecargaPrepago";
                    viewmodel.ActionName = "Filter";
                    return RedirectToAction("GenericView", viewmodel);
                }
                else
                {
                    List<DetalleOrdenRecargaPrepago> detalleOrden = repOrden.DetalleParaOrdenArchivo(cliente, beneficiarios.FindAll(b => b.Afiliado.estatus == "Activa"), detalleOrdenArchivo);
                    return View("Create", detalleOrden);
                }
            }
            else
            {
                //borrar el archivo
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                viewmodel.Title = "Prepago / Ordenes de Recarga / Crear Orden desde Archivo";
                viewmodel.Message = "Error de aplicacion: El archivo está vacío";
                viewmodel.ControllerName = "OrdenRecargaPrepago";
                viewmodel.ActionName = "Filter";
                return RedirectToAction("GenericView", viewmodel);
            }
        }

        public FileResult DescargarArchivoFilePath()
        {
            string file = Server.MapPath("~/App_Data/Plantilla.xls");
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(file, contentType, Path.GetFileName(file));
        }

        public ActionResult FilterReview()
        {
            OrdenRecargaPrepago orden = new OrdenRecargaPrepago();
            return View(orden);
        }

        [HttpPost]
        public ActionResult FilterReview(string numero, string fecha, string estadoOrden)
        {
            List<OrdenRecargaPrepago> ordenes = new List<OrdenRecargaPrepago>();
            OrdenRecargaPrepago orden;
            if (numero != "")
            {
                orden = repOrden.Find(Convert.ToInt32(numero));
                if (orden != null)
                {
                    ordenes.Add(orden);
                }
            }
            else
            {
                ordenes = repOrden.Find(fecha, estadoOrden).OrderBy(x => x.Cliente.nameCliente).ThenBy(y => y.id).ToList();
            }
            return View("Index", ordenes);
        }

        public ActionResult GenericView(ViewModel viewmodel)
        {
            return View(viewmodel);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        
    }
}
