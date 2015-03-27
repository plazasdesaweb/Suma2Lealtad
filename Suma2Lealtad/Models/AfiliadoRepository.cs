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

        // buscar el registro en el Modelo PlazasWeb.
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

                // nuevo afiliado : cuando el afiliado está registrado en PlazasWeb pero no está registrado en SumaLealtad.
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
                        FECHA_NACIMIENTO = System.DateTime.Now,       //PENDIENTE : record.birthdate,
                        SEXO = AfiliadoSuma.gender,
                        EDO_CIVIL = AfiliadoSuma.maritalstatus,
                        OCUPACION = AfiliadoSuma.occupation,
                        TELEFONO_HAB = AfiliadoSuma.phone1,
                        TELEFONO_OFIC = AfiliadoSuma.phone2,
                        TELEFONO_CEL = AfiliadoSuma.phone3,
                        E_MAIL = AfiliadoSuma.email,
                        COD_SUCURSAL = AfiliadoSuma.storeiddelivery,
                        COD_ESTADO = AfiliadoSuma.cod_estado ,
                        COD_CIUDAD = AfiliadoSuma.cod_ciudad,
                        COD_MUNICIPIO = AfiliadoSuma.cod_municipio,
                        COD_PARROQUIA = AfiliadoSuma.cod_parroquia,
                        COD_URBANIZACION = AfiliadoSuma.cod_urbanizacion,
                        FECHA_CREACION = System.DateTime.Now
                    };

                    var _affiliate = new Affiliate()
                    {
                        id = AfilliatesID(),                       
                        customerid = Int32.Parse(AfiliadoSuma.id),
                        docnumber = AfiliadoSuma.docnumber,
                        clientid = Int32.Parse(AfiliadoSuma.id),
                        storeid = AfiliadoSuma.storeiddelivery,
                        channelid = 1,
                        typeid = 1,
                        affiliatedate = System.DateTime.Now,
                        typedelivery = "",
                        storeiddelivery = AfiliadoSuma.storeiddelivery,
                        estimateddatedelivery = System.DateTime.Now,
                        creationdate = System.DateTime.Now,
                        creationuserid = (int) HttpContext.Current.Session["userid"],
                        modifieduserid = (int) HttpContext.Current.Session["userid"],
                        modifieddate = System.DateTime.Now,
                        statusid = 1,
                        reasonsid = 1,
                        twitter_account = AfiliadoSuma.twitter_account,
                        facebook_account = AfiliadoSuma.facebook_account,
                        instagram_account = AfiliadoSuma.instagram_account,
                        comments = AfiliadoSuma.comments
                    };

                    var _companyaff = new CompanyAffiliate()
                    {
                        affiliateid = _affiliate.id,
                        companyid = 1,
                        begindate = System.DateTime.Now,
                        enddate = System.DateTime.Now,
                        comments = "",
                        active = true
                    };

                    db.CLIENTES.Add(_cliente);

                    db.Affiliates.Add(_affiliate);

                    db.CompanyAffiliates.Add(_companyaff);

                    foreach (var interes in AfiliadoSuma.Intereses.Where(x => x.Checked == true))
                    {

                        CustomerInterest customerInterest = new CustomerInterest(){ 
                            customerid = _affiliate.customerid, 
                            interestid = interes.id,
                            comments = ""
                        };

                        db.CustomerInterests.Add(customerInterest);

                    }

                    var affiliateAuditoria = new AffiliateAud()
                    {
                        id = AfilliateAudID(),
                        affiliateid = _affiliate.id,
                        modifieduserid = (int) HttpContext.Current.Session["userid"],
                        modifieddate = System.DateTime.Now,
                        statusid = 1,
                        reasonsid = 1,
                        comments = ""
                    };

                    db.AffiliateAuds.Add( affiliateAuditoria );

                    db.SaveChanges();

                }

            }


        }

        private int AfilliatesID()
        {
    
            using (LealtadEntities db = new LealtadEntities())
            {
                if (db.Affiliates.Count() == 0)
                    return 1;
                return (db.Affiliates.Max( a => a.id ) + 1);
            }

        }

        private int AfilliateAudID()
        {

            using (LealtadEntities db = new LealtadEntities())
            {
                if (db.AffiliateAuds.Count() == 0)
                    return 1;
                return (db.AffiliateAuds.Max(a => a.id) + 1);
            }

        } 

    }


}