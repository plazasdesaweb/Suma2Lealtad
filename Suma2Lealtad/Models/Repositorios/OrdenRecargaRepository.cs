using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class OrdenRecargaRepository
    {
        public List<OrdenRecargaPrepago> Find(string fecha, string estadoOrden)
        {
            List<OrdenRecargaPrepago> ordenes;
            using (LealtadEntities db = new LealtadEntities())
            {
                if (fecha == "")
                {
                    fecha = null;
                }
                if (estadoOrden == "")
                {
                    estadoOrden = null;
                }
                //BUSCAR POR estadoOrden
                if (estadoOrden != null)
                {
                    ordenes = (from o in db.Orders
                               join c in db.PrepaidCustomers on o.prepaidcustomerid equals c.id
                               join s in db.SumaStatuses on o.sumastatusid equals s.id
                               where s.name == estadoOrden
                               select new OrdenRecargaPrepago()
                               {
                                   id = o.id,
                                   statusOrden = s.name,
                                   montoOrden = o.totalamount,
                                   creationdateOrden = o.creationdate,
                                   Cliente = new ClientePrepago()
                                   {
                                       idCliente = c.id,
                                       nameCliente = c.name,
                                       aliasCliente = c.alias,
                                       rifCliente = c.rif,
                                       addressCliente = c.address,
                                       phoneCliente = c.phone,
                                       emailCliente = c.email
                                   }
                               }).OrderBy(x => x.id).ToList();
                }
                //BUSCAR POR fecha
                else if (fecha != null)
                {
                    DateTime f = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    ordenes = (from o in db.Orders
                               join c in db.PrepaidCustomers on o.prepaidcustomerid equals c.id
                               join s in db.SumaStatuses on o.sumastatusid equals s.id
                               select new OrdenRecargaPrepago()
                               {
                                   id = o.id,
                                   statusOrden = s.name,
                                   montoOrden = o.totalamount,
                                   creationdateOrden = o.creationdate,
                                   Cliente = new ClientePrepago()
                                   {
                                       idCliente = c.id,
                                       nameCliente = c.name,
                                       aliasCliente = c.alias,
                                       rifCliente = c.rif,
                                       addressCliente = c.address,
                                       phoneCliente = c.phone,
                                       emailCliente = c.email
                                   }
                               }).ToList()
                               .Where(q => q.creationdateOrden.Date == f.Date)
                               .OrderBy(x => x.id)
                               .ToList();
                }
                //BUSCAR TODAS                
                else
                {
                    ordenes = (from o in db.Orders
                               join c in db.PrepaidCustomers on o.prepaidcustomerid equals c.id
                               join s in db.SumaStatuses on o.sumastatusid equals s.id
                               select new OrdenRecargaPrepago()
                               {
                                   id = o.id,
                                   statusOrden = s.name,
                                   montoOrden = o.totalamount,
                                   creationdateOrden = o.creationdate,
                                   Cliente = new ClientePrepago()
                                   {
                                       idCliente = c.id,
                                       nameCliente = c.name,
                                       aliasCliente = c.alias,
                                       rifCliente = c.rif,
                                       addressCliente = c.address,
                                       phoneCliente = c.phone,
                                       emailCliente = c.email
                                   }
                               }).OrderBy(x => x.id).ToList();
                }
            }
            return ordenes;
        }
    }
}