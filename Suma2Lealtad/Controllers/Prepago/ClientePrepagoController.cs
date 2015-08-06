using Suma2Lealtad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers.Prepago
{
    public class ClientePrepagoController : Controller
    {
        private const int ID_TYPE_PREPAGO = 2;
        private ClientePrepagoRepository repCliente = new ClientePrepagoRepository();
        private BeneficiarioPrepagoRepository repBeneficiario = new BeneficiarioPrepagoRepository();
        private AfiliadoSumaRepository repAfiliado = new AfiliadoSumaRepository();
        private OrdenRecargaRepository repOrden = new OrdenRecargaRepository();

        public ActionResult Filter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Filter(string rif)
        {
            ClientePrepago cliente = repCliente.Find(rif);
            if (cliente != null)
            {
                return View("Index", cliente);
            }
            else
            {
                cliente.rifCliente = rif;
                return View("Create", cliente);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientePrepago cliente)
        {
            ViewModel viewmodel = new ViewModel();
            if (repCliente.Save(cliente))
            {
                viewmodel.Title = "Prepago / Cliente / Crear Cliente";
                viewmodel.Message = "Cliente creado exitosamente.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "Index";
                viewmodel.RouteValues = cliente.rifCliente;
            }
            else
            {
                viewmodel.Title = "Prepago / Cliente / Crear Cliente";
                viewmodel.Message = "Error de aplicacion: No se pudo crear el Cliente.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "IndexAll";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult FilterReview()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FilterReview(string name, string rif)
        {
            List<ClientePrepago> clientes = repCliente.Find(name, rif);
            return View("Index", clientes);
        }

        public ActionResult Index(string rif)
        {
            List<ClientePrepago> clientes = repCliente.Find("", rif);
            return View(clientes);
        }

        public ActionResult IndexAll()
        {
            List<ClientePrepago> clientes = repCliente.Find("", "");
            return View("Index", clientes);
        }

        public ActionResult Edit(int id)
        {
            ClientePrepago cliente = repCliente.Find(id);
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientePrepago cliente)
        {
            ViewModel viewmodel = new ViewModel();
            if (repCliente.SaveChanges(cliente))
            {
                viewmodel.Title = "Prepago / Cliente / Editar Cliente";
                viewmodel.Message = "La información del Cliente ha sido actualizada satisfactoriamente.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "Index";
                viewmodel.RouteValues = cliente.rifCliente;
            }
            else
            {
                viewmodel.Title = "Prepago / Cliente / Editar Cliente";
                viewmodel.Message = "Error de aplicacion: No se pudo crear el Cliente.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "IndexAll";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult Delete(int id)
        {
            ViewModel viewmodel = new ViewModel();
            ClientePrepago cliente = repCliente.Find(id);
            List<BeneficiarioPrepago> beneficiarios = repBeneficiario.Find("", "", "", "", "").Where(b => b.Cliente.idCliente == id).ToList();
            if (beneficiarios.Count != 0)
            {
                viewmodel.Title = "Prepapago / Cliente / Eliminar";
                viewmodel.Message = "No se puede eliminar este Cliente, tiene Beneficiarios asociados.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "IndexAll";
                return RedirectToAction("GenericView", viewmodel);
            }
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ClientePrepago cliente)
        {
            ViewModel viewmodel = new ViewModel();
            if (repCliente.BorrarCliente(cliente.idCliente))
            {
                viewmodel.Title = "Prepago / Cliente / Borrar Cliente";
                viewmodel.Message = "Cliente borradao exitosamente.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "IndexAll";
            }
            else
            {
                viewmodel.Title = "Prepago / Cliente / Borrar Cliente";
                viewmodel.Message = "Error de aplicacion: No se pudo borrar el Cliente.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "IndexAll";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult FilterBeneficiarios(int id)
        {
            ClientePrepago cliente = repCliente.Find(id);
            return View(cliente);
        }

        [HttpPost]
        public ActionResult FilterBeneficiarios(int id, string numdoc, string name, string email, string estadoAfiliacion, string estadoTarjeta)
        {
            List<BeneficiarioPrepago> beneficiarios = repBeneficiario.Find(numdoc, name, email, estadoAfiliacion, estadoTarjeta).Where(b => b.Cliente.idCliente == id).ToList();
            if (beneficiarios.Count > 0)
            {
                return View("IndexBeneficiarios", beneficiarios);
            }
            else
            {
                BeneficiarioPrepago beneficiario = new BeneficiarioPrepago()
                {
                    Cliente = repCliente.Find(id)
                };
                beneficiarios.Add(beneficiario);
                return View("IndexBeneficiarios", beneficiarios);
            }
        }

        public ActionResult FindBeneficiario(int id)
        {
            ClientePrepago cliente = repCliente.Find(id);
            return View(cliente);
        }

        [HttpPost]
        public ActionResult FindBeneficiario(int id, string numdoc)
        {
            BeneficiarioPrepago beneficiario = repBeneficiario.Find(numdoc, "", "", "", "").FirstOrDefault();
            //NO ES Beneficiario PrepagoPlazas
            if (beneficiario == null)
            {
                beneficiario = new BeneficiarioPrepago()
                {
                    Afiliado = repAfiliado.Find(numdoc),
                    Cliente = repCliente.Find(id)
                };
                beneficiario.Afiliado.typeid = ID_TYPE_PREPAGO;
                beneficiario.Afiliado.type = "Prepago";
                return View("CreateBeneficiario", beneficiario);
            }
            //ES Benefciario PrepagoPlazas
            else
            {
                //ES Beneficiario PrepagoPlazas de el cliente
                if (beneficiario.Cliente.idCliente == id)
                {
                    beneficiario.Afiliado = repAfiliado.Find(beneficiario.Afiliado.id);
                    return View("EditBeneficiario", beneficiario);
                }
                //ES Beneficiario Prepago Plazas de otro cliente
                else
                {
                    ViewModel viewmodel = new ViewModel();
                    viewmodel.Title = "Prepago / Cliente / Crear Beneficiario / Filtro de Búsqueda";
                    viewmodel.Message = "El Beneficiario está asociado con otro Cliente. No se puede asociar con el Cliente actual.";
                    viewmodel.ControllerName = "ClientePrepago";
                    viewmodel.ActionName = "FindBeneficiario";
                    viewmodel.RouteValues = id.ToString();
                    return RedirectToAction("GenericView", viewmodel);
                }
            }
        }

        [HttpPost]
        public ActionResult CreateBeneficiario(AfiliadoSuma Afiliado, ClientePrepago Cliente, HttpPostedFileBase file)
        {
            ViewModel viewmodel = new ViewModel();
            if (repAfiliado.Save(Afiliado, file))
            {
                BeneficiarioPrepago beneficiario = new BeneficiarioPrepago()
                {
                    Afiliado = Afiliado,
                    Cliente = Cliente
                };
                if (repBeneficiario.Save(beneficiario))
                {
                    viewmodel.Title = "Prepago / Cliente / Beneficiario / Crear Afiliación";
                    viewmodel.Message = "Solicitud de afiliación creada exitosamente.";
                    viewmodel.ControllerName = "ClientePrepago";
                    viewmodel.ActionName = "FilterBeneficiarios";
                    viewmodel.RouteValues = Cliente.idCliente.ToString();
                }
            }
            else
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Crear Afiliación";
                viewmodel.Message = "Error de aplicacion: No se pudo crear solicitud de afiliación.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = Cliente.idCliente.ToString();
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult EditBeneficiario(int id, int idBeneficiario)
        {
            BeneficiarioPrepago beneficiario = new BeneficiarioPrepago()
            {
                Afiliado = repAfiliado.Find(idBeneficiario),
                Cliente = repCliente.Find(id)
            };
            return View(beneficiario);
        }

        [HttpPost]
        public ActionResult EditBeneficiario(AfiliadoSuma Afiliado, ClientePrepago Cliente)
        {
            ViewModel viewmodel = new ViewModel();
            if (!repAfiliado.SaveChanges(Afiliado))
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Revisar Afiliación";
                viewmodel.Message = "Error de aplicacion: No se pudo actualizar afiliación.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = Cliente.idCliente.ToString();
            }
            else
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Revisar Afiliación";
                viewmodel.Message = "La información del beneficiario ha sido actualizada satisfactoriamente.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = Cliente.idCliente.ToString();
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult AprobarBeneficiario(int id, int idBeneficiario)
        {
            ViewModel viewmodel = new ViewModel();
            BeneficiarioPrepago beneficiario = new BeneficiarioPrepago()
            {
                Afiliado = repAfiliado.Find(idBeneficiario),
                Cliente = repCliente.Find(id)
            };
            if (repAfiliado.Aprobar(beneficiario.Afiliado))
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Aprobar Afiliación:";
                viewmodel.Message = "Afiliación Aprobada.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = beneficiario.Cliente.idCliente.ToString();
            }
            else
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Aprobar Afiliación:";
                viewmodel.Message = "Error de aplicacion: No se pudo aprobar afiliación.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = beneficiario.Cliente.idCliente.ToString();
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult FilterOrdenes(int id)
        {
            ClientePrepago cliente = repCliente.Find(id);
            return View(cliente);
        }

        [HttpPost]
        public ActionResult FilterOrdenes(int id, string fecha, string estadoOrden)
        {
            List<OrdenRecargaPrepago> ordenes = repOrden.Find(fecha, estadoOrden).Where(o => o.Cliente.idCliente == id).ToList(); ;
            if (ordenes.Count > 0)
            {
                return View("IndexOrdenes", ordenes);
            }
            else
            {
                OrdenRecargaPrepago orden = new OrdenRecargaPrepago()
                {
                    Cliente = repCliente.Find(id)
                };
                ordenes.Add(orden);
                return View("IndexOrdenes", ordenes);
            }
        }

        public ActionResult CrearPin(int id, int idBeneficiario)
        {
            BeneficiarioPrepago beneficiario = new BeneficiarioPrepago()
            {
                Afiliado = repAfiliado.Find(idBeneficiario),
                Cliente = repCliente.Find(id)
            };
            return View("PinPadCrearPin", beneficiario);
        }

        public ActionResult CambiarPin(int id, int idBeneficiario)
        {
            BeneficiarioPrepago beneficiario = new BeneficiarioPrepago()
            {
                Afiliado = repAfiliado.Find(idBeneficiario),
                Cliente = repCliente.Find(id)
            };
            return View("PinPadCambiarPin", beneficiario);
        }

        public ActionResult ReiniciarPin(int id, int idBeneficiario)
        {
            BeneficiarioPrepago beneficiario = new BeneficiarioPrepago()
            {
                Afiliado = repAfiliado.Find(idBeneficiario),
                Cliente = repCliente.Find(id)
            };
            return View("PinPadReiniciarPin", beneficiario);
        }

        public ActionResult BloquearTarjeta(int id, int idBeneficiario)
        {
            BeneficiarioPrepago beneficiario = new BeneficiarioPrepago()
            {
                Afiliado = repAfiliado.Find(idBeneficiario),
                Cliente = repCliente.Find(id)
            };
            return View(beneficiario);  
        }

        [HttpPost]
        public ActionResult BloquearTarjeta(int id, int idBeneficiario, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(idBeneficiario);
            if (repAfiliado.BloquearTarjeta(afiliado))
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Bloquear Tarjeta";
                viewmodel.Message = "Tarjeta bloqueada correctamente";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = id.ToString();
            }
            else
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Bloquear Tarjeta";
                viewmodel.Message = "Falló el proceso de bloqueo de la Tarjeta";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = id.ToString();
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult SuspenderTarjeta(int id, int idBeneficiario)
        {
            BeneficiarioPrepago beneficiario = new BeneficiarioPrepago()
            {
                Afiliado = repAfiliado.Find(idBeneficiario),
                Cliente = repCliente.Find(id)
            };
            return View(beneficiario);
        }

        [HttpPost]
        public ActionResult SuspenderTarjeta(int id, int idBeneficiario, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(idBeneficiario);
            if (repAfiliado.SuspenderTarjeta(afiliado))
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Suspender Tarjeta";
                viewmodel.Message = "Tarjeta suspendida correctamente";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = id.ToString();
            }
            else
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Suspender Tarjeta";
                viewmodel.Message = "Falló el proceso de suspención de la Tarjeta";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = id.ToString();
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult ReactivarTarjeta(int id, int idBeneficiario)
        {
            BeneficiarioPrepago beneficiario = new BeneficiarioPrepago()
            {
                Afiliado = repAfiliado.Find(idBeneficiario),
                Cliente = repCliente.Find(id)
            };
            return View(beneficiario);
        }

        [HttpPost]
        public ActionResult ReactivarTarjeta(int id, int idBeneficiario, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(idBeneficiario);
            if (repAfiliado.ReactivarTarjeta(afiliado))
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Reactivar Tarjeta";
                viewmodel.Message = "Tarjeta reactivada correctamente";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = id.ToString();
            }
            else
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Reactivar Tarjeta";
                viewmodel.Message = "Falló el proceso de reactivación de la Tarjeta";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = id.ToString();
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult SaldosMovimientos(int id, int idBeneficiario)
        {
            BeneficiarioPrepago beneficiario = new BeneficiarioPrepago()
            {
                Afiliado = repAfiliado.Find(idBeneficiario),
                Cliente = repCliente.Find(id)
            };
            SaldosMovimientos SaldosMovimientos = repAfiliado.FindSaldosMovimientos(beneficiario.Afiliado);
            if (SaldosMovimientos != null)
            {
                BeneficiarioPrepagoSaldosMovimientos beneficiarioSaldosMovimientos = new BeneficiarioPrepagoSaldosMovimientos()
                {
                    denominacionPrepago = "Bs.",
                    denominacionSuma = "Más.",
                    Beneficiario = beneficiario,
                    SaldosMovimientos = SaldosMovimientos,
                };
                return View(beneficiarioSaldosMovimientos);
            }
            else
            {
                //ERROR EN METODO FIND
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Saldos y Movimientos";
                viewmodel.Message = "Ha ocurrido un error de aplicación (FindSaldosMovimientos). Notifique al Administrador.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = id.ToString();
                return RedirectToAction("GenericView", viewmodel);
            }
        }

        public ActionResult Acreditar(int id, int idBeneficiario)
        {
            BeneficiarioPrepago beneficiario = new BeneficiarioPrepago()
            {
                Afiliado = repAfiliado.Find(idBeneficiario),
                Cliente = repCliente.Find(id)
            };
            return View(beneficiario);            
        }

        [HttpPost]
        public ActionResult Acreditar(int id, int idBeneficiario, string monto, string observaciones)
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(idBeneficiario);
            if (repAfiliado.Acreditar(afiliado, monto))
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Acreditar";
                viewmodel.Message = "Acreditación exitosa.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = id.ToString();
            }
            else
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Acreditar ";
                viewmodel.Message = "Falló el proceso de acreditación.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = id.ToString();
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult GenericView(ViewModel viewmodel)
        {
            return View(viewmodel);
        }

        public FileContentResult GetImageBeneficiario(int id)
        {
            Photos_Affiliate Photo = repAfiliado.Find(id).picture;
            if (Photo.photo != null)
            {
                return File(Photo.photo, Photo.photo_type);
            }
            else
            {
                return null;
            }
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
