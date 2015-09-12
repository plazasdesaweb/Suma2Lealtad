using Suma2Lealtad.Models;
using Suma2Lealtad.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers.Prepago
{
    public class FueraLineaPrepagoController : Controller
    {
        BeneficiarioPrepagoRepository repBeneficiario = new BeneficiarioPrepagoRepository();

        //
        // GET: /FueraLineaPrepago/Filter/

        public ActionResult Filter()
        {
            return View();
        }

        //
        // POST: /FueraLineaPrepago/Filter/

        [HttpPost]
        public ActionResult Filter( string numdoc, string name )
        {

            List<BeneficiarioPrepago> beneficiarios = repBeneficiario.Find(numdoc, name, "", "", "").OrderBy(x => x.Cliente.nameCliente).ThenBy(y => y.Afiliado.docnumber).ToList();

            return View("Table", beneficiarios);   
        }

        //
        // GET: /FueraLineaPrepago/Table/

        public ActionResult Table()
        {
            return View();
        }

        //
        // GET: /FueraLineaPrepago/AddNew/1

        public ActionResult AddNew(int id)
        {

            BeneficiarioPrepago record = repBeneficiario.Find(id);

            BeneficiarioPrepagoViewModel model = new BeneficiarioPrepagoViewModel() { numdoc = record.Afiliado.docnumber, beneficiario = record.Afiliado.name + " " + record.Afiliado.lastname1, monto = "0,00" };

            return View( model );
        
        }

        //
        // POST: /FueraLineaPrepago/AddNew

        [HttpPost]
        public ActionResult AddNew( BeneficiarioPrepagoViewModel model )
        {

            if (ModelState.IsValid)
            {

                if (decimal.Parse(model.monto) <= 0)
                {
                    ModelState.AddModelError("Monto", "El Monto de Transacción debe ser superior a cero.");
                }
                else if ( model.monto.IndexOf(",") == -1 )
                {
                    ModelState.AddModelError("Monto", "El Monto de Transacción debe contener coma (,) como símbolo separador decimal.");
                }
                else {

                    ViewModel viewmodel = new ViewModel();
                    viewmodel.Title = "Prepago / Fuera de Línea / Crear Transacción de Compra";
                    viewmodel.Message = "La Transacción ha sido efectuada satisfactoriamente.";
                    viewmodel.ControllerName = "FueraLineaPrepago";
                    viewmodel.ActionName = "Filter";

                    if (! repBeneficiario.CompraFueraLinea(model.documento, model.montotrx))
                        viewmodel.Message = "La Transacción no pudo ser efectuada. Revise los estatus de la Tarjeta o Cuenta e intente de nuevo.";

                    return RedirectToAction("GenericView", viewmodel);

                }

            }

            return View();

        }

        public ActionResult GenericView(ViewModel viewmodel)
        {
            return View(viewmodel);
        }

    }
}
