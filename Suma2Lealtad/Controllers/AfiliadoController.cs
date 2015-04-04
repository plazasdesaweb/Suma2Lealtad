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

            //Sa cambia el metodo de busuqeda para pruebas
            //Afiliado afiliadoparcial = rep.FindSuma(numdoc, "", "").FirstOrDefault();
            //Afiliado afiliado = rep.FindSuma(afiliadoparcial.id);
                                    
            Afiliado afiliado = rep.Find(numdoc);
            
            if (afiliado == null)
            {
                //pendiente
                return RedirectToAction("GenericView");
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
                List<Afiliado> afiliados = new List<Afiliado> { afiliado };                
                return View("Index", afiliados);
            }
            else
            {
                //pendiente
                return RedirectToAction("GenericView");
            }
        }

        public ActionResult GenericView()
        {
            return View();
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

            List<Afiliado> afiliado = rep.FindSuma(numdoc,"","");

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