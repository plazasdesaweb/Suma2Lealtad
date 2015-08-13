﻿using Suma2Lealtad.Models;
using Suma2Lealtad.Modules;
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

        public ActionResult ImprimirTarjeta(int id, int idBeneficiario)
        {
            BeneficiarioPrepago beneficiario = new BeneficiarioPrepago()
            {
                Afiliado = repAfiliado.Find(idBeneficiario),
                Cliente = repCliente.Find(id)
            };
            return View("ImpresoraImprimirTarjeta", beneficiario);
        }

        [HttpPost]
        public ActionResult ImprimirTarjeta(int id, int idBeneficiario, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(idBeneficiario);
            afiliado.trackI = Tarjeta.ConstruirTrackI(afiliado.pan);
            afiliado.trackII = Tarjeta.ConstruirTrackII(afiliado.pan);
            if (repAfiliado.ImprimirTarjeta(afiliado))
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Operacio.nes con la Impresora / Imprimir Tarjeta";
                viewmodel.Message = "Tarjeta impresa y activada correctamente";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = id.ToString();
            }
            else
            {
                viewmodel.Title = "Prepago / Cliente / Beneficiario / Operaciones con la Impresora / Imprimir Tarjeta";
                viewmodel.Message = "Falló el proceso de impresión y activación de la Tarjeta.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = id.ToString();
            }
            return RedirectToAction("GenericView", viewmodel);
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

        public ActionResult CreateOrdenRecarga(int id)
        {
            ClientePrepago cliente = repCliente.Find(id);
            List<BeneficiarioPrepago> beneficiarios = repBeneficiario.Find("", "", "", "", "").Where(b => b.Cliente.idCliente == id).ToList();
            List<DetalleOrdenRecargaPrepago> detalleOrden = repOrden.DetalleParaOrden(cliente, beneficiarios.FindAll(b => b.Afiliado.estatus == "Activa"));
            return View(detalleOrden);
        }

        [HttpPost]
        public ActionResult CreateOrdenRecarga(int id, IList<DetalleOrdenRecargaPrepago> detalleOrden, decimal MontoTotalRecargas)
        {
            int idOrden = repOrden.CrearOrden(id, detalleOrden.ToList(), MontoTotalRecargas);
            if (idOrden != 0)
            {
                //viewmodel.Title = "Prepago / Cliente / Ordenes de Recarga / Detalle de la Orden";
                //viewmodel.Message = "Orden Aprobada.";
                //viewmodel.ControllerName = "ClientePrepago";
                //viewmodel.ActionName = "FilterOrdenes";
                //viewmodel.RouteValues = id.ToString();
                return RedirectToAction("DetalleOrden", new { id = id, idOrden = idOrden });
            }
            else
            {
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Prepago / Cliente / Ordenes de Recarga / Detalle de la Orden";
                viewmodel.Message = "Falló el proceso de aprobación de la Orden.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterOrdenes";
                viewmodel.RouteValues = id.ToString();
                return RedirectToAction("GenericView", viewmodel);
            }
        }

        public ActionResult CargarArchivoRecarga(int id)
        {
            ClientePrepago cliente = repCliente.Find(id);
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
        public ActionResult CreateOrdenRecargaArchivo(int id, HttpPostedFileBase file)
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
                    viewmodel.Title = "Hay que poner titulo";
                    viewmodel.Message = "Error de aplicacion: No se pudo cargar archivo (" + ex.Message + ")";
                    viewmodel.ControllerName = "ClientePrepago";
                    viewmodel.ActionName = "FilterOrdenes";
                    viewmodel.RouteValues = id.ToString();
                    return RedirectToAction("GenericView", viewmodel);
                }
                ClientePrepago cliente = repCliente.Find(id);
                List<BeneficiarioPrepago> beneficiarios = repBeneficiario.Find("", "", "", "", "").Where(b => b.Cliente.idCliente == id).ToList();
                List<DetalleOrdenRecargaPrepago> detalleOrdenArchivo = repOrden.GetBeneficiariosArchivo(path);
                if (detalleOrdenArchivo == null)
                {
                    viewmodel.Title = "Hay que poner titulo";
                    viewmodel.Message = "Ocurrio un error al leer el archivo";
                    viewmodel.ControllerName = "ClientePrepago";
                    viewmodel.ActionName = "FilterOrdenes";
                    viewmodel.RouteValues = id.ToString();
                    return RedirectToAction("GenericView", viewmodel);
                }
                List<DetalleOrdenRecargaPrepago> detalleOrden = repOrden.DetalleParaOrdenArchivo(cliente, beneficiarios.FindAll(b => b.Afiliado.estatus == "Activa"), detalleOrdenArchivo);
                //borrar el archivo
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return View("CreateOrdenRecarga", detalleOrden);
            }
            else
            {
                //borrar el archivo
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                viewmodel.Title = "Hay que poner titulo";
                viewmodel.Message = "El archivo está vacío";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterOrdenes";
                viewmodel.RouteValues = id.ToString();
                return RedirectToAction("GenericView", viewmodel);
            }
        }

        public ActionResult FilterOrdenes(int id)
        {
            ClientePrepago cliente = repCliente.Find(id);
            return View(cliente);
        }

        [HttpPost]
        public ActionResult FilterOrdenes(int id, string numero, string fecha, string estadoOrden)
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
                ordenes = repOrden.Find(fecha, estadoOrden).Where(o => o.Cliente.idCliente == id).ToList();
            }
            if (ordenes.Count > 0)
            {
                return View("IndexOrdenes", ordenes);
            }
            else
            {
                orden = new OrdenRecargaPrepago()
                {
                    Cliente = repCliente.Find(id)
                };
                ordenes.Add(orden);
                return View("IndexOrdenes", ordenes);
            }
        }

        public ActionResult DetalleOrden(int id, int idOrden)
        {
            List<DetalleOrdenRecargaPrepago> detalleOrden = repOrden.FindDetalleOrden(idOrden);
            return View(detalleOrden);
        }

        [HttpPost]
        public ActionResult AprobarOrden(int id, int idOrden, IList<DetalleOrdenRecargaPrepago> detalleOrden, decimal MontoTotalRecargas)
        {
            if (repOrden.AprobarOrden(detalleOrden.ToList(), MontoTotalRecargas))
            {
                //viewmodel.Title = "Prepago / Cliente / Ordenes de Recarga / Detalle de la Orden";
                //viewmodel.Message = "Orden Aprobada.";
                //viewmodel.ControllerName = "ClientePrepago";
                //viewmodel.ActionName = "FilterOrdenes";
                //viewmodel.RouteValues = id.ToString();
                return RedirectToAction("DetalleOrden", new { id = id, idOrden = idOrden });
            }
            else
            {
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Prepago / Cliente / Ordenes de Recarga / Detalle de la Orden";
                viewmodel.Message = "Falló el proceso de aprobación de la Orden.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterOrdenes";
                viewmodel.RouteValues = id.ToString();
                return RedirectToAction("GenericView", viewmodel);
            }
        }

        public ActionResult RechazarOrden(int id, int idOrden)
        {
            ViewModel viewmodel = new ViewModel();
            if (repOrden.RechazarOrden(idOrden))
            {
                viewmodel.Title = "Prepago / Cliente / Ordenes de Recarga / Detalle de la Orden";
                viewmodel.Message = "Orden Rechazada.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterOrdenes";
                viewmodel.RouteValues = id.ToString();
            }
            else
            {
                viewmodel.Title = "Prepago / Cliente / Ordenes de Recarga / Detalle de la Orden";
                viewmodel.Message = "Falló el proceso de rechazo de la Orden.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterOrdenes";
                viewmodel.RouteValues = id.ToString();
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult ProcesarOrden(int id, int idOrden)
        {
            if (repOrden.ProcesarOrden(idOrden))
            {
                //ViewModel viewmodel = new ViewModel();
                //viewmodel.Title = "Prepago / Cliente / Ordenes de Recarga / Detalle de la Orden / Procesar Orden";
                //viewmodel.Message = "Orden Procesada.";
                //viewmodel.ControllerName = "ClientePrepago";
                //viewmodel.ActionName = "FilterOrdenes";
                //viewmodel.RouteValues = id.ToString();
                //return RedirectToAction("GenericView", viewmodel);
                //List<DetalleOrdenRecargaPrepago> detalleOrden = repOrden.FindDetalleOrden(idOrden);
                //return View("ResultadoOrden", detalleOrden);
                List<DetalleOrdenRecargaPrepago> detalleOrden = repOrden.FindDetalleOrden(idOrden);
                return View("DetalleOrden", detalleOrden);
            }
            else
            {
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Prepago / Cliente / Ordenes de Recarga / Detalle de la Orden / Procesar Orden";
                viewmodel.Message = "Falló el procesamiento de la Orden.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "FilterOrdenes";
                viewmodel.RouteValues = id.ToString();
                return RedirectToAction("GenericView", viewmodel);
            }
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
