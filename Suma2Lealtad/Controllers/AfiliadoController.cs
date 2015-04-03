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

        public ActionResult GenericView()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Find(string numdoc)
        {

            Afiliado afiliado = rep.Find(numdoc);

            if (afiliado == null)
            {
                ViewBag.GenericView = "Registro No Encontrado.";
                return RedirectToAction("GenericView", "Afiliado");
            }

            TempData["AfiliadoModel"] = afiliado;
            return RedirectToAction("Create", "Afiliado");

        }

        public ActionResult Create()
        {
            var model = TempData["AfiliadoModel"] as Afiliado;
            return View(model);
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
                        string path = System.IO.Path.Combine(Server.MapPath(AppModule.GetPathPicture()), System.IO.Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        //ViewBag.Message2 = "Archivo cargado.";
                        //MessageBox.Show("Archivo cargado");
                    }
                    catch (Exception ex)
                    {
                        //ViewBag.Message2 = "ERROR:" + ex.Message.ToString();
                        //MessageBox.Show("ERROR:" + ex.Message.ToString());
                        ViewBag.GenericView = "Ocurrió un error al subir el archivo al servidor," + ex.Message.ToString();
                    }
                else
                {
                    //ViewBag.Message2 = "Debe seleccionar un archivo.";
                    //MessageBox.Show("Debe seleccionar un archivo");

                }
                //PENDIENTE: SI FALLA ALGUNA DE LAS ACTIVIDADES. HAY QUE DESHACER LAS ACTIVIDADES ANTERIORES EXITOSAS.                
                
                ViewBag.GenericView = "Registro creado satisfactoriamente.";
            }

            return RedirectToAction("GenericView", "Afiliado");

        }

        public ActionResult FilterReview()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(string numdoc, string name, string email)
        {
            //para pruebas
            //numdoc = "V-12919906";

            List<Afiliado> afiliado = rep.FindSuma(numdoc, name, email);

            return View(afiliado);

        }

        public ActionResult Index(string numdoc)
        {
            //para pruebas
            //numdoc = "V-12919906";

            List<Afiliado> afiliado = rep.FindSuma(numdoc, "", "");

            return View(afiliado);

        }

        public ActionResult Edit(int id = 0)
        {

            Afiliado afiliado = rep.FindSuma(id);

            if (afiliado == null)
            {
                //return HttpNotFound();
                return RedirectToAction("GenericView", "Afiliado");
            }
            return View(afiliado);

        }

        [HttpPost]
        public ActionResult Edit(Afiliado afiliado)
        {

            if (!rep.SaveChanges(afiliado))
            {
                return RedirectToAction("GenericView", "Afiliado");
                //return RedirectToAction("FilterReview");
            }

            //Aqui debo llamar a los servicios de actualización

            return RedirectToAction("FilterReview");
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

        //public ActionResult SaldosMovimientos(string documento)
        public ActionResult SaldosMovimientos(int id)
        {
            Afiliado afiliado = rep.FindSuma(id);

            SaldosMovimientos SaldosMovimientos = new SaldosMovimientos();

            SaldosMovimientos.DocId = afiliado.docnumber;

            //para pruebas
            //SaldosMovimientos.DocId = "V-6960635";

            string nrodocumento = SaldosMovimientos.DocId.Substring(2);
            string saldosJson = WSL.Cards.getBalance(nrodocumento);
            SaldosMovimientos.Saldos = (IEnumerable<Saldo>)JsonConvert.DeserializeObject<IEnumerable<Saldo>>(saldosJson);
            string movimientosPrepagoJson = WSL.Cards.getBatch(SaldosMovimientos.Saldos.First().accounttype, nrodocumento);
            string movimientosLealtadJson = WSL.Cards.getBatch(SaldosMovimientos.Saldos.Skip(1).First().accounttype, nrodocumento);
            SaldosMovimientos.MovimientosPrepago = (IEnumerable<Movimiento>)JsonConvert.DeserializeObject<IEnumerable<Movimiento>>(movimientosPrepagoJson);
            SaldosMovimientos.MovimientosSuma = (IEnumerable<Movimiento>)JsonConvert.DeserializeObject<IEnumerable<Movimiento>>(movimientosLealtadJson);

            return View(SaldosMovimientos);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }

}