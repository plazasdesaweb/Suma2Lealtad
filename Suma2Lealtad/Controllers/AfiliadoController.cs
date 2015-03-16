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
using System.Windows.Forms;

namespace Suma2Lealtad.Controllers
{
    public class AfiliadoController : Controller
    {

        private LealtadEntities db = new LealtadEntities();

        public ActionResult Filter()
        {
            return View();
        }

        //
        // GET: /Search/
        public ActionResult Search(string numdoc)
        {
            var Model = JsonConvert.DeserializeObject<AfiliadoSuma>(WSL.PlazasWeb.getClientByNumDoc(numdoc));

            TempData["MiAfiliado"] = Model;

            return RedirectToAction("Index", "Afiliado");

        }

        public ActionResult Index()
        {
            return View(TempData["MiAfiliado"]);
        }

        [HttpPost]
        public ActionResult Index(AfiliadoSuma record, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                using (LealtadEntities context = new LealtadEntities())
                {

                    var keyword = record.docnumber;

                    var result = context.CLIENTES.SingleOrDefault(c => c.NRO_DOCUMENTO == keyword);

                    var _clienteSuma = new CLIENTE()
                    {
                        TIPO_DOCUMENTO = record.type,
                        NRO_DOCUMENTO = record.docnumber,
                        NACIONALIDAD = record.nationality,
                        NOMBRE_CLIENTE1 = record.name,
                        NOMBRE_CLIENTE2 = record.name2,
                        APELLIDO_CLIENTE1 = record.lastname1,
                        APELLIDO_CLIENTE2 = record.lastname2,
                        FECHA_NACIMIENTO = System.DateTime.Now,       //record.birthdate,
                        SEXO = record.gender,
                        EDO_CIVIL = record.maritalstatus,
                        OCUPACION = record.occupation,
                        TELEFONO_HAB = record.phone1,
                        TELEFONO_OFIC = record.phone2,
                        TELEFONO_CEL = record.phone3,
                        E_MAIL = record.email,
                        COD_SUCURSAL = 1,
                        COD_ESTADO = null,
                        COD_CIUDAD = null,
                        COD_MUNICIPIO = null,
                        COD_PARROQUIA = null,
                        COD_URBANIZACION = null,
                        FECHA_CREACION = System.DateTime.Now
                    };

                    var _affiliateSuma = new Affiliate()
                    {
                        id = 1,                                //context.Affiliates.Max( k => k.id) + 1,
                        customerid = Int32.Parse(record.id),
                        docnumber = record.docnumber,
                        clientid = Int32.Parse(record.id),
                        storeid = 1,
                        channelid = 0,
                        typeid = 0,
                        affiliatedate = System.DateTime.Now,
                        typedelivery = "",
                        storeiddelivery = 1,
                        estimateddatedelivery = System.DateTime.Now,
                        creationdate = System.DateTime.Now,
                        creationuserid = 2,
                        modifieduserid = 2,
                        modifieddate = System.DateTime.Now,
                        statusid = 0,
                        reasonsid = 9,
                        comments = null
                    };

                    // evaluar si existe el registro en el modelo de SUMA.
                    if (result == null)
                    {
                        context.CLIENTES.Add(_clienteSuma);

                        context.Affiliates.Add(_affiliateSuma);

                        context.SaveChanges();
                    }
                    else
                    {
                        //context.Entry(afiliado).State = EntityState.Modified;
                        //context.SaveChanges();
                    }

                    // actualizar el registro de cliente en el modelo de PlazasWeb.
                    string resp = WSL.PlazasWeb.UpdateClient(record);

                    //aqui metemos el código para subir la imagen al server
                    if (file != null && file.ContentLength > 0)
                        try
                        {
                            string path = System.IO.Path.Combine(Server.MapPath("~/Fotos"), System.IO.Path.GetFileName(file.FileName));
                            file.SaveAs(path);
                            //ViewBag.Message2 = "Archivo cargado.";
                            //MessageBox.Show("Archivo cargado");
                        }
                        catch (Exception ex)
                        {
                            //ViewBag.Message2 = "ERROR:" + ex.Message.ToString();
                            MessageBox.Show("ERROR: No se pudo subir el archivo seleccionado (" + ex.Message.ToString() + ")");
                        }
                    else
                    {
                        //ViewBag.Message2 = "Debe seleccionar un archivo.";
                        //MessageBox.Show("Debe seleccionar un archivo");
                    }

                }

                //PENDIENTE: SI FALLA ALGUNA DE LAS ACTIVIDADES. HAY QUE DESHACER LAS ACTIVIDADES ANTERIORES EXITOSAS.                

                // PENDIENTE : Colocar la vista que informe al usuario, que el cliente no existe en PlazasWeb, y continuar con el flujo.
                //return RedirectToAction("Filter");
            }

            return RedirectToAction("Filter");

        }
    }
}