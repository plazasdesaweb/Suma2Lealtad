using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class BeneficiarioPrepagoRepository
    {
        private const int ID_ESTATUS_AFILIACION_INICIAL = 0;

        public List<BeneficiarioPrepago> Find(string numdoc, string name, string email, string estadoAfiliacion, string estadoTarjeta)
        {
            List<BeneficiarioPrepago> beneficiarios;
            using (LealtadEntities db = new LealtadEntities())
            {
                if (numdoc == "")
                {
                    numdoc = null;
                }
                if (name == "")
                {
                    name = null;
                }
                if (email == "")
                {
                    email = null;
                }
                if (estadoAfiliacion == "")
                {
                    estadoAfiliacion = null;
                }
                if (estadoTarjeta == "")
                {
                    estadoTarjeta = null;
                }
                //BUSCAR POR ESTADO DE TARJETA
                if (estadoTarjeta != null)
                {
                    beneficiarios = (from a in db.Affiliates
                                     join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                     join s in db.SumaStatuses on a.statusid equals s.id
                                     join t in db.Types on a.typeid equals t.id
                                     join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                     join x in db.PrepaidCustomers on b.prepaidcustomerid equals x.id
                                     join tar in db.TARJETAS on a.id equals tar.NRO_AFILIACION
                                     where tar.ESTATUS_TARJETA.Equals(estadoTarjeta)
                                     select new BeneficiarioPrepago
                                     {
                                         Afiliado = new AfiliadoSuma
                                         {
                                             //ENTIDAD Affiliate 
                                             id = a.id,
                                             docnumber = a.docnumber,
                                             typeid = a.typeid,
                                             //ENTIDAD CLIENTE
                                             name = c.NOMBRE_CLIENTE1,
                                             lastname1 = c.APELLIDO_CLIENTE1,
                                             email = c.E_MAIL,
                                             //ENTIDAD SumaStatuses
                                             estatus = s.name,
                                             //ENTIDAD Type
                                             type = t.name,
                                         },
                                         Cliente = new ClientePrepago
                                         {
                                             idCliente = x.id,
                                             nameCliente = x.name,
                                             aliasCliente = x.alias,
                                             rifCliente = x.rif,
                                             addressCliente = x.address,
                                             phoneCliente = x.phone,
                                             emailCliente = x.email
                                         }
                                     }).OrderBy(n => n.Cliente.nameCliente).OrderBy(x => x.Afiliado.docnumber).ToList();
                }
                else
                {
                    //BUSCAR TODOS
                    if (numdoc == null && name == null && email == null && estadoAfiliacion == null && estadoTarjeta == null)
                    {
                        beneficiarios = (from a in db.Affiliates
                                         join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                         join s in db.SumaStatuses on a.statusid equals s.id
                                         join t in db.Types on a.typeid equals t.id
                                         join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                         join x in db.PrepaidCustomers on b.prepaidcustomerid equals x.id
                                         select new BeneficiarioPrepago
                                         {
                                             Afiliado = new AfiliadoSuma
                                             {
                                                 //ENTIDAD Affiliate 
                                                 id = a.id,
                                                 docnumber = a.docnumber,
                                                 typeid = a.typeid,
                                                 //ENTIDAD CLIENTE
                                                 name = c.NOMBRE_CLIENTE1,
                                                 lastname1 = c.APELLIDO_CLIENTE1,
                                                 email = c.E_MAIL,
                                                 //ENTIDAD SumaStatuses
                                                 estatus = s.name,
                                                 //ENTIDAD Type
                                                 type = t.name
                                             },
                                             Cliente = new ClientePrepago
                                             {
                                                 idCliente = x.id,
                                                 nameCliente = x.name,
                                                 aliasCliente = x.alias,
                                                 rifCliente = x.rif,
                                                 addressCliente = x.address,
                                                 phoneCliente = x.phone,
                                                 emailCliente = x.email
                                             }
                                         }).OrderBy(n => n.Cliente.nameCliente).OrderBy(x => x.Afiliado.docnumber).ToList();
                    }
                    //BUSCAR POR numdoc
                    else if (numdoc != null)
                    {
                        beneficiarios = (from a in db.Affiliates
                                         join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                         join s in db.SumaStatuses on a.statusid equals s.id
                                         join t in db.Types on a.typeid equals t.id
                                         join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                         join x in db.PrepaidCustomers on b.prepaidcustomerid equals x.id
                                         where a.docnumber.Equals(numdoc)
                                         select new BeneficiarioPrepago
                                         {
                                             Afiliado = new AfiliadoSuma
                                             {
                                                 //ENTIDAD Affiliate 
                                                 id = a.id,
                                                 docnumber = a.docnumber,
                                                 typeid = a.typeid,
                                                 //ENTIDAD CLIENTE
                                                 name = c.NOMBRE_CLIENTE1,
                                                 lastname1 = c.APELLIDO_CLIENTE1,
                                                 email = c.E_MAIL,
                                                 //ENTIDAD SumaStatuses
                                                 estatus = s.name,
                                                 //ENTIDAD Type
                                                 type = t.name
                                             },
                                             Cliente = new ClientePrepago
                                             {
                                                 idCliente = x.id,
                                                 nameCliente = x.name,
                                                 aliasCliente = x.alias,
                                                 rifCliente = x.rif,
                                                 addressCliente = x.address,
                                                 phoneCliente = x.phone,
                                                 emailCliente = x.email
                                             }
                                         }).OrderBy(n => n.Cliente.nameCliente).OrderBy(x => x.Afiliado.docnumber).ToList();
                    }
                    //BUSCAR POR name O email O estadoAfiliacion
                    else
                    {
                        beneficiarios = (from a in db.Affiliates
                                         join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                         join s in db.SumaStatuses on a.statusid equals s.id
                                         join t in db.Types on a.typeid equals t.id
                                         join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                         join x in db.PrepaidCustomers on b.prepaidcustomerid equals x.id
                                         where (c.NOMBRE_CLIENTE1.Contains(name) || c.APELLIDO_CLIENTE1.Contains(name) || c.E_MAIL.Equals(email) || s.name.Equals(estadoAfiliacion))
                                         select new BeneficiarioPrepago
                                         {
                                             Afiliado = new AfiliadoSuma
                                             {
                                                 //ENTIDAD Affiliate 
                                                 id = a.id,
                                                 docnumber = a.docnumber,
                                                 typeid = a.typeid,
                                                 //ENTIDAD CLIENTE
                                                 name = c.NOMBRE_CLIENTE1,
                                                 lastname1 = c.APELLIDO_CLIENTE1,
                                                 email = c.E_MAIL,
                                                 //ENTIDAD SumaStatuses
                                                 estatus = s.name,
                                                 //ENTIDAD Type
                                                 type = t.name
                                             },
                                             Cliente = new ClientePrepago
                                             {
                                                 idCliente = x.id,
                                                 nameCliente = x.name,
                                                 aliasCliente = x.alias,
                                                 rifCliente = x.rif,
                                                 addressCliente = x.address,
                                                 phoneCliente = x.phone,
                                                 emailCliente = x.email
                                             }
                                         }).OrderBy(n => n.Cliente.nameCliente).OrderBy(x => x.Afiliado.docnumber).ToList();
                    }
                }
                foreach (var beneficiario in beneficiarios)
                {
                    Decimal p = (from t in db.TARJETAS
                                 where t.NRO_AFILIACION.Equals(beneficiario.Afiliado.id)
                                 select t.NRO_TARJETA
                                 ).SingleOrDefault();
                    if (p != 0)
                    {
                        beneficiario.Afiliado.pan = p.ToString();
                    }
                    else
                    {
                        beneficiario.Afiliado.pan = "";
                    }
                    string e = (from t in db.TARJETAS
                                where t.NRO_AFILIACION.Equals(beneficiario.Afiliado.id)
                                select t.ESTATUS_TARJETA
                                ).SingleOrDefault();
                    if (e != null)
                    {
                        beneficiario.Afiliado.estatustarjeta = e.ToString();
                    }
                    else
                    {
                        beneficiario.Afiliado.estatustarjeta = "";
                    }
                }
            }
            return beneficiarios;
        }

        public BeneficiarioPrepago Find(int id)
        {
            AfiliadoSumaRepository repAfiliado = new AfiliadoSumaRepository();        
            BeneficiarioPrepago beneficiario;
            using (LealtadEntities db = new LealtadEntities())
            {
                beneficiario = (from a in db.Affiliates
                                join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                join s in db.SumaStatuses on a.statusid equals s.id
                                join t in db.Types on a.typeid equals t.id
                                join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                join x in db.PrepaidCustomers on b.prepaidcustomerid equals x.id
                                where a.id == id
                                select new BeneficiarioPrepago
                                {
                                    Afiliado = new AfiliadoSuma
                                    {
                                        //ENTIDAD Affiliate 
                                        id = a.id,
                                        customerid = a.customerid,
                                        docnumber = a.docnumber,
                                        clientid = a.clientid,
                                        storeid = a.storeid,
                                        channelid = a.channelid,
                                        typeid = a.typeid,
                                        typedelivery = a.typedelivery,
                                        storeiddelivery = a.storeiddelivery,
                                        statusid = a.statusid,
                                        reasonsid = a.reasonsid,
                                        twitter_account = a.twitter_account,
                                        facebook_account = a.facebook_account,
                                        instagram_account = a.instagram_account,
                                        comments = a.comments,
                                        //ENTIDAD CLIENTE
                                        nationality = c.NACIONALIDAD,
                                        name = c.NOMBRE_CLIENTE1,
                                        name2 = c.NOMBRE_CLIENTE2,
                                        lastname1 = c.APELLIDO_CLIENTE1,
                                        lastname2 = c.APELLIDO_CLIENTE2,
                                        gender = c.SEXO,
                                        maritalstatus = c.EDO_CIVIL,
                                        occupation = c.OCUPACION,
                                        phone1 = c.TELEFONO_HAB,
                                        phone2 = c.TELEFONO_OFIC,
                                        phone3 = c.TELEFONO_CEL,
                                        email = c.E_MAIL,
                                        cod_estado = c.COD_ESTADO,
                                        cod_ciudad = c.COD_CIUDAD,
                                        cod_municipio = c.COD_MUNICIPIO,
                                        cod_parroquia = c.COD_PARROQUIA,
                                        cod_urbanizacion = c.COD_URBANIZACION,
                                        //ENTIDAD SumaStatuses
                                        estatus = s.name,
                                        //ENTIDAD Type
                                        type = t.name
                                    },
                                    Cliente = new ClientePrepago
                                    {
                                        idCliente = x.id,
                                        nameCliente = x.name,
                                        aliasCliente = x.alias,
                                        rifCliente = x.rif,
                                        addressCliente = x.address,
                                        phoneCliente = x.phone,
                                        emailCliente = x.email
                                    }
                                }).FirstOrDefault();
                if (beneficiario != null)
                {
                    DateTime? d = (from c in db.CLIENTES
                                   where (c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO).Equals(beneficiario.Afiliado.docnumber)
                                   select c.FECHA_NACIMIENTO
                                   ).SingleOrDefault();
                    beneficiario.Afiliado.birthdate = d.Value.ToString("dd-MM-yyyy");
                    //ENTIDAD CustomerInterest
                    beneficiario.Afiliado.Intereses = repAfiliado.chargeInterestList(beneficiario.Afiliado.id);
                    //Llenar las listas de Datos Geográficos.
                    beneficiario.Afiliado.ListaEstados = repAfiliado.GetEstados();
                    beneficiario.Afiliado.ListaCiudades = repAfiliado.GetCiudades(beneficiario.Afiliado.cod_estado);
                    beneficiario.Afiliado.ListaMunicipios = repAfiliado.GetMunicipios(beneficiario.Afiliado.cod_ciudad);
                    beneficiario.Afiliado.ListaParroquias = repAfiliado.GetParroquias(beneficiario.Afiliado.cod_municipio);
                    beneficiario.Afiliado.ListaUrbanizaciones = repAfiliado.GetUrbanizaciones(beneficiario.Afiliado.cod_parroquia);
                    //ENTIDAD TARJETA                    
                    Decimal p = (from t in db.TARJETAS
                                 where t.NRO_AFILIACION.Equals(beneficiario.Afiliado.id)
                                 select t.NRO_TARJETA
                                 ).SingleOrDefault();
                    if (p != 0)
                    {
                        beneficiario.Afiliado.pan = p.ToString();
                    }
                    else
                    {
                        beneficiario.Afiliado.pan = "";
                    }
                    string e = (from t in db.TARJETAS
                                where t.NRO_AFILIACION.Equals(beneficiario.Afiliado.id)
                                select t.ESTATUS_TARJETA
                                ).SingleOrDefault();
                    if (e != null)
                    {
                        beneficiario.Afiliado.estatustarjeta = e.ToString();
                    }
                    else
                    {
                        beneficiario.Afiliado.estatustarjeta = "";
                    }
                }
            }
            return beneficiario;
        }

        public bool Save(BeneficiarioPrepago beneficiario)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                Affiliate afiliado = db.Affiliates.FirstOrDefault(a => a.docnumber == beneficiario.Afiliado.docnumber);
                //ENTIDAD PrepaidBeneficiary
                var prepaidbeneficiary = new PrepaidBeneficiary()
                {
                    affiliateid = afiliado.id,
                    prepaidcustomerid = beneficiario.Cliente.idCliente,
                    begindate = DateTime.Now,
                    active = true
                };
                db.PrepaidBeneficiaries.Add(prepaidbeneficiary);
                db.SaveChanges();
                return true;
            }
        }

        public bool Delete(BeneficiarioPrepago beneficiario)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                //ENTIDAD PrepaidBeneficiary
                PrepaidBeneficiary prepaidbeneficiary = db.PrepaidBeneficiaries.FirstOrDefault(b => b.affiliateid == beneficiario.Afiliado.id && b.prepaidcustomerid == beneficiario.Cliente.idCliente);
                db.PrepaidBeneficiaries.Remove(prepaidbeneficiary);                
                db.SaveChanges();
                return true;
            }
        }       

        public List<PrepaidCustomer> GetClientes()
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                return db.PrepaidCustomers.OrderBy(u => u.name).ToList();
            }
        }
        
    }
}