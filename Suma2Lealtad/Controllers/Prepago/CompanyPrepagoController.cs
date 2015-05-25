using Suma2Lealtad.Filters;
using Suma2Lealtad.Models;
using Suma2Lealtad.Modules;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers
{
    [AuditingFilter]
    public class CompanyPrepagoController : Controller
    {
        private CompanyRepository rep = new CompanyRepository();
        private AfiliadoRepository repAfiliado = new AfiliadoRepository();

        public ActionResult FilterCompany()
        {
            return View();
        }

        public ActionResult Index(string rif)
        {
            List<PrepagoCompanyAffiliattes> compañias = rep.Find(rif, "");
            return View(compañias);
        }

        [HttpPost]
        public ActionResult FindCompany(string rif, string name)
        {
            List<PrepagoCompanyAffiliattes> compañias = rep.Find(rif, name);
            if (compañias.Count == 0)
            {
                //NOCLIENTE/NOAFILIADO
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Prepago / Compañia / Buscar Compañia";
                viewmodel.Message = "No se ha(n) encontrado la(las) Compañia(s) con los datos suministrados";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "FilterCompany";
                return RedirectToAction("GenericView", viewmodel);
            }
            return View("Index", compañias);
        }

        public ActionResult Details(int id)
        {
            Company company = rep.FindCompany(id);
            return View(company);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            ViewModel viewmodel = new ViewModel();
            if (rep.Save(company))
            {
                viewmodel.Title = "Prepago / Compañia / Crear Compañia";
                viewmodel.Message = "Compañia creada exitosamente.";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "Index";
                viewmodel.RouteValues = company.rif;
            }
            else
            {
                viewmodel.Title = "Prepago / Compañia / Crear Compañia";
                viewmodel.Message = "Error de aplicacion: No se pudo crear Compañia.";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "FilterCompany";
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult Edit(int id)
        {
            Company company = rep.FindCompany(id);
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            ViewModel viewmodel = new ViewModel();
            if (!rep.SaveChanges(company))
            {
                viewmodel.Title = "Prepago / Compañia / Editar Compañia";
                viewmodel.Message = "Existen campos que son requeridos para procesar el formulario.";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "Index";
                viewmodel.RouteValues = company.rif;
            }
            else
            {
                viewmodel.Title = "Prepago / Compañia / Editar Compañia";
                viewmodel.Message = "La información de la Compañia ha sido actualizada satisfactoriamente.";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "Index";
                viewmodel.RouteValues = company.rif;
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult Delete(int id)
        {
            ViewModel viewmodel = new ViewModel();
            PrepagoCompanyAffiliattes compañia = rep.Find(id);
            if (compañia.Beneficiarios.Count != 0)
            {

                viewmodel.Title = "Prepapago / Compañia / Eliminar";
                viewmodel.Message = "No se puede eliminar esta compañia, tiene beneficiarios asociados.";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "Index";
                viewmodel.RouteValues = compañia.rif;
                return RedirectToAction("GenericView", viewmodel);
            }
            return View(compañia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewModel viewmodel = new ViewModel();
            if (rep.BorrarCompañia(id))
            {
                viewmodel.Title = "Prepago / Compañia / Borrar Compañia";
                viewmodel.Message = "Compañia borrada exitosamente.";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "FilterCompany";
            }
            else
            {
                PrepagoCompanyAffiliattes compañia = rep.Find(id);
                viewmodel.Title = "Prepago / Compañia / Borrar Compañia";
                viewmodel.Message = "Error de aplicacion: No se pudo borrar Compañia.";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "Index";
                viewmodel.RouteValues = compañia.rif;
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult Beneficiarios(int companyid)
        {
            PrepagoCompanyAffiliattes compañiaBeneficiarios = rep.Find(companyid);
            return View(compañiaBeneficiarios);
        }

        [HttpPost]
        public ActionResult Beneficiarios(int companyid, string numdoc, string name, string email)
        {
            PrepagoCompanyAffiliattes compañiaBeneficiarios;
            if (numdoc == null && name == null && email == null)
            {
                compañiaBeneficiarios = rep.Find(companyid);
            }
            else 
            {
                compañiaBeneficiarios = rep.FindBeneficiarios(companyid, numdoc, name, email);
            }           
            return View(compañiaBeneficiarios);
        }

        public ActionResult FilterBeneficiarios(int companyid)
        {
            PrepagoCompanyAffiliattes compañiaBeneficiarios = rep.Find(companyid);
            return View(compañiaBeneficiarios);
        }

        //[HttpPost]
        //public ActionResult CargarBeneficiarios(int companyid)
        //{
        //    PrepagoCompanyAffiliattes compañiaBeneficiarios = rep.Find(companyid);
        //    return View("Beneficiarios",compañiaBeneficiarios);
        //}

        public ActionResult Filter(int companyid)
        {
            Afiliado afiliado = new Afiliado();
            afiliado.companyid = companyid;
            return View(afiliado);
        }

        [HttpPost]
        public ActionResult Find(string numdoc, int companyid, int typeid = 2)
        {
            //        //Los estados actuales para una persona son:
            //        //NOCLIENTE                         (no registrado en WEBPLAZAS)
            //        //NOAFILIADO                        (no afiliado en SUMAPLAZAS)
            //        //CLIENTE                           (registrado en WEBPLAZAS)
            //        //AFILIADO SUMA                     (afiliado en SUMAPLAZAS)
            //        //BENEFCIARIO PREPAGO COMPAÑIA      (afiliado en SUMAPLAZAS)
            //        //BENEFCIARIO PREPAGO OTRA COMPAÑIA (afiliado en SUMAPLAZAS)
            //        //El estado deseado es:
            //        //CLIENTE/BENEFCIARIO PREPAGO COMPAÑIA     (registrado en WEBPLAZAS y afiliado en SUMAPLAZAS TIPO PREPAGO) 
            //        //Existen los siguientes resultados posibles para esta búsqueda
            //        //NOCLIENTE/NOAFILIADO                        -> por definir acción para crear registro de CLIENTE y crear afiliación de AFILIADO => Redireccionar a GenericView con mensaje descriptivo
            //        //NOCLIENTE/AFILIADO SUMA                     -> por definir acción para crear registro de CLIENTE => Redireccionar a GenericView con mensaje descriptivo
            //        //NOCLIENTE/BENEFCIARIO PREPAGO COMPAÑIA      -> por definir acción para crear registro de CLIENTE => Redireccionar a GenericView con mensaje descriptivo
            //        //NOCLIENTE/BENEFCIARIO PREPAGO OTRA COMPAÑIA -> por definir acción para crear registro de CLIENTE => Redireccionar a GenericView con mensaje descriptivo            
            //        //CLIENTE/NOAFILIADO                          -> acción: editar registro de CLIENTE y crear afiliación de AFILIADO => CREAR AFILIACION PREPAGO (retornar vista Create)
            //        //CLIENTE/AFILIADO SUMA                       -> acción: error cliente suma => Redireccionar a GenericView con mensaje descriptivo            
            //        //CLIENTE/BENEFCIARIO PREPAGO                 -> acción: editar registro de CLIENTE y editar afiliación de BENEFCIARIO PREPAGO => REVISAR AFILIACION (Redirecciónar a acción Index ó Edit)
            //        //CLIENTE/BENEFCIARIO PREPAGO OTRA COMPAÑIA   -> acción: error cliente prepago otra compañia => Redireccionar a GenericView con mensaje descriptivo

            Afiliado afiliado = repAfiliado.Find(numdoc, typeid, companyid);
            if (afiliado == null)
            {
                //ERROR EN METODO FIND
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Prepago / Beneficiario / Buscar Cliente";
                viewmodel.Message = "Ha ocurrido un error de aplicación (Find). Notifique al Administrador.";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "Beneficiarios";
                viewmodel.RouteValues = companyid.ToString();
                return RedirectToAction("GenericView", viewmodel);
            }
            if (afiliado.id == -1)
            {
                //CLIENTE/AFILIADO SUMA
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Prepago / Beneficiario / Buscar Cliente";
                viewmodel.Message = "Error: El cliente ya es afiliado Suma. (Escenario CLIENTE/AFILIADO SUMA)";
                viewmodel.ControllerName = "CompanyPrepago";
                //viewmodel.ActionName = "Beneficiarios";
                viewmodel.ActionName = "Filter";
                viewmodel.RouteValues = companyid.ToString();
                return RedirectToAction("GenericView", viewmodel);
            }
            if (afiliado.id == -2)
            {
                //CLIENTE/BENEFCIARIO PREPAGO OTRA COMPAÑIA
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Prepago / Beneficiario / Buscar Cliente";
                viewmodel.Message = "Error: El cliente ya es beneficiario de otra compañia. (Escenario CLIENTE/BENEFCIARIO PREPAGO OTRA COMPAÑIA)";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "Beneficiarios";
                viewmodel.RouteValues = companyid.ToString();
                return RedirectToAction("GenericView", viewmodel);
            }
            if (afiliado.clientid == 0 && afiliado.id == 0)
            {
                //NOCLIENTE/NOAFILIADO
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Prepago / Beneficiario / Buscar Cliente";
                viewmodel.Message = "El Cliente no esta registrado en WebPlazas. (Escenario NOCLIENTE/NOAFILIADO)";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "Beneficiarios";
                viewmodel.RouteValues = companyid.ToString();
                return RedirectToAction("GenericView", viewmodel);
            }
            else if (afiliado.clientid == 0 && afiliado.id != 0)
            {
                //NOCLIENTE/AFILIADO SUMA, NOCLIENTE/BENEFCIARIO PREPAGO COMPAÑIA, NOCLIENTE/BENEFCIARIO PREPAGO OTRA COMPAÑIA
                ViewModel viewmodel = new ViewModel();
                viewmodel.Title = "Prepago / Beneficiario / Buscar Cliente";
                viewmodel.Message = "El Cliente no esta registrado en WebPlazas. (Escenario NOCLIENTE/AFILIADO SUMA, NOCLIENTE/BENEFCIARIO PREPAGO COMPAÑIA, NOCLIENTE/BENEFCIARIO PREPAGO OTRA COMPAÑIA)";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "Beneficiarios";
                viewmodel.RouteValues = companyid.ToString();
                return RedirectToAction("GenericView", viewmodel);
            }
            else if (afiliado.clientid != 0 && afiliado.id == 0)
            {
                //CLIENTE/NOAFILIADO
                return RedirectToAction("Create", "Afiliado", new { numdoc = afiliado.docnumber, typeid, afiliado.companyid });
            }
            else
            {
                //CLIENTE/BENEFCIARIO PREPAGO COMPAÑIA
                PrepagoCompanyAffiliattes compañiaBeneficiarios = rep.FindBeneficiarios(companyid, numdoc, "", "");
                return View("Beneficiarios", compañiaBeneficiarios);
            }
        }

        [HttpPost]
        public ActionResult CreateBeneficiario(Afiliado afiliado, HttpPostedFileBase file)
        {
            string companyid = afiliado.companyid.ToString();
            ViewModel viewmodel = new ViewModel();
            if (repAfiliado.Save(afiliado, file))
            {
                viewmodel.Title = "Prepago / Beneficiario / Crear Afiliación";
                viewmodel.Message = "Solicitud de afiliación creada exitosamente.";
                viewmodel.ControllerName = "CompanyPrepago";
                //viewmodel.ActionName = "CargarBeneficiarios";
                viewmodel.ActionName = "Beneficiarios";
                viewmodel.RouteValues = companyid;
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / Crear Afiliación";
                viewmodel.Message = "Error de aplicacion: No se pudo crear solicitud de afiliación.";
                viewmodel.ControllerName = "CompanyPrepago";
                //viewmodel.ActionName = "CargarBeneficiarios";
                viewmodel.ActionName = "Beneficiarios";
                viewmodel.RouteValues = companyid;
            }
            return RedirectToAction("GenericView", viewmodel);
        }


        public ActionResult EditBeneficiario(int id)
        {
            Afiliado afiliado = repAfiliado.Find(id);
            return RedirectToAction("Edit", "Afiliado", new { id = afiliado.id});
        }

        [HttpPost]
        public ActionResult EditBeneficiario(Afiliado afiliado)
        {
            string companyid = afiliado.companyid.ToString();
            ViewModel viewmodel = new ViewModel();
            if (!repAfiliado.SaveChanges(afiliado))
            {
                viewmodel.Title = "Prepago / Beneficiario / Revisar Afiliación";
                viewmodel.Message = "Existen campos que son requeridos para procesar el formulario.";
                viewmodel.ControllerName = "CompanyPrepago";
                //viewmodel.ActionName = "CargarBeneficiarios";
                viewmodel.ActionName = "Beneficiarios";
                viewmodel.RouteValues = companyid;
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / Revisar Afiliación";
                viewmodel.Message = "La información del beneficiario ha sido actualizada satisfactoriamente.";
                viewmodel.ControllerName = "CompanyPrepago";
                //viewmodel.ActionName = "CargarBeneficiarios";
                viewmodel.ActionName = "Beneficiarios";
                viewmodel.RouteValues = companyid;
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult AprobarBeneficiario(int id)
        {
            ViewModel viewmodel = new ViewModel();
            Afiliado afiliado = repAfiliado.Find(id);
            string companyid = afiliado.companyid.ToString();
            if (repAfiliado.Aprobar(afiliado))
            {
                viewmodel.Title = "Prepago / Beneficiario / Aprobar Afiliación:";
                viewmodel.Message = "Afiliación Aprobada.";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "Beneficiarios";
                viewmodel.RouteValues = companyid;
            }
            else
            {
                viewmodel.Title = "Afiliado / Aprobar Afiliación:";
                viewmodel.Message = "Aprobación Fallida.";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "Beneficiarios";
                viewmodel.RouteValues = companyid;
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult CrearPin(int id)
        {
            Afiliado afiliado = repAfiliado.Find(id);
            return RedirectToAction("PinPadCrearPin", "Afiliado", new { id = afiliado.id });
        }

        public ActionResult CambiarPin(int id)
        {
            Afiliado afiliado = repAfiliado.Find(id);
            return RedirectToAction("PinPadCambiarPin", "Afiliado", new { id = afiliado.id });
        }

        public ActionResult ReiniciarPin(int id)
        {
            Afiliado afiliado = repAfiliado.Find(id);
            return RedirectToAction("PinPadReiniciarPin", "Afiliado", new { id = afiliado.id });
        }

        public ActionResult ImprimirTarjeta(int id)
        {
            Afiliado afiliado = repAfiliado.Find(id);
            return RedirectToAction("ImpresoraImprimirTarjeta", "Afiliado", new { id = afiliado.id });
        }

        [HttpPost]
        public ActionResult ImprimirTarjeta(int id, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            Afiliado afiliado = repAfiliado.Find(id);
            afiliado.trackI = Tarjeta.ConstruirTrackI(afiliado.pan);
            afiliado.trackII = Tarjeta.ConstruirTrackII(afiliado.pan);
            if (repAfiliado.ImprimirTarjeta(afiliado))
            {
                viewmodel.Title = "Prepago / Beneficiario / Operaciones con la Impresora / Imprimir Tarjeta";
                viewmodel.Message = "Tarjeta impresa y activada correctamente";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = afiliado.companyid.ToString();
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / Operaciones con la Impresora / Imprimir Tarjeta";
                viewmodel.Message = "Falló el proceso de impresión y activación de la Tarjeta";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = afiliado.companyid.ToString();
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult BloquearTarjeta(int id)
        {
            Afiliado afiliado = repAfiliado.Find(id);
            return RedirectToAction("BloquearTarjeta", "Afiliado", new { id = afiliado.id });
        }

        [HttpPost]
        public ActionResult BloquearTarjeta(int id, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            Afiliado afiliado = repAfiliado.Find(id);
            if (repAfiliado.BloquearTarjeta(afiliado))
            {
                viewmodel.Title = "Prepago / Beneficiario / Bloquear Tarjeta";
                viewmodel.Message = "Tarjeta bloqueada correctamente";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = afiliado.companyid.ToString();
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / Bloquear Tarjeta";
                viewmodel.Message = "Falló el proceso de bloqueo de la Tarjeta";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = afiliado.companyid.ToString();
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult SuspenderTarjeta(int id)
        {
            Afiliado afiliado = repAfiliado.Find(id);
            return RedirectToAction("SuspenderTarjeta", "Afiliado", new { id = afiliado.id });
        }

        [HttpPost]
        public ActionResult SuspenderTarjeta(int id, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            Afiliado afiliado = repAfiliado.Find(id);
            if (repAfiliado.SuspenderTarjeta(afiliado))
            {
                viewmodel.Title = "Prepago / Beneficiario / Suspender Tarjeta";
                viewmodel.Message = "Tarjeta suspendida correctamente";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = afiliado.companyid.ToString();
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / Suspender Tarjeta";
                viewmodel.Message = "Falló el proceso de suspensión de la Tarjeta";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = afiliado.companyid.ToString();
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult ReactivarTarjeta(int id)
        {
            Afiliado afiliado = repAfiliado.Find(id);
            return RedirectToAction("ReactivarTarjeta", "Afiliado", new { id = afiliado.id });
        }

        [HttpPost]
        public ActionResult ReactivarTarjeta(int id, string mode = "post")
        {
            ViewModel viewmodel = new ViewModel();
            Afiliado afiliado = repAfiliado.Find(id);
            if (repAfiliado.ReactivarTarjeta(afiliado))
            {
                viewmodel.Title = "Prepago / Beneficiario / Reactivar Tarjeta";
                viewmodel.Message = "Tarjeta reactivada correctamente";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = afiliado.companyid.ToString();
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / Reactivar Tarjeta";
                viewmodel.Message = "Falló el proceso de reactivación de la Tarjeta";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = afiliado.companyid.ToString();
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult SaldosMovimientos(int id)
        {
            //Afiliado afiliado = repAfiliado.Find(id);
            //SaldosMovimientos SaldosMovimientos = repAfiliado.FindSaldosMovimientos(afiliado);
            //if (SaldosMovimientos == null)
            //{
            //    //ERROR EN METODO FIND
            //    ViewModel viewmodel = new ViewModel();
            //    viewmodel.Title = "Prepago / Beneficiario / Saldos y Movimientos";
            //    viewmodel.Message = "Ha ocurrido un error de aplicación (FindSaldosMovimientos). Notifique al Administrador.";
            //    viewmodel.ControllerName = "CompanyPrepago";
            //    viewmodel.ActionName = "FilterBeneficiarios";
            //    viewmodel.RouteValues = afiliado.companyid.ToString();
            //    return RedirectToAction("GenericView", viewmodel);
            //}
            //AfiliadoSaldosMovimientos AfiliadoSaldosMovimientos = new AfiliadoSaldosMovimientos()
            //{
            //    denominacionPrepago = "Bs.",
            //    denominacionSuma = "Más.",
            //    Afiliado = afiliado,
            //    SaldosMovimientos = SaldosMovimientos
            //};
            //return View(AfiliadoSaldosMovimientos);
            Afiliado afiliado = repAfiliado.Find(id);
            return RedirectToAction("SaldosMovimientos", "Afiliado", new { id = afiliado.id });
        }

        public ActionResult Acreditar(int id)
        {
            Afiliado afiliado = repAfiliado.Find(id);
            return RedirectToAction("Acreditar", "Afiliado", new { id = afiliado.id });
        }

        [HttpPost]
        public ActionResult Acreditar(int id, string monto)
        {
            ViewModel viewmodel = new ViewModel();
            Afiliado afiliado = repAfiliado.Find(id);
            if (repAfiliado.Acreditar(afiliado, monto))
            {
                viewmodel.Title = "Prepago / Beneficiario / Acreditar";
                viewmodel.Message = "Acreditación exitosa";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = afiliado.companyid.ToString();
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / Acreditar ";
                viewmodel.Message = "Falló el proceso de acreditación";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "FilterBeneficiarios";
                viewmodel.RouteValues = afiliado.companyid.ToString();
            }
            return RedirectToAction("GenericView", viewmodel);
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