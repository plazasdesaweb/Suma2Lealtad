using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Suma2Lealtad.Models;
using Suma2Lealtad.Modules;
using Newtonsoft.Json;

namespace Suma2Lealtad.Controllers
{
    public class AfiliadoController : Controller
    {
        private SumaEntities db = new SumaEntities();

        public ActionResult Filter()
        {
            return View();
        }

        //
        // GET: /Search/

        public ActionResult Search(string numdoc)
        {

            string resp = WSL.PlazasWeb("getclientbynumdoc/" + numdoc);
            var Model = JsonConvert.DeserializeObject<AfiliadoWeb>(resp);

            TempData["MiAfiliado"] = Model;
            return RedirectToAction("Index", "Afiliado");
        }

        public ActionResult Index()
        {
            return View(TempData["MiAfiliado"]);
        }

        [HttpPost]
        public ActionResult Index( AfiliadoWeb Afiliado )
        {
            if (ModelState.IsValid)
            {

                using (SumaEntities context = new SumaEntities())
                {

                    var keyword = Afiliado.docnumber;

                    var result = context.Customers.SingleOrDefault(c => c.docnumber == keyword);

                    // llenar la estructura del cliente.
                    var afiliado = new Customer()
                    {
                        id = int.Parse(Afiliado.id),
                        email = Afiliado.email,
                        type = 1,//byte.Parse(Afiliado.type + ""),
                        docnumber = Afiliado.docnumber,
                        name = Afiliado.name,
                        phone1 = Afiliado.phone1,
                        phone2 = Afiliado.phone2,
                        //password = Afiliado.clave,
                        //questionid = Afiliado.questionid,
                        //answer = Afiliado.answer
                    };

                    // evaluar si existe el registro en el modelo de SUMA.
                    if (result == null)
                    {
                        context.Customers.Add(afiliado);
                        context.SaveChanges();
                    }
                    else
                    {
                        //context.Entry(afiliado).State = EntityState.Modified;
                        //context.SaveChanges();
                    }

                    // actualizar el registro de cliente en el modelo de PlazasWeb.
                    string wsl = "updclient/{id}/{type}/{docnumber}/{name}/{phone1}/{phone2}";
                    //string wsl = "updclient/{id}/{type}/{docnumber}/{email}/{name}/{name2}/{lastname1}/{lastname2}/{phone1}/{phone2}/{birthdate}/{maritalstatus}/{gender}";

                    wsl = wsl.Replace( "{id}"        , Afiliado.id          );
                    wsl = wsl.Replace( "{type}"      , Afiliado.type + ""   );
                    wsl = wsl.Replace( "{docnumber}" , Afiliado.docnumber   );
                    wsl = wsl.Replace( "{email}"     , Afiliado.email       );
                    wsl = wsl.Replace( "{name}"      , Afiliado.name        );
                    wsl = wsl.Replace( "{name2}"     , Afiliado.name2 + ""  );
                    wsl = wsl.Replace( "{lastname1}" , Afiliado.lastname1   );
                    wsl = wsl.Replace("{lastname2}", Afiliado.lastname2 + "");
                    wsl = wsl.Replace( "{phone1}"    , Afiliado.phone1      );
                    wsl = wsl.Replace( "{phone2}"    , Afiliado.phone2      );
                    wsl = wsl.Replace( "{birthdate}" , Afiliado.birthdate + "" );
                    wsl = wsl.Replace( "{maritalstatus}", Afiliado.maritalstatus + "");
                    wsl = wsl.Replace( "{gender}"    , Afiliado.gender + "");
                      
                    string resp = WSL.PlazasWeb(wsl); 

                }

                // PENDIENTE : Colocar la vista que informe al usuario, que el cliente no existe en PlazasWeb, y continuar con el flujo.
                //return RedirectToAction("Filter");
            }

            return RedirectToAction("Filter");

        }

        //
        // GET: /Afiliado/Details/5

        public ActionResult Details(int id = 0)
        {
            Afiliacion afiliacion = db.Afiliaciones.Find(id);
            if (afiliacion == null)
            {
                return HttpNotFound();
            }
            return View(afiliacion);
        }

        //
        // GET: /Afiliado/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Afiliado/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Afiliacion afiliacion)
        {
            if (ModelState.IsValid)
            {
                db.Afiliaciones.Add(afiliacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(afiliacion);
        }

        //
        // GET: /Afiliado/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Afiliacion afiliacion = db.Afiliaciones.Find(id);
            if (afiliacion == null)
            {
                return HttpNotFound();
            }
            return View(afiliacion);
        }

        //
        // POST: /Afiliado/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Afiliacion afiliacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(afiliacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(afiliacion);
        }

        //
        // GET: /Afiliado/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Afiliacion afiliacion = db.Afiliaciones.Find(id);
            if (afiliacion == null)
            {
                return HttpNotFound();
            }
            return View(afiliacion);
        }

        //
        // POST: /Afiliado/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Afiliacion afiliacion = db.Afiliaciones.Find(id);
            db.Afiliaciones.Remove(afiliacion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}