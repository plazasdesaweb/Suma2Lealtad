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
        private ClientePrepagoRepository rep = new ClientePrepagoRepository();

        public ActionResult Filter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Filter(string rif)
        {
            List<ClientePrepago> clientes = rep.Find("", rif);
            if (clientes.Count != 0)
            {
                return View("Index", clientes);
            }
            else
            {
                return View("Create", clientes.FirstOrDefault());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientePrepago cliente)
        {
            ViewModel viewmodel = new ViewModel();
            if (rep.Save(cliente))
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
                viewmodel.ActionName = "Index";
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
            List<ClientePrepago> clientes = rep.Find(name, rif);
            return View("Index", clientes);
        }

        public ActionResult Index(string rif)
        {
            List<ClientePrepago> clientes = rep.Find("",rif);
            return View(clientes);
        }

        public ActionResult IndexAll()
        {
            List<ClientePrepago> clientes = rep.Find("", "");
            return View("Index", clientes);
        }

        //Quede Aqui. La idea es mostrar el filtro como pantalla incial, con 5 campos(numdoc, name, email, estadoAfiliacion, estadoTarjeta) y boton de buscar todos
        public ActionResult FilterBeneficiarios(int id)
        {
            ClientePrepago cliente = rep.Find(id);
            return View(cliente);
        }

        [HttpPost]
        public ActionResult FilterBeneficiarios(int id, string numdoc, string name, string email, string estadoAfiliacion, string estadoTarjeta)
        {
            //List<BeneficiarioPrepago> clientes = rep.FindBeneficiarios(id, numdoc, name, email, estadoAfiliacion, estadoTarjeta);
            //return View("Beneficiarios", clientes);
            return View();
        }

        public ActionResult Edit(int id)
        {
            ClientePrepago cliente = rep.Find(id);
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientePrepago cliente)
        {
            ViewModel viewmodel = new ViewModel();
            if (!rep.SaveChanges(cliente))
            {
                viewmodel.Title = "Prepago / Cliente / Editar Cliente";
                viewmodel.Message = "Existen campos que son requeridos para procesar el formulario.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "Index";
            }
            else
            {
                viewmodel.Title = "Prepago / Cliente / Editar Cliente";
                viewmodel.Message = "La información del Cliente ha sido actualizada satisfactoriamente.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "Index";
                viewmodel.RouteValues = cliente.rifCliente;
            }
            return RedirectToAction("GenericView", viewmodel);
        }

        public ActionResult Delete(int id)
        {
            ViewModel viewmodel = new ViewModel();
            ClientePrepago cliente = rep.Find(id);
            //List<BeneficiarioPrepago> beneficiarios = new List<BeneficiarioPrepago>(); //repBeneficiario.Find(id);
            //if (beneficiarios.Count != 0)
            //{
            //    viewmodel.Title = "Prepapago / Cliente / Eliminar";
            //    viewmodel.Message = "No se puede eliminar este Cliente, tiene Beneficiarios asociados.";
            //    viewmodel.ControllerName = "ClientePrepago";
            //    viewmodel.ActionName = "Index";
            //    viewmodel.RouteValues = cliente.rifCliente;
            //    return RedirectToAction("GenericView", viewmodel);
            //}
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ClientePrepago cliente)
        {
            ViewModel viewmodel = new ViewModel();
            if (rep.BorrarCliente(cliente.idCliente))
            {
                viewmodel.Title = "Prepago / Cliente / Borrar Cliente";
                viewmodel.Message = "Cliente borradao exitosamente.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "Index";
            }
            else
            {
                viewmodel.Title = "Prepago / Compañia / Borrar Compañia";
                viewmodel.Message = "Error de aplicacion: No se pudo borrar Compañia.";
                viewmodel.ControllerName = "ClientePrepago";
                viewmodel.ActionName = "Index";
                viewmodel.RouteValues = cliente.rifCliente;
            }
            return RedirectToAction("GenericView", viewmodel);
        }
        
    }
}
