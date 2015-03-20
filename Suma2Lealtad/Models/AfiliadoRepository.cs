using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Suma2Lealtad.Modules;

namespace Suma2Lealtad.Models
{
    public class AfiliadoRepository
    {

        public AfiliadoRepository() { }

        public Afiliado Model { get; set; }

        // buscar el registro se encuentra almacenado en el Modelo PlazasWeb.
        public bool IsRecordPlazasWeb(string numdoc)
        {

            Model = JsonConvert.DeserializeObject<Afiliado>(WSL.PlazasWeb.getClientByNumDoc(numdoc));

            Model.Intereses = chargeInterestList();

            return Model.exnumber.Equals("0");

        }


        private List<Interest> chargeInterestList()
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                return db.Interests.Where(x => x.active == true).ToList();
            }
        }


        // guardar el registro en el Modelo SumaLealtad.
        public void Save(Afiliado AfiliadoSuma)
        {

            using (LealtadEntities db = new LealtadEntities())
            {

                var result = db.CLIENTES.SingleOrDefault(c => c.NRO_DOCUMENTO == AfiliadoSuma.docnumber);

                // nuevo afiliado : registrado en PlazasWeb, no registrado en SumaLealtad.
                if (result == null)
                {

                    var _cliente = new CLIENTE()
                    {
                        TIPO_DOCUMENTO = AfiliadoSuma.type,
                        NRO_DOCUMENTO = AfiliadoSuma.docnumber,
                        NACIONALIDAD = AfiliadoSuma.nationality,
                        NOMBRE_CLIENTE1 = AfiliadoSuma.name,
                        NOMBRE_CLIENTE2 = AfiliadoSuma.name2,
                        APELLIDO_CLIENTE1 = AfiliadoSuma.lastname1,
                        APELLIDO_CLIENTE2 = AfiliadoSuma.lastname2,
                        FECHA_NACIMIENTO = System.DateTime.Now,       //record.birthdate,
                        SEXO = AfiliadoSuma.gender,
                        EDO_CIVIL = AfiliadoSuma.maritalstatus,
                        OCUPACION = AfiliadoSuma.occupation,
                        TELEFONO_HAB = AfiliadoSuma.phone1,
                        TELEFONO_OFIC = AfiliadoSuma.phone2,
                        TELEFONO_CEL = AfiliadoSuma.phone3,
                        E_MAIL = AfiliadoSuma.email,
                        COD_SUCURSAL = 1,
                        COD_ESTADO = null,
                        COD_CIUDAD = null,
                        COD_MUNICIPIO = null,
                        COD_PARROQUIA = null,
                        COD_URBANIZACION = null,
                        FECHA_CREACION = System.DateTime.Now
                    };

                    var _affiliate = new Affiliate()
                    {
                        id = 100,                       //getAffiliateID(),
                        customerid = Int32.Parse(AfiliadoSuma.id),
                        docnumber = AfiliadoSuma.docnumber,
                        clientid = Int32.Parse(AfiliadoSuma.id),
                        storeid = 1,
                        channelid = 1,
                        typeid = 1,
                        affiliatedate = System.DateTime.Now,
                        typedelivery = "",
                        storeiddelivery = 1,
                        estimateddatedelivery = System.DateTime.Now,
                        creationdate = System.DateTime.Now,
                        creationuserid = (int)HttpContext.Current.Session["userid"],
                        modifieduserid = (int)HttpContext.Current.Session["userid"],
                        modifieddate = System.DateTime.Now,
                        statusid = 1,
                        reasonsid = 1,
                        comments = AfiliadoSuma.comments
                    };

                    db.CLIENTES.Add(_cliente);

                    db.Affiliates.Add(_affiliate);

                    db.SaveChanges();

                }

            }

        }
    }
}