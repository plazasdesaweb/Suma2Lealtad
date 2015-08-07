using System;
using System.Collections.Generic;
using System.Globalization;
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

        public ClientePrepago Find(string rif)
        {
            ClientePrepago cliente;
            using (LealtadEntities db = new LealtadEntities())
            {
                cliente = (from c in db.PrepaidCustomers
                           select new ClientePrepago()
                           {
                               idCliente = c.id,
                               nameCliente = c.name,
                               aliasCliente = c.alias,
                               rifCliente = c.rif,
                               phoneCliente = c.phone,
                               emailCliente = c.email
                           }).FirstOrDefault();
            }
            return cliente;
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
                                }).OrderBy(x => x.rifCliente).ToList();
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
                                }).OrderBy(x => x.rifCliente).ToList();
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
       
        //public List<BeneficiarioPrepago> FindBeneficiarios(int id, string numdoc, string name, string email, string estadoAfiliacion, string estadoTarjeta)
        //{
        //    List<BeneficiarioPrepago> beneficiarios;
        //    using (LealtadEntities db = new LealtadEntities())
        //    {
        //        if (numdoc == "")
        //        {
        //            numdoc = null;
        //        }
        //        if (name == "")
        //        {
        //            name = null;
        //        }
        //        if (email == "")
        //        {
        //            email = null;
        //        }
        //        if (estadoAfiliacion == "")
        //        {
        //            estadoAfiliacion = null;
        //        }
        //        if (estadoTarjeta == "")
        //        {
        //            estadoTarjeta = null;
        //        }
        //        //BUSCAR POR ESTADO DE TARJETA
        //        if (estadoTarjeta != null)
        //        {
        //            beneficiarios = (from a in db.Affiliates
        //                             join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
        //                             //join s in db.SumaStatuses on a.statusid equals s.value
        //                             join s in db.SumaStatuses on a.statusid equals s.id
        //                             join t in db.Types on a.typeid equals t.id
        //                             join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
        //                             join x in db.PrepaidCustomers on b.prepaidcustomerid equals x.id
        //                             join tar in db.TARJETAS on a.id equals tar.NRO_AFILIACION
        //                             where b.prepaidcustomerid.Equals(id)
        //                                 //&& s.tablename.Equals("Affiliatte") //los estatus se organizan por tabla en sumastatus
        //                             && tar.ESTATUS_TARJETA.Equals(estadoTarjeta)
        //                             select new BeneficiarioPrepago()
        //                             {
        //                                 Afiliado = new AfiliadoSuma()
        //                                 {
        //                                     //ENTIDAD Affiliate 
        //                                     id = a.id,
        //                                     docnumber = a.docnumber,
        //                                     typeid = a.typeid,
        //                                     //ENTIDAD CLIENTE
        //                                     name = c.NOMBRE_CLIENTE1,
        //                                     lastname1 = c.APELLIDO_CLIENTE1,
        //                                     email = c.E_MAIL,
        //                                     //ENTIDAD SumaStatuses
        //                                     estatus = s.name,
        //                                     //ENTIDAD Type
        //                                     type = t.name
        //                                 },
        //                                 Cliente = new ClientePrepago()
        //                                 {
        //                                     idCliente = x.id,
        //                                     nameCliente = x.name,
        //                                     aliasCliente = x.alias,
        //                                     rifCliente = x.rif,
        //                                     addressCliente = x.address,
        //                                     phoneCliente = x.phone,
        //                                     emailCliente = x.email
        //                                 }
        //                             }).OrderBy(x => x.Afiliado.docnumber).ToList();
        //            return beneficiarios;
        //        }
        //        //BUSCAR TODOS
        //        if (numdoc == null && name == null && email == null && estadoAfiliacion == null && estadoTarjeta == null)
        //        {
        //            beneficiarios = (from a in db.Affiliates
        //                             join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
        //                             //join s in db.SumaStatuses on a.statusid equals s.value
        //                             join s in db.SumaStatuses on a.statusid equals s.id
        //                             join t in db.Types on a.typeid equals t.id
        //                             join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
        //                             join x in db.PrepaidCustomers on b.prepaidcustomerid equals x.id
        //                             where b.prepaidcustomerid.Equals(id)
        //                             //&& s.tablename == "Affiliatte" //los estatus se organizan por tabla en sumastatus
        //                             select new BeneficiarioPrepago()
        //                             {
        //                                 Afiliado = new AfiliadoSuma()
        //                                 {
        //                                     //ENTIDAD Affiliate 
        //                                     id = a.id,
        //                                     docnumber = a.docnumber,
        //                                     typeid = a.typeid,
        //                                     //ENTIDAD CLIENTE
        //                                     name = c.NOMBRE_CLIENTE1,
        //                                     lastname1 = c.APELLIDO_CLIENTE1,
        //                                     email = c.E_MAIL,
        //                                     //ENTIDAD SumaStatuses
        //                                     estatus = s.name,
        //                                     //ENTIDAD Type
        //                                     type = t.name
        //                                 },
        //                                 Cliente = new ClientePrepago()
        //                                 {
        //                                     idCliente = x.id,
        //                                     nameCliente = x.name,
        //                                     aliasCliente = x.alias,
        //                                     rifCliente = x.rif,
        //                                     addressCliente = x.address,
        //                                     phoneCliente = x.phone,
        //                                     emailCliente = x.email
        //                                 }
        //                             }).OrderBy(x => x.Afiliado.docnumber).ToList();
        //        }
        //        //BUSCAR POR numdoc
        //        else if (numdoc != null)
        //        {
        //            beneficiarios = (from a in db.Affiliates
        //                             join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
        //                             //join s in db.SumaStatuses on a.statusid equals s.value
        //                             join s in db.SumaStatuses on a.statusid equals s.id
        //                             join t in db.Types on a.typeid equals t.id
        //                             join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
        //                             join x in db.PrepaidCustomers on b.prepaidcustomerid equals x.id
        //                             where b.prepaidcustomerid.Equals(id)
        //                                 //&& s.tablename == "Affiliatte" //los estatus se organizan por tabla en sumastatus
        //                             && a.docnumber.Equals(numdoc)
        //                             select new BeneficiarioPrepago()
        //                             {
        //                                 Afiliado = new AfiliadoSuma()
        //                                 {
        //                                     //ENTIDAD Affiliate 
        //                                     id = a.id,
        //                                     docnumber = a.docnumber,
        //                                     typeid = a.typeid,
        //                                     //ENTIDAD CLIENTE
        //                                     name = c.NOMBRE_CLIENTE1,
        //                                     lastname1 = c.APELLIDO_CLIENTE1,
        //                                     email = c.E_MAIL,
        //                                     //ENTIDAD SumaStatuses
        //                                     estatus = s.name,
        //                                     //ENTIDAD Type
        //                                     type = t.name
        //                                 },
        //                                 Cliente = new ClientePrepago()
        //                                 {
        //                                     idCliente = x.id,
        //                                     nameCliente = x.name,
        //                                     aliasCliente = x.alias,
        //                                     rifCliente = x.rif,
        //                                     addressCliente = x.address,
        //                                     phoneCliente = x.phone,
        //                                     emailCliente = x.email
        //                                 }
        //                             }).OrderBy(x => x.Afiliado.docnumber).ToList();
        //        }
        //        //BUSCAR POR name O email O estadoAfiliacion
        //        else
        //        {
        //            beneficiarios = (from a in db.Affiliates
        //                             join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
        //                             //join s in db.SumaStatuses on a.statusid equals s.value
        //                             join s in db.SumaStatuses on a.statusid equals s.id
        //                             join t in db.Types on a.typeid equals t.id
        //                             join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
        //                             join x in db.PrepaidCustomers on b.prepaidcustomerid equals x.id
        //                             where b.prepaidcustomerid.Equals(id)
        //                                 //&& s.tablename == "Affiliatte" //los estatus se organizan por tabla en sumastatus
        //                             && (c.NOMBRE_CLIENTE1.Contains(name) || c.APELLIDO_CLIENTE1.Contains(name) || c.E_MAIL.Equals(email) || s.name.Equals(estadoAfiliacion))
        //                             select new BeneficiarioPrepago()
        //                             {
        //                                 Afiliado = new AfiliadoSuma()
        //                                 {
        //                                     //ENTIDAD Affiliate 
        //                                     id = a.id,
        //                                     docnumber = a.docnumber,
        //                                     typeid = a.typeid,
        //                                     //ENTIDAD CLIENTE
        //                                     name = c.NOMBRE_CLIENTE1,
        //                                     lastname1 = c.APELLIDO_CLIENTE1,
        //                                     email = c.E_MAIL,
        //                                     //ENTIDAD SumaStatuses
        //                                     estatus = s.name,
        //                                     //ENTIDAD Type
        //                                     type = t.name
        //                                 },
        //                                 Cliente = new ClientePrepago()
        //                                 {
        //                                     idCliente = x.id,
        //                                     nameCliente = x.name,
        //                                     aliasCliente = x.alias,
        //                                     rifCliente = x.rif,
        //                                     addressCliente = x.address,
        //                                     phoneCliente = x.phone,
        //                                     emailCliente = x.email
        //                                 }
        //                             }).OrderBy(x => x.Afiliado.docnumber).ToList();
        //        }
        //        foreach (var beneficiario in beneficiarios)
        //        {
        //            Decimal p = (from t in db.TARJETAS
        //                         where t.NRO_AFILIACION.Equals(beneficiario.Afiliado.id)
        //                         select t.NRO_TARJETA
        //                         ).SingleOrDefault();
        //            if (p != 0)
        //            {
        //                beneficiario.Afiliado.pan = p.ToString();
        //            }
        //            else
        //            {
        //                beneficiario.Afiliado.pan = "";
        //            }
        //            string e = (from t in db.TARJETAS
        //                        where t.NRO_AFILIACION.Equals(beneficiario.Afiliado.id)
        //                        select t.ESTATUS_TARJETA
        //                        ).SingleOrDefault();
        //            if (e != null)
        //            {
        //                beneficiario.Afiliado.estatustarjeta = e.ToString();
        //            }
        //            else
        //            {
        //                beneficiario.Afiliado.estatustarjeta = "";
        //            }
        //        }
        //    }
        //    return beneficiarios;
        //}

        //public List<OrdenRecargaPrepago> FindOrdenes(int id, string fecha, string estadoOrden)
        //{
        //    List<OrdenRecargaPrepago> ordenes;
        //    using (LealtadEntities db = new LealtadEntities())
        //    {
        //        if (fecha == "")
        //        {
        //            fecha = null;
        //        }
        //        if (estadoOrden == "")
        //        {
        //            estadoOrden = null;
        //        }
        //        //BUSCAR POR estadoOrden
        //        if (estadoOrden != null)
        //        {
        //            ordenes = (from o in db.Orders
        //                       join c in db.PrepaidCustomers on o.prepaidcustomerid equals c.id
        //                       //join s in db.SumaStatuses on o.sumastatusid equals s.value
        //                       join s in db.SumaStatuses on o.sumastatusid equals s.id
        //                       where o.prepaidcustomerid.Equals(id)
        //                           //&& s.tablename == "Order"
        //                       && s.name == estadoOrden
        //                       select new OrdenRecargaPrepago()
        //                       {
        //                           id = o.id,
        //                           statusOrden = s.name,
        //                           montoOrden = o.totalamount,
        //                           creationdateOrden = o.creationdate,
        //                           Cliente = new ClientePrepago()
        //                           {
        //                               idCliente = c.id,
        //                               nameCliente = c.name,
        //                               aliasCliente = c.alias,
        //                               rifCliente = c.rif,
        //                               addressCliente = c.address,
        //                               phoneCliente = c.phone,
        //                               emailCliente = c.email
        //                           }
        //                       }).OrderBy(x => x.id).ToList();
        //        }
        //        //BUSCAR POR fecha
        //        else if (fecha != null)
        //        {
        //            DateTime f = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //            ordenes = (from o in db.Orders
        //                       join c in db.PrepaidCustomers on o.prepaidcustomerid equals c.id
        //                       //join s in db.SumaStatuses on o.sumastatusid equals s.value
        //                       join s in db.SumaStatuses on o.sumastatusid equals s.id
        //                       where o.prepaidcustomerid.Equals(id)
        //                       //&& s.tablename == "Order"
        //                       select new OrdenRecargaPrepago()
        //                       {
        //                           id = o.id,
        //                           statusOrden = s.name,
        //                           montoOrden = o.totalamount,
        //                           creationdateOrden = o.creationdate,
        //                           Cliente = new ClientePrepago()
        //                           {
        //                               idCliente = c.id,
        //                               nameCliente = c.name,
        //                               aliasCliente = c.alias,
        //                               rifCliente = c.rif,
        //                               addressCliente = c.address,
        //                               phoneCliente = c.phone,
        //                               emailCliente = c.email
        //                           }
        //                       }).ToList()
        //                       .Where(q => q.creationdateOrden.Date == f.Date)
        //                       .OrderBy(x => x.id)
        //                       .ToList();
        //        }
        //        //BUSCAR TODAS                
        //        else
        //        {
        //            ordenes = (from o in db.Orders
        //                       join c in db.PrepaidCustomers on o.prepaidcustomerid equals c.id
        //                       //join s in db.SumaStatuses on o.sumastatusid equals s.value
        //                       join s in db.SumaStatuses on o.sumastatusid equals s.id
        //                       where o.prepaidcustomerid.Equals(id)
        //                       //&& s.tablename == "Order"
        //                       select new OrdenRecargaPrepago()
        //                       {
        //                           id = o.id,
        //                           statusOrden = s.name,
        //                           montoOrden = o.totalamount,
        //                           creationdateOrden = o.creationdate,
        //                           Cliente = new ClientePrepago()
        //                           {
        //                               idCliente = c.id,
        //                               nameCliente = c.name,
        //                               aliasCliente = c.alias,
        //                               rifCliente = c.rif,
        //                               addressCliente = c.address,
        //                               phoneCliente = c.phone,
        //                               emailCliente = c.email
        //                           }
        //                       }).OrderBy(x => x.id).ToList();
        //        }
        //    }
        //    return ordenes;
        //}

    }
}