﻿using Suma2Lealtad.Models;
using Suma2Lealtad.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers.Prepago
{
    public class BeneficiarioPrepagoController : Controller
    {
        private const int ID_TYPE_PREPAGO = 2;
        private ClientePrepagoRepository repCliente = new ClientePrepagoRepository();        
        private BeneficiarioPrepagoRepository repBeneficiario = new BeneficiarioPrepagoRepository();
        private AfiliadoSumaRepository repAfiliado = new AfiliadoSumaRepository();
        
        public ActionResult Filter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Filter(string numdoc)
        {
            BeneficiarioPrepago beneficiario = repBeneficiario.Find(numdoc, "", "", "", "").FirstOrDefault();
            //NO ES Beneficiario PrepagoPlazas
            if (beneficiario == null)
            {
                beneficiario = new BeneficiarioPrepago()
                {
                    Afiliado = repAfiliado.Find(numdoc),
                    Cliente = new ClientePrepago()
                    {
                        ListaClientes = repBeneficiario.GetClientes()
                    }
                };
                beneficiario.Cliente.ListaClientes.Insert(0, new PrepaidCustomer { id = 0, name = "" });
                beneficiario.Afiliado.typeid = ID_TYPE_PREPAGO;
                beneficiario.Afiliado.type = "Prepago";
                return View("Create", beneficiario);
            }
            //ES Beneficiario PrepagoPlazas
            else
            {
                beneficiario.Afiliado = repAfiliado.Find(beneficiario.Afiliado.id);
                return View("Edit", beneficiario);                
            }
        }

        public ActionResult FilterReview()
        {
            BeneficiarioPrepago beneficiario = new BeneficiarioPrepago();
            return View(beneficiario);
        }

        [HttpPost]
        public ActionResult FilterReview(string numdoc, string name, string email, string estadoAfiliacion, string estadoTarjeta)
        {
            List<BeneficiarioPrepago> beneficiarios = repBeneficiario.Find(numdoc, name, email, estadoAfiliacion, estadoTarjeta).OrderBy(x => x.Cliente.nameCliente).ThenBy(y => y.Afiliado.docnumber).ToList();
            return View("Index", beneficiarios);
        }

        public ActionResult EditBeneficiario(int id)
        {
            BeneficiarioPrepago beneficiario = repBeneficiario.Find(id);
            return View("Edit",beneficiario);
        }

        [HttpPost]
        public ActionResult EditBeneficiario(AfiliadoSuma afiliado, ClientePrepago Cliente)
        {
            ViewModel viewmodel = new ViewModel();
            if (!repAfiliado.SaveChanges(afiliado))
            {
                viewmodel.Title = "Prepago / Beneficiario / Revisar Afiliación";
                viewmodel.Message = "Error de aplicacion: No se pudo actualizar afiliación.";
                viewmodel.ControllerName = "BeneficiarioPrepago";
                viewmodel.ActionName = "FilterReview";
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / Revisar Afiliación";
                viewmodel.Message = "La información del beneficiario ha sido actualizada satisfactoriamente.";
                viewmodel.ControllerName = "BeneficiarioPrepago";
                viewmodel.ActionName = "FilterReview";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult AprobarBeneficiario(int id)
        {
            ViewModel viewmodel = new ViewModel();
            BeneficiarioPrepago beneficiario = repBeneficiario.Find(id);
            if (repAfiliado.Aprobar(beneficiario.Afiliado))
            {
                viewmodel.Title = "Prepago / Beneficiario / Aprobar Afiliación:";
                viewmodel.Message = "Afiliación Aprobada.";
                viewmodel.ControllerName = "BeneficiarioPrepago";
                viewmodel.ActionName = "FilterReview";
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / Aprobar Afiliación:";
                viewmodel.Message = "Error de aplicacion: No se pudo aprobar afiliación.";
                viewmodel.ControllerName = "BeneficiarioPrepago";
                viewmodel.ActionName = "FilterReview";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult ImprimirTarjeta(int id)
        {
            BeneficiarioPrepago beneficiario = repBeneficiario.Find(id);
            beneficiario.Afiliado.trackI = Tarjeta.ConstruirTrackI(beneficiario.Afiliado.pan);
            beneficiario.Afiliado.trackII = Tarjeta.ConstruirTrackII(beneficiario.Afiliado.pan);            
            return View("ImpresoraImprimirTarjeta", beneficiario);
        }

        public ActionResult ReImprimirTarjeta(int id, int idCliente)
        {
            ViewModel viewmodel = new ViewModel();
            //BeneficiarioPrepago beneficiario = new BeneficiarioPrepago()
            //{
            //    Afiliado = repAfiliado.Find(id),
            //    Cliente = repCliente.Find(idCliente)
            //};
            BeneficiarioPrepago beneficiario = repBeneficiario.Find(id);
            if (repAfiliado.ImprimirTarjeta(beneficiario.Afiliado))
            {
                if (repAfiliado.BloquearTarjeta(beneficiario.Afiliado))
                {
                    beneficiario.Afiliado.trackI = Tarjeta.ConstruirTrackI(beneficiario.Afiliado.pan);
                    beneficiario.Afiliado.trackII = Tarjeta.ConstruirTrackII(beneficiario.Afiliado.pan);                        
                    return View("ImpresoraImprimirTarjeta", beneficiario);
                }
                else
                {
                    viewmodel.Title = "Prepago / Beneficiario / ReImprimir Tarjeta";
                    viewmodel.Message = "Falló el proceso de reimpresión de la Tarjeta";
                    viewmodel.ControllerName = "BeneficiarioPrepago";
                    viewmodel.ActionName = "FilterReview";
                    return RedirectToAction("GenericView", viewmodel);
                }
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / ReImprimir Tarjeta";
                viewmodel.Message = "Falló el proceso de reimpresión de la Tarjeta";
                viewmodel.ControllerName = "BeneficiarioPrepago";
                viewmodel.ActionName = "FilterReview";
                return RedirectToAction("GenericView", viewmodel);
            }
        }

        [HttpPost]
        public ActionResult ImprimirTarjeta(int id, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            BeneficiarioPrepago beneficiario = repBeneficiario.Find(id);
            //beneficiario.Afiliado.trackI = Tarjeta.ConstruirTrackI(beneficiario.Afiliado.pan);
            //beneficiario.Afiliado.trackII = Tarjeta.ConstruirTrackII(beneficiario.Afiliado.pan);
            if (repAfiliado.ImprimirTarjeta(beneficiario.Afiliado))
            {
                viewmodel.Title = "Prepago / Beneficiario / Operaciones con la Impresora / Imprimir Tarjeta";
                viewmodel.Message = "Tarjeta impresa y activada correctamente";
                viewmodel.ControllerName = "BeneficiarioPrepago";
                viewmodel.ActionName = "FilterReview";
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / Operaciones con la Impresora / Imprimir Tarjeta";
                viewmodel.Message = "Falló el proceso de impresión y activación de la Tarjeta.";
                viewmodel.ControllerName = "BeneficiarioPrepago";
                viewmodel.ActionName = "FilterReview";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult CrearPin(int id)
        {
            BeneficiarioPrepago beneficiario = repBeneficiario.Find(id);
            return View("PinPadCrearPin", beneficiario);
        }

        public ActionResult CambiarPin(int id)
        {
            BeneficiarioPrepago beneficiario = repBeneficiario.Find(id);
            return View("PinPadCambiarPin", beneficiario);
        }

        public ActionResult ReiniciarPin(int id)
        {
            BeneficiarioPrepago beneficiario = repBeneficiario.Find(id);
            return View("PinPadReiniciarPin", beneficiario);
        }

        public ActionResult BloquearTarjeta(int id)
        {
            BeneficiarioPrepago beneficiario = repBeneficiario.Find(id);
            return View(beneficiario);
        }

        [HttpPost]
        public ActionResult BloquearTarjeta(int id, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            if (repAfiliado.BloquearTarjeta(afiliado))
            {
                viewmodel.Title = "Prepago / Beneficiario / Bloquear Tarjeta";
                viewmodel.Message = "Tarjeta bloqueada correctamente";
                viewmodel.ControllerName = "BeneficiarioPrepago";
                viewmodel.ActionName = "FilterReview";
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / Bloquear Tarjeta";
                viewmodel.Message = "Falló el proceso de bloqueo de la Tarjeta";
                viewmodel.ControllerName = "BeneficiarioPrepago";
                viewmodel.ActionName = "FilterReview";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult SuspenderTarjeta(int id)
        {
            BeneficiarioPrepago beneficiario = repBeneficiario.Find(id);
            return View(beneficiario);
        }

        [HttpPost]
        public ActionResult SuspenderTarjeta(int id, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            if (repAfiliado.SuspenderTarjeta(afiliado))
            {
                viewmodel.Title = "Prepago / Beneficiario / Suspender Tarjeta";
                viewmodel.Message = "Tarjeta suspendida correctamente";
                viewmodel.ControllerName = "BeneficiarioPrepago";
                viewmodel.ActionName = "FilterReview";
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / Suspender Tarjeta";
                viewmodel.Message = "Falló el proceso de suspensión de la Tarjeta";
                viewmodel.ControllerName = "BeneficiarioPrepago";
                viewmodel.ActionName = "FilterReview";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult ReactivarTarjeta(int id)
        {
            BeneficiarioPrepago beneficiario = repBeneficiario.Find(id);
            return View(beneficiario);
        }
        
        [HttpPost]
        public ActionResult ReactivarTarjeta(int id, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            AfiliadoSuma afiliado = repAfiliado.Find(id);
            if (repAfiliado.ReactivarTarjeta(afiliado))
            {
                viewmodel.Title = "Prepago / Beneficiario / Reactivar Tarjeta";
                viewmodel.Message = "Tarjeta reactivada correctamente";
                viewmodel.ControllerName = "BeneficiarioPrepago";
                viewmodel.ActionName = "FilterReview";
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / Reactivar Tarjeta";
                viewmodel.Message = "Falló el proceso de reactivación de la Tarjeta";
                viewmodel.ControllerName = "BeneficiarioPrepago";
                viewmodel.ActionName = "FilterReview";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult SaldosMovimientos(int id)
        {
            BeneficiarioPrepago beneficiario = repBeneficiario.Find(id);
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
                viewmodel.Title = "Prepago / Beneficiario / Saldos y Movimientos";
                viewmodel.Message = "Ha ocurrido un error de aplicación (FindSaldosMovimientos). Notifique al Administrador.";
                viewmodel.ControllerName = "BeneficiarioPrepago";
                viewmodel.ActionName = "FilterReview";
                return RedirectToAction("GenericView", viewmodel);
            }
        }

        public ActionResult GenericView(ViewModel viewmodel)
        {
            return View(viewmodel);
        }

    }
}
