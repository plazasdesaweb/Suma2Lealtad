using Suma2Lealtad.Filters;
using Suma2Lealtad.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            return View("Index",compañias);
        }

        public ActionResult Details(int id = 0)
        {
            Company company = rep.Find(id,"Company");
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

        public ActionResult Edit(int id = 0)
        {
            Company company = rep.Find(id, "Company");
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
        
        public ActionResult Delete(int id = 0)
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
            
        public ActionResult FilterBeneficiarios(int id)
        {
            PrepagoCompanyAffiliattes compañiaBeneficiarios = rep.Find(id);
            return View(compañiaBeneficiarios);
        }

        public ActionResult Beneficiarios(int id)
        {
            PrepagoCompanyAffiliattes compañiaBeneficiarios = rep.Find(id);
            return View(compañiaBeneficiarios);
        }
        
        [HttpPost]
        public ActionResult Beneficiarios(int id, string numdoc, string name, string email)
        {
            PrepagoCompanyAffiliattes compañiaBeneficiarios = rep.FindBeneficiarios(id, numdoc, name, email);
            return View(compañiaBeneficiarios);
        }

        [HttpPost]
        public ActionResult CargarBeneficiarios(int id)
        {
            PrepagoCompanyAffiliattes compañiaBeneficiarios = rep.Find(id);
            return View("Beneficiarios",compañiaBeneficiarios);
        }

        [HttpPost]
        public ActionResult EditBeneficiario(Afiliado afiliado)
        {
            string id = rep.BuscarCompañia(afiliado).ToString();
            ViewModel viewmodel = new ViewModel();
            if (!repAfiliado.SaveChanges(afiliado))
            {
                viewmodel.Title = "Prepago / Beneficiario / Revisar Afiliación";
                viewmodel.Message = "Existen campos que son requeridos para procesar el formulario.";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "CargarBeneficiarios";
                viewmodel.RouteValues = id;
            }
            else
            {
                viewmodel.Title = "Prepago / Beneficiario / Revisar Afiliación";
                viewmodel.Message = "La información del beneficiario ha sido actualizada satisfactoriamente.";
                viewmodel.ControllerName = "CompanyPrepago";
                viewmodel.ActionName = "CargarBeneficiarios";
                viewmodel.RouteValues = id;
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