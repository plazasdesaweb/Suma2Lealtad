using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class BeneficiarioPrepagoRepository
    {
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
                                }).FirstOrDefault();
                if (beneficiario != null)
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

    }
}