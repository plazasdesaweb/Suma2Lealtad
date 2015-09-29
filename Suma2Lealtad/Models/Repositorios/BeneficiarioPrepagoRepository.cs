using LinqToExcel;
using Newtonsoft.Json;
using Suma2Lealtad.Modules;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class BeneficiarioPrepagoRepository
    {
        private const int ID_ESTATUS_AFILIACION_INICIAL = 0;
        private const String TRANSCODE_COMPRA_PREPAGO = "145";
        
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

        public List<ReportePrepago> ReporteComprasxCliente(string tiporeporte, string fechadesde, string fechahasta, string modotrans, int idCliente = 0)
        {
            string fechasdesdemod = fechadesde.Substring(6, 4) + fechadesde.Substring(3, 2) + fechadesde.Substring(0, 2);
            string fechahastamod = fechahasta.Substring(6, 4) + fechahasta.Substring(3, 2) + fechahasta.Substring(0, 2);
            List<ReportePrepago> reporte = new List<ReportePrepago>();
            EncabezadoReporte encabezado = new EncabezadoReporte();
            #region Por Cliente específico
            if (tiporeporte == "uno")
            {
                List<BeneficiarioPrepago> beneficiarios = Find("", "", "", "", "").Where(b => b.Cliente.idCliente == idCliente).ToList();
                encabezado.nombreReporte = "Reporte de Compras";
                encabezado.tipoconsultaReporte = "Por Cliente";
                encabezado.parametrotipoconsultaReporte = beneficiarios.First().Cliente.rifCliente + " " + beneficiarios.First().Cliente.nameCliente;
                encabezado.fechainicioReporte = fechadesde;
                encabezado.fechafinReporte = fechahasta;
                encabezado.modotransaccionReporte = modotrans;
                foreach (BeneficiarioPrepago b in beneficiarios)
                {
                    string movimientosPrepagoJson = WSL.Cards.getReport(fechasdesdemod, fechahastamod, b.Afiliado.docnumber.Substring(2), TRANSCODE_COMPRA_PREPAGO);
                    if (WSL.Cards.ExceptionServicioCards(movimientosPrepagoJson))
                    {
                        return null;
                    }
                    List<Movimiento> movimientosPrepago = (List<Movimiento>)JsonConvert.DeserializeObject<List<Movimiento>>(movimientosPrepagoJson).OrderBy(x => x.fecha).ToList();
                    foreach (Movimiento m in movimientosPrepago)
                    {
                        ReportePrepago linea = new ReportePrepago()
                        {
                            Beneficiario = b,
                            fecha = DateTime.ParseExact(m.fecha.Substring(6, 2) + "-" + m.fecha.Substring(4, 2) + "-" + m.fecha.Substring(0, 4), "dd-MM-yyyy", CultureInfo.InvariantCulture),
                            monto = Convert.ToDecimal(m.saldo),
                            detalle = m.isodescription,
                            tipo = m.transcode + "-" + m.transname,
                            Encabezado = encabezado
                        };
                        if (modotrans == "En Linea")
                        {
                            if (!linea.detalle.Contains("offline"))
                            {
                                reporte.Add(linea);
                            }
                        }
                        else if (modotrans == "Offline")
                        {
                            if (linea.detalle.Contains("offline"))
                            {
                                reporte.Add(linea);
                            }
                        }
                        else
                        {
                            reporte.Add(linea);
                        }
                    }
                }
            }
            #endregion
            #region Todos los Clientes
            else if (tiporeporte == "todos")
            {
                ClientePrepagoRepository repCliente = new ClientePrepagoRepository();
                List<ClientePrepago> clientes = repCliente.Find("", "").OrderBy(x => x.idCliente).ToList();
                encabezado.nombreReporte = "Reporte de Compras";
                encabezado.tipoconsultaReporte = "Por Cliente";
                encabezado.parametrotipoconsultaReporte = "Todos";
                encabezado.fechainicioReporte = fechadesde;
                encabezado.fechafinReporte = fechahasta;
                encabezado.modotransaccionReporte = modotrans;
                foreach (ClientePrepago c in clientes)
                {
                    List<BeneficiarioPrepago> beneficiarios = Find("", "", "", "", "").Where(b => b.Cliente.idCliente == c.idCliente).OrderBy(x => x.Afiliado.id).ToList();
                    foreach (BeneficiarioPrepago b in beneficiarios)
                    {
                        string movimientosPrepagoJson = WSL.Cards.getReport(fechasdesdemod, fechahastamod, b.Afiliado.docnumber.Substring(2), TRANSCODE_COMPRA_PREPAGO);
                        if (WSL.Cards.ExceptionServicioCards(movimientosPrepagoJson))
                        {
                            return null;
                        }
                        List<Movimiento> movimientosPrepago = (List<Movimiento>)JsonConvert.DeserializeObject<List<Movimiento>>(movimientosPrepagoJson).OrderBy(x => x.fecha).ToList();
                        foreach (Movimiento m in movimientosPrepago)
                        {
                            ReportePrepago linea = new ReportePrepago()
                            {
                                Beneficiario = b,
                                fecha = DateTime.ParseExact(m.fecha.Substring(6, 2) + "-" + m.fecha.Substring(4, 2) + "-" + m.fecha.Substring(0, 4), "dd-MM-yyyy", CultureInfo.InvariantCulture),
                                monto = Convert.ToDecimal(m.saldo),
                                detalle = m.isodescription,
                                tipo = m.transcode + "-" + m.transname,
                                Encabezado = encabezado
                            };
                            if (modotrans == "En Linea")
                            {
                                if (!linea.detalle.Contains("offline"))
                                {
                                    reporte.Add(linea);
                                }
                            }
                            else if (modotrans == "Offline")
                            {
                                if (linea.detalle.Contains("offline"))
                                {
                                    reporte.Add(linea);
                                }
                            }
                            else
                            {
                                reporte.Add(linea);
                            }
                        }
                    }
                }
            }
            #endregion
            if (reporte.Count == 0)
            {
                ReportePrepago r = new ReportePrepago()
                {
                    Encabezado = encabezado
                };
                reporte.Add(r);
            }
            return reporte.OrderBy(x => x.fecha).ToList();
        }

        public List<ReportePrepago> ReporteComprasxBeneficiario(string tiporeporte, string fechadesde, string fechahasta, string modotrans, string numdoc = "")
        {
            string fechasdesdemod = fechadesde.Substring(6, 4) + fechadesde.Substring(3, 2) + fechadesde.Substring(0, 2);
            string fechahastamod = fechahasta.Substring(6, 4) + fechahasta.Substring(3, 2) + fechahasta.Substring(0, 2);
            List<ReportePrepago> reporte = new List<ReportePrepago>();
            EncabezadoReporte encabezado = new EncabezadoReporte();
            #region Por Beneficiario específico
            if (tiporeporte == "uno")
            {
                List<BeneficiarioPrepago> beneficiarios = Find(numdoc, "", "", "", "");
                encabezado.nombreReporte = "Reporte de Compras";
                encabezado.tipoconsultaReporte = "Por Beneficiario";
                if (beneficiarios.Count == 0)
                {
                    encabezado.parametrotipoconsultaReporte = numdoc;
                }
                else
                {
                    encabezado.parametrotipoconsultaReporte = beneficiarios.First().Afiliado.docnumber + " " + beneficiarios.First().Afiliado.name + " " + beneficiarios.First().Afiliado.lastname1;
                }
                encabezado.fechainicioReporte = fechadesde;
                encabezado.fechafinReporte = fechahasta;
                encabezado.modotransaccionReporte = modotrans;
                foreach (BeneficiarioPrepago b in beneficiarios)
                {
                    string movimientosPrepagoJson = WSL.Cards.getReport(fechasdesdemod, fechahastamod, b.Afiliado.docnumber.Substring(2), TRANSCODE_COMPRA_PREPAGO);
                    if (WSL.Cards.ExceptionServicioCards(movimientosPrepagoJson))
                    {
                        return null;
                    }
                    List<Movimiento> movimientosPrepago = (List<Movimiento>)JsonConvert.DeserializeObject<List<Movimiento>>(movimientosPrepagoJson).OrderBy(x => x.fecha).ToList();
                    foreach (Movimiento m in movimientosPrepago)
                    {
                        ReportePrepago linea = new ReportePrepago()
                        {
                            Beneficiario = b,
                            fecha = DateTime.ParseExact(m.fecha.Substring(6, 2) + "-" + m.fecha.Substring(4, 2) + "-" + m.fecha.Substring(0, 4), "dd-MM-yyyy", CultureInfo.InvariantCulture),
                            monto = Convert.ToDecimal(m.saldo),
                            detalle = m.isodescription,
                            tipo = m.transcode + "-" + m.transname,
                            Encabezado = encabezado
                        };
                        if (modotrans == "En Linea")
                        {
                            if (!linea.detalle.Contains("offline"))
                            {
                                reporte.Add(linea);
                            }
                        }
                        else if (modotrans == "Offline")
                        {
                            if (linea.detalle.Contains("offline"))
                            {
                                reporte.Add(linea);
                            }
                        }
                        else
                        {
                            reporte.Add(linea);
                        }
                    }
                }
            }
            #endregion
            #region Todos los Beneficiarios
            else if (tiporeporte == "todos")
            {
                List<BeneficiarioPrepago> beneficiarios = Find("", "", "", "", "");
                encabezado.nombreReporte = "Reporte de Compras";
                encabezado.tipoconsultaReporte = "Por Cliente";
                encabezado.parametrotipoconsultaReporte = "Todos";
                encabezado.fechainicioReporte = fechadesde;
                encabezado.fechafinReporte = fechahasta;
                encabezado.modotransaccionReporte = modotrans;
                foreach (BeneficiarioPrepago b in beneficiarios)
                {
                    string movimientosPrepagoJson = WSL.Cards.getReport(fechasdesdemod, fechahastamod, b.Afiliado.docnumber.Substring(2), TRANSCODE_COMPRA_PREPAGO);
                    if (WSL.Cards.ExceptionServicioCards(movimientosPrepagoJson))
                    {
                        return null;
                    }
                    List<Movimiento> movimientosPrepago = (List<Movimiento>)JsonConvert.DeserializeObject<List<Movimiento>>(movimientosPrepagoJson).OrderBy(x => x.fecha).ToList();
                    foreach (Movimiento m in movimientosPrepago)
                    {
                        ReportePrepago linea = new ReportePrepago()
                        {
                            Beneficiario = b,
                            fecha = DateTime.ParseExact(m.fecha.Substring(6, 2) + "-" + m.fecha.Substring(4, 2) + "-" + m.fecha.Substring(0, 4), "dd-MM-yyyy", CultureInfo.InvariantCulture),
                            monto = Convert.ToDecimal(m.saldo),
                            detalle = m.isodescription,
                            tipo = m.transcode + "-" + m.transname,
                            Encabezado = encabezado
                        };
                        if (modotrans == "En Linea")
                        {
                            if (!linea.detalle.Contains("offline"))
                            {
                                reporte.Add(linea);
                            }
                        }
                        else if (modotrans == "Offline")
                        {
                            if (linea.detalle.Contains("offline"))
                            {
                                reporte.Add(linea);
                            }
                        }
                        else
                        {
                            reporte.Add(linea);
                        }
                    }
                }
            }
            #endregion
            if (reporte.Count == 0)
            {
                ReportePrepago r = new ReportePrepago()
                {
                    Encabezado = encabezado
                };
                reporte.Add(r);
            }
            return reporte.OrderBy(x => x.fecha).ToList();
        }

        public List<ReportePrepago> ReporteRecargasxCliente(string tiporeporte, string fechadesde, string fechahasta, int idCliente = 0, string referencia = "")
        {
            List<ReportePrepago> reporte = new List<ReportePrepago>();
            using (LealtadEntities db = new LealtadEntities())
            {
                #region Por Cliente específico
                if (tiporeporte == "uno")
                {
                    if (referencia == "")
                    {
                        reporte = (from o in db.Orders
                                   join od in db.OrdersDetails on o.id equals od.orderid
                                   join a in db.Affiliates on od.customerid equals a.id
                                   join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                   join s in db.SumaStatuses on o.sumastatusid equals s.id
                                   join p in db.PrepaidCustomers on o.prepaidcustomerid equals p.id
                                   where o.prepaidcustomerid == idCliente && s.name == "Procesada" && od.comments == "Recarga efectiva"
                                   select new ReportePrepago()
                                   {
                                       Beneficiario = new BeneficiarioPrepago()
                                       {
                                           Afiliado = new AfiliadoSuma()
                                           {
                                               docnumber = a.docnumber,
                                               name = c.NOMBRE_CLIENTE1,
                                               lastname1 = c.APELLIDO_CLIENTE1
                                           },
                                           Cliente = new ClientePrepago()
                                           {
                                               idCliente = idCliente,
                                               nameCliente = p.name
                                           }
                                       },
                                       fecha = o.processdate,
                                       monto = od.amount,
                                       detalle = od.comments,
                                       tipo = "200-Recarga",
                                       nroordenrecarga = o.id,
                                       referenciarecarga = o.documento,
                                       Encabezado = new EncabezadoReporte()
                                       {
                                           nombreReporte = "Reporte de Recargas",
                                           tipoconsultaReporte = "Por Cliente",
                                           parametrotipoconsultaReporte = p.rif + " " + p.name,
                                           fechainicioReporte = fechadesde,
                                           fechafinReporte = fechahasta,
                                           documentoreferenciaReporte = referencia
                                       }
                                   }).ToList();
                    }
                    else
                    {
                        reporte = (from o in db.Orders
                                   join od in db.OrdersDetails on o.id equals od.orderid
                                   join a in db.Affiliates on od.customerid equals a.id
                                   join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                   join s in db.SumaStatuses on o.sumastatusid equals s.id
                                   join p in db.PrepaidCustomers on o.prepaidcustomerid equals p.id
                                   where o.prepaidcustomerid == idCliente && s.name == "Procesada" && o.documento == referencia && od.comments == "Recarga efectiva"
                                   select new ReportePrepago()
                                   {
                                       Beneficiario = new BeneficiarioPrepago()
                                       {
                                           Afiliado = new AfiliadoSuma()
                                           {
                                               docnumber = a.docnumber,
                                               name = c.NOMBRE_CLIENTE1,
                                               lastname1 = c.APELLIDO_CLIENTE1
                                           },
                                           Cliente = new ClientePrepago()
                                           {
                                               idCliente = idCliente,
                                               nameCliente = p.name
                                           }
                                       },
                                       fecha = o.processdate,
                                       monto = od.amount,
                                       detalle = od.comments,
                                       tipo = "200-Recarga",
                                       nroordenrecarga = o.id,
                                       referenciarecarga = o.documento,
                                       Encabezado = new EncabezadoReporte()
                                       {
                                           nombreReporte = "Reporte de Recargas",
                                           tipoconsultaReporte = "Por Cliente",
                                           parametrotipoconsultaReporte = p.rif + " " + p.name,
                                           fechainicioReporte = fechadesde,
                                           fechafinReporte = fechahasta,
                                           documentoreferenciaReporte = referencia
                                       }
                                   }).ToList();
                    }
                    if (reporte.Count() == 0)
                    {
                        ClientePrepagoRepository repCliente = new ClientePrepagoRepository();
                        ClientePrepago Cliente = repCliente.Find(idCliente);
                        ReportePrepago r = new ReportePrepago()
                        {
                            Encabezado = new EncabezadoReporte()
                            {
                                nombreReporte = "Reporte de Recargas",
                                tipoconsultaReporte = "Por Cliente",
                                parametrotipoconsultaReporte = Cliente.rifCliente + " " + Cliente.nameCliente,
                                fechainicioReporte = fechadesde,
                                fechafinReporte = fechahasta,
                                documentoreferenciaReporte = referencia
                            }
                        };
                        reporte.Add(r);
                    }
                }
                #endregion
                #region Todos los Clientes
                else if (tiporeporte == "todos")
                {
                    if (referencia == "")
                    {
                        reporte = (from o in db.Orders
                                   join od in db.OrdersDetails on o.id equals od.orderid
                                   join a in db.Affiliates on od.customerid equals a.id
                                   join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                   join s in db.SumaStatuses on o.sumastatusid equals s.id
                                   join p in db.PrepaidCustomers on o.prepaidcustomerid equals p.id
                                   where s.name == "Procesada" && od.comments == "Recarga efectiva"
                                   select new ReportePrepago()
                                   {
                                       Beneficiario = new BeneficiarioPrepago()
                                       {
                                           Afiliado = new AfiliadoSuma()
                                           {
                                               docnumber = a.docnumber,
                                               name = c.NOMBRE_CLIENTE1,
                                               lastname1 = c.APELLIDO_CLIENTE1
                                           },
                                           Cliente = new ClientePrepago()
                                           {
                                               idCliente = idCliente,
                                               nameCliente = p.name
                                           }
                                       },
                                       fecha = o.processdate,
                                       monto = od.amount,
                                       detalle = od.comments,
                                       tipo = "200-Recarga",
                                       nroordenrecarga = o.id,
                                       referenciarecarga = o.documento,
                                       Encabezado = new EncabezadoReporte()
                                       {
                                           nombreReporte = "Reporte de Recargas",
                                           tipoconsultaReporte = "Por Cliente",
                                           parametrotipoconsultaReporte = "Todos",
                                           fechainicioReporte = fechadesde,
                                           fechafinReporte = fechahasta,
                                           documentoreferenciaReporte = referencia
                                       }
                                   }).ToList();
                    }
                    else
                    {
                        reporte = (from o in db.Orders
                                   join od in db.OrdersDetails on o.id equals od.orderid
                                   join a in db.Affiliates on od.customerid equals a.id
                                   join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                   join s in db.SumaStatuses on o.sumastatusid equals s.id
                                   join p in db.PrepaidCustomers on o.prepaidcustomerid equals p.id
                                   where s.name == "Procesada" && o.documento == referencia && od.comments == "Recarga efectiva"
                                   select new ReportePrepago()
                                   {
                                       Beneficiario = new BeneficiarioPrepago()
                                       {
                                           Afiliado = new AfiliadoSuma()
                                           {
                                               docnumber = a.docnumber,
                                               name = c.NOMBRE_CLIENTE1,
                                               lastname1 = c.APELLIDO_CLIENTE1
                                           },
                                           Cliente = new ClientePrepago()
                                           {
                                               idCliente = idCliente,
                                               nameCliente = p.name
                                           }
                                       },
                                       fecha = o.processdate,
                                       monto = od.amount,
                                       detalle = od.comments,
                                       tipo = "200-Recarga",
                                       nroordenrecarga = o.id,
                                       referenciarecarga = o.documento,
                                       Encabezado = new EncabezadoReporte()
                                       {
                                           nombreReporte = "Reporte de Recargas",
                                           tipoconsultaReporte = "Por Cliente",
                                           parametrotipoconsultaReporte = "Todos",
                                           fechainicioReporte = fechadesde,
                                           fechafinReporte = fechahasta,
                                           documentoreferenciaReporte = referencia
                                       }
                                   }).ToList();

                    }
                    if (reporte.Count() == 0)
                    {
                        ClientePrepagoRepository repCliente = new ClientePrepagoRepository();
                        ClientePrepago Cliente = repCliente.Find(idCliente);
                        ReportePrepago r = new ReportePrepago()
                        {
                            Encabezado = new EncabezadoReporte()
                            {
                                nombreReporte = "Reporte de Recargas",
                                tipoconsultaReporte = "Por Cliente",
                                parametrotipoconsultaReporte = "Todos",
                                fechainicioReporte = fechadesde,
                                fechafinReporte = fechahasta,
                                documentoreferenciaReporte = referencia
                            }
                        };
                        reporte.Add(r);
                    }
                }
                #endregion
            }
            DateTime desde = DateTime.ParseExact(fechadesde, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime hasta = DateTime.ParseExact(fechahasta, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return reporte.Where(x => x.fecha.Date > desde && x.fecha.Date < hasta).OrderBy(x => x.fecha).ToList();
        }

        public List<ReportePrepago> ReporteRecargasxBeneficiario(string tiporeporte, string fechadesde, string fechahasta, string numdoc = "", string referencia = "")
        {
            List<ReportePrepago> reporte = new List<ReportePrepago>();
            EncabezadoReporte encabezado = new EncabezadoReporte();
            string textoparametro;
            using (LealtadEntities db = new LealtadEntities())
            {
                #region Por Beneficiario específico
                if (tiporeporte == "uno")
                {
                    List<BeneficiarioPrepago> beneficiarios = Find(numdoc, "", "", "", "");
                    if (beneficiarios.Count == 0)
                    {
                        textoparametro = numdoc;
                    }
                    else
                    {
                        textoparametro = beneficiarios.First().Afiliado.docnumber + " " + beneficiarios.First().Afiliado.name + " " + beneficiarios.First().Afiliado.lastname1;
                        if (referencia == "")
                        {
                            reporte = (from o in db.Orders
                                       join od in db.OrdersDetails on o.id equals od.orderid
                                       join a in db.Affiliates on od.customerid equals a.id
                                       join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                       join s in db.SumaStatuses on o.sumastatusid equals s.id
                                       join p in db.PrepaidCustomers on o.prepaidcustomerid equals p.id
                                       where a.docnumber == numdoc && s.name == "Procesada" && od.comments == "Recarga efectiva"
                                       select new ReportePrepago()
                                       {
                                           Beneficiario = new BeneficiarioPrepago()
                                           {
                                               Afiliado = new AfiliadoSuma()
                                               {
                                                   docnumber = a.docnumber,
                                                   name = c.NOMBRE_CLIENTE1,
                                                   lastname1 = c.APELLIDO_CLIENTE1
                                               },
                                               Cliente = new ClientePrepago()
                                               {
                                                   idCliente = p.id,
                                                   nameCliente = p.name
                                               }
                                           },
                                           fecha = o.processdate,
                                           monto = od.amount,
                                           detalle = od.comments,
                                           tipo = "200-Recarga",
                                           nroordenrecarga = o.id,
                                           referenciarecarga = o.documento,
                                           Encabezado = new EncabezadoReporte()
                                           {
                                               nombreReporte = "Reporte de Recargas",
                                               tipoconsultaReporte = "Por Beneficiario",
                                               parametrotipoconsultaReporte = textoparametro,
                                               fechainicioReporte = fechadesde,
                                               fechafinReporte = fechahasta,
                                               documentoreferenciaReporte = referencia
                                           }
                                       }).ToList();
                        }
                        else
                        {
                            reporte = (from o in db.Orders
                                       join od in db.OrdersDetails on o.id equals od.orderid
                                       join a in db.Affiliates on od.customerid equals a.id
                                       join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                       join s in db.SumaStatuses on o.sumastatusid equals s.id
                                       join p in db.PrepaidCustomers on o.prepaidcustomerid equals p.id
                                       where a.docnumber == numdoc && s.name == "Procesada" && o.documento == referencia && od.comments == "Recarga efectiva"
                                       select new ReportePrepago()
                                       {
                                           Beneficiario = new BeneficiarioPrepago()
                                           {
                                               Afiliado = new AfiliadoSuma()
                                               {
                                                   docnumber = a.docnumber,
                                                   name = c.NOMBRE_CLIENTE1,
                                                   lastname1 = c.APELLIDO_CLIENTE1
                                               },
                                               Cliente = new ClientePrepago()
                                               {
                                                   idCliente = p.id,
                                                   nameCliente = p.name
                                               }
                                           },
                                           fecha = o.processdate,
                                           monto = od.amount,
                                           detalle = od.comments,
                                           tipo = "200-Recarga",
                                           nroordenrecarga = o.id,
                                           referenciarecarga = o.documento,
                                           Encabezado = new EncabezadoReporte()
                                           {
                                               nombreReporte = "Reporte de Recargas",
                                               tipoconsultaReporte = "Por Beneficiario",
                                               parametrotipoconsultaReporte = textoparametro,
                                               fechainicioReporte = fechadesde,
                                               fechafinReporte = fechahasta,
                                               documentoreferenciaReporte = referencia
                                           }
                                       }).ToList();

                        }
                    }
                    if (reporte.Count() == 0)
                    {
                        ReportePrepago r = new ReportePrepago()
                        {
                            Encabezado = new EncabezadoReporte()
                            {
                                nombreReporte = "Reporte de Recargas",
                                tipoconsultaReporte = "Por Beneficiario",
                                parametrotipoconsultaReporte = textoparametro,
                                fechainicioReporte = fechadesde,
                                fechafinReporte = fechahasta,
                                documentoreferenciaReporte = referencia
                            }
                        };
                        reporte.Add(r);
                    }
                }
                #endregion
                #region Todos los Beneficiarios
                else if (tiporeporte == "todos")
                {
                    if (referencia == "")
                    {
                        reporte = (from o in db.Orders
                                   join od in db.OrdersDetails on o.id equals od.orderid
                                   join a in db.Affiliates on od.customerid equals a.id
                                   join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                   join s in db.SumaStatuses on o.sumastatusid equals s.id
                                   join p in db.PrepaidCustomers on o.prepaidcustomerid equals p.id
                                   where s.name == "Procesada" && od.comments == "Recarga efectiva"
                                   select new ReportePrepago()
                                   {
                                       Beneficiario = new BeneficiarioPrepago()
                                       {
                                           Afiliado = new AfiliadoSuma()
                                           {
                                               docnumber = a.docnumber,
                                               name = c.NOMBRE_CLIENTE1,
                                               lastname1 = c.APELLIDO_CLIENTE1
                                           },
                                           Cliente = new ClientePrepago()
                                           {
                                               idCliente = p.id,
                                               nameCliente = p.name
                                           }
                                       },
                                       fecha = o.processdate,
                                       monto = od.amount,
                                       detalle = od.comments,
                                       tipo = "200-Recarga",
                                       nroordenrecarga = o.id,
                                       referenciarecarga = o.documento,
                                       Encabezado = new EncabezadoReporte()
                                       {
                                           nombreReporte = "Reporte de Recargas",
                                           tipoconsultaReporte = "Por Beneficiario",
                                           parametrotipoconsultaReporte = "Todos",
                                           fechainicioReporte = fechadesde,
                                           fechafinReporte = fechahasta,
                                           documentoreferenciaReporte = referencia
                                       }
                                   }).ToList();
                    }
                    else
                    {
                        reporte = (from o in db.Orders
                                   join od in db.OrdersDetails on o.id equals od.orderid
                                   join a in db.Affiliates on od.customerid equals a.id
                                   join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                   join s in db.SumaStatuses on o.sumastatusid equals s.id
                                   join p in db.PrepaidCustomers on o.prepaidcustomerid equals p.id
                                   where s.name == "Procesada" && o.documento == referencia && od.comments == "Recarga efectiva"
                                   select new ReportePrepago()
                                   {
                                       Beneficiario = new BeneficiarioPrepago()
                                       {
                                           Afiliado = new AfiliadoSuma()
                                           {
                                               docnumber = a.docnumber,
                                               name = c.NOMBRE_CLIENTE1,
                                               lastname1 = c.APELLIDO_CLIENTE1
                                           },
                                           Cliente = new ClientePrepago()
                                           {
                                               idCliente = p.id,
                                               nameCliente = p.name
                                           }
                                       },
                                       fecha = o.processdate,
                                       monto = od.amount,
                                       detalle = od.comments,
                                       tipo = "200-Recarga",
                                       nroordenrecarga = o.id,
                                       referenciarecarga = o.documento,
                                       Encabezado = new EncabezadoReporte()
                                       {
                                           nombreReporte = "Reporte de Recargas",
                                           tipoconsultaReporte = "Por Beneficiario",
                                           parametrotipoconsultaReporte = "Todos",
                                           fechainicioReporte = fechadesde,
                                           fechafinReporte = fechahasta,
                                           documentoreferenciaReporte = referencia
                                       }
                                   }).ToList();

                    }
                    if (reporte.Count() == 0)
                    {
                        ReportePrepago r = new ReportePrepago()
                        {
                            Encabezado = new EncabezadoReporte()
                            {
                                nombreReporte = "Reporte de Recargas",
                                tipoconsultaReporte = "Por Beneficiario",
                                parametrotipoconsultaReporte = "Todos",
                                fechainicioReporte = fechadesde,
                                fechafinReporte = fechahasta,
                                documentoreferenciaReporte = referencia
                            }
                        };
                        reporte.Add(r);
                    }
                }
                #endregion
            }
            DateTime desde = DateTime.ParseExact(fechadesde, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime hasta = DateTime.ParseExact(fechahasta, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return reporte.Where(x => x.fecha.Date > desde && x.fecha.Date < hasta).OrderBy(x => x.fecha).ToList();
        }

        public List<ReportePrepago> ReporteTarjetasxCliente(string tiporeporte, string fechadesde, string fechahasta, int idCliente = 0, string estadoTarjeta = "")
        {
            List<ReportePrepago> reporte = new List<ReportePrepago>();
            using (LealtadEntities db = new LealtadEntities())
            {
                #region Por Cliente específico
                if (tiporeporte == "uno")
                {
                    if (estadoTarjeta == "")
                    {
                        reporte = (from a in db.Affiliates
                                   join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                   join s in db.SumaStatuses on a.statusid equals s.id
                                   join t in db.Types on a.typeid equals t.id
                                   join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                   join p in db.PrepaidCustomers on b.prepaidcustomerid equals p.id
                                   join tar in db.TARJETAS on a.id equals tar.NRO_AFILIACION
                                   where p.id == idCliente
                                   select new ReportePrepago()
                                   {
                                       Beneficiario = new BeneficiarioPrepago()
                                       {
                                           Afiliado = new AfiliadoSuma()
                                           {
                                               docnumber = a.docnumber,
                                               name = c.NOMBRE_CLIENTE1,
                                               lastname1 = c.APELLIDO_CLIENTE1
                                           },
                                           Cliente = new ClientePrepago()
                                           {
                                               idCliente = idCliente,
                                               nameCliente = p.name
                                           }
                                       },
                                       fecha = tar.FECHA_CREACION.Value,
                                       numerotarjeta = tar.NRO_TARJETA,
                                       estatustarjeta = tar.ESTATUS_TARJETA,
                                       Encabezado = new EncabezadoReporte()
                                       {
                                           nombreReporte = "Reporte de Tarjetas",
                                           tipoconsultaReporte = "Por Cliente",
                                           parametrotipoconsultaReporte = p.rif + " " + p.name,
                                           fechainicioReporte = fechadesde,
                                           fechafinReporte = fechahasta,
                                           estatustarjetaReporte = "Todos"
                                       }
                                   }).ToList();
                    }
                    else
                    {
                        reporte = (from a in db.Affiliates
                                   join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                   join s in db.SumaStatuses on a.statusid equals s.id
                                   join t in db.Types on a.typeid equals t.id
                                   join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                   join p in db.PrepaidCustomers on b.prepaidcustomerid equals p.id
                                   join tar in db.TARJETAS on a.id equals tar.NRO_AFILIACION
                                   where p.id == idCliente && tar.ESTATUS_TARJETA == estadoTarjeta
                                   select new ReportePrepago()
                                   {
                                       Beneficiario = new BeneficiarioPrepago()
                                       {
                                           Afiliado = new AfiliadoSuma()
                                           {
                                               docnumber = a.docnumber,
                                               name = c.NOMBRE_CLIENTE1,
                                               lastname1 = c.APELLIDO_CLIENTE1
                                           },
                                           Cliente = new ClientePrepago()
                                           {
                                               idCliente = idCliente,
                                               nameCliente = p.name
                                           }
                                       },
                                       fecha = tar.FECHA_CREACION.Value,
                                       numerotarjeta = tar.NRO_TARJETA,
                                       estatustarjeta = tar.ESTATUS_TARJETA,
                                       Encabezado = new EncabezadoReporte()
                                       {
                                           nombreReporte = "Reporte de Tarjetas",
                                           tipoconsultaReporte = "Por Cliente",
                                           parametrotipoconsultaReporte = p.rif + " " + p.name,
                                           fechainicioReporte = fechadesde,
                                           fechafinReporte = fechahasta,
                                           estatustarjetaReporte = estadoTarjeta
                                       }
                                   }).ToList();
                    }
                    if (reporte.Count() == 0)
                    {
                        ClientePrepagoRepository repCliente = new ClientePrepagoRepository();
                        ClientePrepago Cliente = repCliente.Find(idCliente);
                        ReportePrepago r = new ReportePrepago()
                        {
                            Encabezado = new EncabezadoReporte()
                            {
                                nombreReporte = "Reporte de Tarjetas",
                                tipoconsultaReporte = "Por Cliente",
                                parametrotipoconsultaReporte = Cliente.rifCliente + " " + Cliente.nameCliente,
                                fechainicioReporte = fechadesde,
                                fechafinReporte = fechahasta,
                                estatustarjetaReporte = estadoTarjeta
                            }
                        };
                        reporte.Add(r);
                    }
                }
                #endregion
                #region Todos los Clientes
                else if (tiporeporte == "todos")
                {
                    if (estadoTarjeta == "")
                    {
                        reporte = (from a in db.Affiliates
                                   join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                   join s in db.SumaStatuses on a.statusid equals s.id
                                   join t in db.Types on a.typeid equals t.id
                                   join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                   join p in db.PrepaidCustomers on b.prepaidcustomerid equals p.id
                                   join tar in db.TARJETAS on a.id equals tar.NRO_AFILIACION
                                   select new ReportePrepago()
                                   {
                                       Beneficiario = new BeneficiarioPrepago()
                                       {
                                           Afiliado = new AfiliadoSuma()
                                           {
                                               docnumber = a.docnumber,
                                               name = c.NOMBRE_CLIENTE1,
                                               lastname1 = c.APELLIDO_CLIENTE1
                                           },
                                           Cliente = new ClientePrepago()
                                           {
                                               idCliente = idCliente,
                                               nameCliente = p.name
                                           }
                                       },
                                       fecha = tar.FECHA_CREACION.Value,
                                       numerotarjeta = tar.NRO_TARJETA,
                                       estatustarjeta = tar.ESTATUS_TARJETA,
                                       Encabezado = new EncabezadoReporte()
                                       {
                                           nombreReporte = "Reporte de Tarjetas",
                                           tipoconsultaReporte = "Por Cliente",
                                           parametrotipoconsultaReporte = "Todos",
                                           fechainicioReporte = fechadesde,
                                           fechafinReporte = fechahasta,
                                           estatustarjetaReporte = "Todos"
                                       }
                                   }).ToList();
                    }
                    else
                    {
                        reporte = (from a in db.Affiliates
                                   join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                   join s in db.SumaStatuses on a.statusid equals s.id
                                   join t in db.Types on a.typeid equals t.id
                                   join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                   join p in db.PrepaidCustomers on b.prepaidcustomerid equals p.id
                                   join tar in db.TARJETAS on a.id equals tar.NRO_AFILIACION
                                   where tar.ESTATUS_TARJETA == estadoTarjeta
                                   select new ReportePrepago()
                                   {
                                       Beneficiario = new BeneficiarioPrepago()
                                       {
                                           Afiliado = new AfiliadoSuma()
                                           {
                                               docnumber = a.docnumber,
                                               name = c.NOMBRE_CLIENTE1,
                                               lastname1 = c.APELLIDO_CLIENTE1
                                           },
                                           Cliente = new ClientePrepago()
                                           {
                                               idCliente = idCliente,
                                               nameCliente = p.name
                                           }
                                       },
                                       fecha = tar.FECHA_CREACION.Value,
                                       numerotarjeta = tar.NRO_TARJETA,
                                       estatustarjeta = tar.ESTATUS_TARJETA,
                                       Encabezado = new EncabezadoReporte()
                                       {
                                           nombreReporte = "Reporte de Tarjetas",
                                           tipoconsultaReporte = "Por Cliente",
                                           parametrotipoconsultaReporte = "Todos",
                                           fechainicioReporte = fechadesde,
                                           fechafinReporte = fechahasta,
                                           estatustarjetaReporte = estadoTarjeta
                                       }
                                   }).ToList();

                    }
                    if (reporte.Count() == 0)
                    {
                        ClientePrepagoRepository repCliente = new ClientePrepagoRepository();
                        ClientePrepago Cliente = repCliente.Find(idCliente);
                        ReportePrepago r = new ReportePrepago()
                        {
                            Encabezado = new EncabezadoReporte()
                            {
                                nombreReporte = "Reporte de Tarjetas",
                                tipoconsultaReporte = "Por Cliente",
                                parametrotipoconsultaReporte = "Todos",
                                fechainicioReporte = fechadesde,
                                fechafinReporte = fechahasta,
                                estatustarjetaReporte = estadoTarjeta
                            }
                        };
                        reporte.Add(r);
                    }
                }
                #endregion
            }
            DateTime desde = DateTime.ParseExact(fechadesde, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime hasta = DateTime.ParseExact(fechahasta, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return reporte.Where(x => x.fecha.Date > desde && x.fecha.Date < hasta).OrderBy(x => x.fecha).ToList();
        }

        public List<ReportePrepago> ReporteTarjetasxBeneficiario(string tiporeporte, string fechadesde, string fechahasta, string numdoc = "", string estadoTarjeta = "")
        {
            List<ReportePrepago> reporte = new List<ReportePrepago>();
            string textoparametro;
            using (LealtadEntities db = new LealtadEntities())
            {
                #region Por Beneficiario específico
                if (tiporeporte == "uno")
                {
                     List<BeneficiarioPrepago> beneficiarios = Find(numdoc, "", "", "", "");
                     if (beneficiarios.Count == 0)
                     {
                         textoparametro = numdoc;
                     }
                     else
                     {
                         textoparametro = beneficiarios.First().Afiliado.docnumber + " " + beneficiarios.First().Afiliado.name + " " + beneficiarios.First().Afiliado.lastname1;
                         if (estadoTarjeta == "")
                         {
                             reporte = (from a in db.Affiliates
                                        join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                        join s in db.SumaStatuses on a.statusid equals s.id
                                        join t in db.Types on a.typeid equals t.id
                                        join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                        join p in db.PrepaidCustomers on b.prepaidcustomerid equals p.id
                                        join tar in db.TARJETAS on a.id equals tar.NRO_AFILIACION
                                        where a.docnumber == numdoc
                                        select new ReportePrepago()
                                        {
                                            Beneficiario = new BeneficiarioPrepago()
                                            {
                                                Afiliado = new AfiliadoSuma()
                                                {
                                                    docnumber = a.docnumber,
                                                    name = c.NOMBRE_CLIENTE1,
                                                    lastname1 = c.APELLIDO_CLIENTE1
                                                },
                                                Cliente = new ClientePrepago()
                                                {
                                                    idCliente = p.id,
                                                    nameCliente = p.name
                                                }
                                            },
                                            fecha = tar.FECHA_CREACION.Value,
                                            numerotarjeta = tar.NRO_TARJETA,
                                            estatustarjeta = tar.ESTATUS_TARJETA,
                                            Encabezado = new EncabezadoReporte()
                                            {
                                                nombreReporte = "Reporte de Tarjetas",
                                                tipoconsultaReporte = "Por Beneficiario",
                                                parametrotipoconsultaReporte = textoparametro,
                                                fechainicioReporte = fechadesde,
                                                fechafinReporte = fechahasta,
                                                estatustarjetaReporte = "Todos"
                                            }
                                        }).ToList();
                         }
                         else
                         {
                             reporte = (from a in db.Affiliates
                                        join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                        join s in db.SumaStatuses on a.statusid equals s.id
                                        join t in db.Types on a.typeid equals t.id
                                        join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                        join p in db.PrepaidCustomers on b.prepaidcustomerid equals p.id
                                        join tar in db.TARJETAS on a.id equals tar.NRO_AFILIACION
                                        where a.docnumber == numdoc && tar.ESTATUS_TARJETA == estadoTarjeta
                                        select new ReportePrepago()
                                        {
                                            Beneficiario = new BeneficiarioPrepago()
                                            {
                                                Afiliado = new AfiliadoSuma()
                                                {
                                                    docnumber = a.docnumber,
                                                    name = c.NOMBRE_CLIENTE1,
                                                    lastname1 = c.APELLIDO_CLIENTE1
                                                },
                                                Cliente = new ClientePrepago()
                                                {
                                                    idCliente = p.id,
                                                    nameCliente = p.name
                                                }
                                            },
                                            fecha = tar.FECHA_CREACION.Value,
                                            numerotarjeta = tar.NRO_TARJETA,
                                            estatustarjeta = tar.ESTATUS_TARJETA,
                                            Encabezado = new EncabezadoReporte()
                                            {
                                                nombreReporte = "Reporte de Tarjetas",
                                                tipoconsultaReporte = "Por Beneficiario",
                                                parametrotipoconsultaReporte = textoparametro,
                                                fechainicioReporte = fechadesde,
                                                fechafinReporte = fechahasta,
                                                estatustarjetaReporte = estadoTarjeta
                                            }
                                        }).ToList();
                         }
                    }
                    if (reporte.Count() == 0)
                    {
                        ReportePrepago r = new ReportePrepago()
                        {
                            Encabezado = new EncabezadoReporte()
                            {
                                nombreReporte = "Reporte de Tarjetas",
                                tipoconsultaReporte = "Por Beneficiario",
                                parametrotipoconsultaReporte = textoparametro,
                                fechainicioReporte = fechadesde,
                                fechafinReporte = fechahasta,
                                estatustarjetaReporte = estadoTarjeta
                            }
                        };
                        reporte.Add(r);
                    }
                }
                #endregion
                #region Todos los Beneficiarios
                else if (tiporeporte == "todos")
                {
                    if (estadoTarjeta == "")
                    {
                        reporte = (from a in db.Affiliates
                                   join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                   join s in db.SumaStatuses on a.statusid equals s.id
                                   join t in db.Types on a.typeid equals t.id
                                   join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                   join p in db.PrepaidCustomers on b.prepaidcustomerid equals p.id
                                   join tar in db.TARJETAS on a.id equals tar.NRO_AFILIACION
                                   select new ReportePrepago()
                                   {
                                       Beneficiario = new BeneficiarioPrepago()
                                       {
                                           Afiliado = new AfiliadoSuma()
                                           {
                                               docnumber = a.docnumber,
                                               name = c.NOMBRE_CLIENTE1,
                                               lastname1 = c.APELLIDO_CLIENTE1
                                           },
                                           Cliente = new ClientePrepago()
                                           {
                                               idCliente = p.id,
                                               nameCliente = p.name
                                           }
                                       },
                                       fecha = tar.FECHA_CREACION.Value,
                                       numerotarjeta = tar.NRO_TARJETA,
                                       estatustarjeta = tar.ESTATUS_TARJETA,
                                       Encabezado = new EncabezadoReporte()
                                       {
                                           nombreReporte = "Reporte de Tarjetas",
                                           tipoconsultaReporte = "Por Beneficiario",
                                           parametrotipoconsultaReporte = "Todos",
                                           fechainicioReporte = fechadesde,
                                           fechafinReporte = fechahasta,
                                           estatustarjetaReporte = "Todos"
                                       }
                                   }).ToList();
                    }
                    else
                    {
                        reporte = (from a in db.Affiliates
                                   join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                   join s in db.SumaStatuses on a.statusid equals s.id
                                   join t in db.Types on a.typeid equals t.id
                                   join b in db.PrepaidBeneficiaries on a.id equals b.affiliateid
                                   join p in db.PrepaidCustomers on b.prepaidcustomerid equals p.id
                                   join tar in db.TARJETAS on a.id equals tar.NRO_AFILIACION
                                   where tar.ESTATUS_TARJETA == estadoTarjeta
                                   select new ReportePrepago()
                                   {
                                       Beneficiario = new BeneficiarioPrepago()
                                       {
                                           Afiliado = new AfiliadoSuma()
                                           {
                                               docnumber = a.docnumber,
                                               name = c.NOMBRE_CLIENTE1,
                                               lastname1 = c.APELLIDO_CLIENTE1
                                           },
                                           Cliente = new ClientePrepago()
                                           {
                                               idCliente = p.id,
                                               nameCliente = p.name
                                           }
                                       },
                                       fecha = tar.FECHA_CREACION.Value,
                                       numerotarjeta = tar.NRO_TARJETA,
                                       estatustarjeta = tar.ESTATUS_TARJETA,
                                       Encabezado = new EncabezadoReporte()
                                       {
                                           nombreReporte = "Reporte de Tarjetas",
                                           tipoconsultaReporte = "Por Beneficiario",
                                           parametrotipoconsultaReporte = "Todos",
                                           fechainicioReporte = fechadesde,
                                           fechafinReporte = fechahasta,
                                           estatustarjetaReporte = estadoTarjeta
                                       }
                                   }).ToList();

                    }
                    if (reporte.Count() == 0)
                    {
                        ReportePrepago r = new ReportePrepago()
                        {
                            Encabezado = new EncabezadoReporte()
                            {
                                nombreReporte = "Reporte de Tarjetas",
                                tipoconsultaReporte = "Por Beneficiario",
                                parametrotipoconsultaReporte = "Todos",
                                fechainicioReporte = fechadesde,
                                fechafinReporte = fechahasta,
                                estatustarjetaReporte = estadoTarjeta
                            }
                        };
                        reporte.Add(r);
                    }
                }
                #endregion
            }
            DateTime desde = DateTime.ParseExact(fechadesde, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime hasta = DateTime.ParseExact(fechahasta, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return reporte.Where(x => x.fecha.Date > desde && x.fecha.Date < hasta).OrderBy(x => x.fecha).ToList();
        }

        /**
         * 
         * Prepago : Transacciones de Compra Fuera de Línea.
         * Función : public bool CompraFueraLinea(string numdoc, string monto)
         * 
         **/
        public bool CompraFueraLinea(string numdoc, string monto)
        {
            //string montoSinSeparador = Math.Truncate(Convert.ToDecimal(monto) * 100).ToString();
            string RespuestaCardsJson = WSL.Cards.addBatch(numdoc, monto, TRANSCODE_COMPRA_PREPAGO, (string)HttpContext.Current.Session["login"]);
            if (WSL.Cards.ExceptionServicioCards(RespuestaCardsJson))
            {
                return false;
            }
            RespuestaCards RespuestaCards = (RespuestaCards)JsonConvert.DeserializeObject<RespuestaCards>(RespuestaCardsJson);
            if (RespuestaCards.excode == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}