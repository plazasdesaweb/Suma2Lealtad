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
                           where c.rif.Equals(rif)
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

        public ClientePrepago FindXidAfiliado(int idAfiliado)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                ClientePrepago cliente;
                cliente = (from c in db.PrepaidCustomers
                           join b in db.PrepaidBeneficiaries on c.id equals b.prepaidcustomerid
                           where b.affiliateid.Equals(idAfiliado)
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
       
    }
}