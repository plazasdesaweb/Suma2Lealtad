using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class ClientePrepagoRepository
    {
        private int ClientePrepagoID()
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                if (db.PrepaidCustomers.Count() == 0)
                    return 1;
                return (db.PrepaidCustomers.Max(c => c.id) + 1);
            }
        }

        public List<ClientePrepago> Find(string name, string rif)
        {
            List<ClientePrepago> clientes;
            using (LealtadEntities db = new LealtadEntities())
            {
                if (name == "")
                {
                    name = null;
                }
                if (rif == "")
                {
                    rif = null;
                }
                if (name == null && rif == null)
                {
                    clientes = (from c in db.PrepaidCustomers
                                select new ClientePrepago()
                                {
                                    idCliente = c.id,
                                    nameCliente = c.name,
                                    aliasCliente = c.alias,
                                    rifCliente = c.rif,
                                    phoneCliente = c.phone,
                                    emailCliente = c.email
                                }).ToList();
                }
                else
                {
                    clientes = (from c in db.PrepaidCustomers
                                where c.rif.Equals(rif) || c.name.Contains(name)
                                select new ClientePrepago()
                                {
                                    idCliente = c.id,
                                    nameCliente = c.name,
                                    aliasCliente = c.alias,
                                    rifCliente = c.rif,
                                    phoneCliente = c.phone,
                                    emailCliente = c.email
                                }).ToList();
                }
            }
            return clientes;
        }

        public ClientePrepago Find(int id)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                ClientePrepago cliente;
                cliente = (from c in db.PrepaidCustomers
                           where c.id.Equals(id)
                           select new ClientePrepago()
                           {
                               idCliente = c.id,
                               nameCliente = c.name,
                               aliasCliente = c.alias,
                               rifCliente = c.rif,
                               addressCliente = c.address,
                               phoneCliente = c.phone,
                               emailCliente = c.email
                           }).FirstOrDefault();
                return cliente;
            }
        }

        public bool Save(ClientePrepago cliente)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                var Cliente = new PrepaidCustomer()
                {
                    id = ClientePrepagoID(),
                    name = cliente.nameCliente,
                    phone = cliente.phoneCliente,
                    rif = cliente.rifCliente,
                    alias = cliente.aliasCliente,
                    address = cliente.addressCliente,
                    email = cliente.emailCliente,
                    creationdate = DateTime.Now,
                    userid = (int)HttpContext.Current.Session["userid"]
                };
                db.PrepaidCustomers.Add(Cliente);
                db.SaveChanges();
                return true;
            }
        }

        public bool SaveChanges(ClientePrepago cliente)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                PrepaidCustomer Cliente = db.PrepaidCustomers.FirstOrDefault(c => c.id == cliente.idCliente);
                if (Cliente != null)
                {
                    Cliente.name = cliente.nameCliente;
                    Cliente.phone = cliente.phoneCliente;
                    Cliente.rif = cliente.rifCliente;
                    Cliente.alias = cliente.aliasCliente;
                    Cliente.address = cliente.addressCliente;
                    Cliente.email = cliente.emailCliente;
                }
                db.SaveChanges();
                return true;
            }
        }

        public bool BorrarCliente(int id)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                PrepaidCustomer Cliente = db.PrepaidCustomers.FirstOrDefault(c => c.id == id);
                if (Cliente != null)
                {
                    db.PrepaidCustomers.Remove(Cliente);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public ClientePrepago FindBeneficiarios(int id, string numdoc, string name, string email, string estadoAfiliacion, string estadoTarjeta)
        {
            ClientePrepago cliente;
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
                cliente = Find(id);
                if (estadoTarjeta != null)
                {
                    cliente.beneficiarios = (from a in db.Affiliates
                                             join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                             join s in db.SumaStatuses on a.statusid equals s.id
                                             join t in db.Types on a.typeid equals t.id
                                             join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                             where b.beneficiaryid.Equals(id) && (c.NOMBRE_CLIENTE1.Contains(name) || c.APELLIDO_CLIENTE1.Contains(name) || c.E_MAIL == email || t.name == estadoAfiliacion)
                                             select new AfiliadoSuma()
                                             {
                                                 //ENTIDAD Affiliate 
                                                 id = a.id,
                                                 docnumber = a.docnumber,
                                                 typeid = a.typeid,
                                                 //ENTIDAD CLIENTE
                                                 name = c.NOMBRE_CLIENTE1,
                                                 lastname1 = c.APELLIDO_CLIENTE1,
                                                 email = c.E_MAIL,
                                                 //ENTIDAD Status
                                                 estatus = s.name,
                                                 //ENTIDAD Type
                                                 type = t.name
                                             }).ToList();
                    if (cliente.beneficiarios != null)
                    {
                        foreach (var beneficiario in cliente.beneficiarios)
                        {
                            Decimal p = (from t in db.TARJETAS
                                         where t.NRO_AFILIACION.Equals(beneficiario.id) && t.ESTATUS_TARJETA.Equals(estadoTarjeta)
                                         select t.NRO_TARJETA
                                         ).SingleOrDefault();
                            if (p != 0)
                            {
                                beneficiario.pan = p.ToString();
                            }
                            else
                            {
                                beneficiario.pan = "";
                            }
                            string e = (from t in db.TARJETAS
                                        where t.NRO_AFILIACION.Equals(beneficiario.id)
                                        select t.ESTATUS_TARJETA
                                        ).SingleOrDefault();
                            if (e != null)
                            {
                                beneficiario.estatustarjeta = e.ToString();
                            }
                            else
                            {
                                beneficiario.estatustarjeta = "";
                            }
                        }
                    }
                    return cliente;
                }
                else if (numdoc == null && name == null && email == null && estadoAfiliacion == null && estadoTarjeta == null)
                {
                    cliente.beneficiarios = (from a in db.Affiliates
                                             join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                             join s in db.SumaStatuses on a.statusid equals s.id
                                             join t in db.Types on a.typeid equals t.id
                                             join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                             where b.beneficiaryid.Equals(id)
                                             select new AfiliadoSuma()
                                             {
                                                 //ENTIDAD Affiliate 
                                                 id = a.id,
                                                 docnumber = a.docnumber,
                                                 //ENTIDAD CLIENTE
                                                 name = c.NOMBRE_CLIENTE1,
                                                 lastname1 = c.APELLIDO_CLIENTE1,
                                                 email = c.E_MAIL,
                                                 //ENTIDAD Status
                                                 estatus = s.name,
                                                 //ENTIDAD Type
                                                 type = t.name
                                             }).ToList();                    
                }
                else if (numdoc != null)
                {
                    cliente.beneficiarios = (from a in db.Affiliates
                                             join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                             join s in db.SumaStatuses on a.statusid equals s.id
                                             join t in db.Types on a.typeid equals t.id
                                             join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                             where b.beneficiaryid.Equals(id) && a.docnumber.Equals(numdoc)
                                             select new AfiliadoSuma()
                                             {
                                                 //ENTIDAD Affiliate 
                                                 id = a.id,
                                                 docnumber = a.docnumber,
                                                 typeid = a.typeid,
                                                 //ENTIDAD CLIENTE
                                                 name = c.NOMBRE_CLIENTE1,
                                                 lastname1 = c.APELLIDO_CLIENTE1,
                                                 email = c.E_MAIL,
                                                 //ENTIDAD Status
                                                 estatus = s.name,
                                                 //ENTIDAD Type
                                                 type = t.name
                                             }).ToList();
                }       
                else 
                {
                    cliente.beneficiarios = (from a in db.Affiliates
                                             join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                             join s in db.SumaStatuses on a.statusid equals s.id
                                             join t in db.Types on a.typeid equals t.id
                                             join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                             where b.beneficiaryid.Equals(id) && (c.NOMBRE_CLIENTE1.Contains(name) || c.APELLIDO_CLIENTE1.Contains(name) || c.E_MAIL == email || t.name == estadoAfiliacion)
                                             select new AfiliadoSuma()
                                             {
                                                 //ENTIDAD Affiliate 
                                                 id = a.id,
                                                 docnumber = a.docnumber,
                                                 typeid = a.typeid,
                                                 //ENTIDAD CLIENTE
                                                 name = c.NOMBRE_CLIENTE1,
                                                 lastname1 = c.APELLIDO_CLIENTE1,
                                                 email = c.E_MAIL,
                                                 //ENTIDAD Status
                                                 estatus = s.name,
                                                 //ENTIDAD Type
                                                 type = t.name
                                             }).ToList();
                }
                if (cliente.beneficiarios != null)
                {
                    foreach (var beneficiario in cliente.beneficiarios)
                    {
                        Decimal p = (from t in db.TARJETAS
                                     where t.NRO_AFILIACION.Equals(beneficiario.id)
                                     select t.NRO_TARJETA
                                     ).SingleOrDefault();
                        if (p != 0)
                        {
                            beneficiario.pan = p.ToString();
                        }
                        else
                        {
                            beneficiario.pan = "";
                        }
                        string e = (from t in db.TARJETAS
                                    where t.NRO_AFILIACION.Equals(beneficiario.id)
                                    select t.ESTATUS_TARJETA
                                    ).SingleOrDefault();
                        if (e != null)
                        {
                            beneficiario.estatustarjeta = e.ToString();
                        }
                        else
                        {
                            beneficiario.estatustarjeta = "";
                        }
                    }
                }
                return cliente;
            }
        }

    }
}