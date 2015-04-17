using Newtonsoft.Json;
using Suma2Lealtad.Modules;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class AfiliadoRepository
    {
        private int INITIAL_INTEGER_VALUE = 1;

        private int ESTATUS_ID_INICIAL = 0;
        private int REASONS_ID_INICIAL = 1;
        private int ID_CORPORACION_PLAZAS = 1;
        private int ID_ESTATUS_ACTIVA = 2;
        private int ID_TYPE_SUMA = 1;
        private int ID_TYPE_PREPAGO = 2;

        private string ID_ESTATUS_TARJETA_NUEVA = "0";
        private string ID_ESTATUS_TARJETA_ACTIVA = "1";
        private string ID_ESTATUS_TARJETA_SUSPENDIDA = "6";

        //private string INITIAL_STRING_VALUE = "";
        //public AfiliadoRepository() { }

        public class customerInterest
        {
            public int customerID { get; set; }
            public int interestID { get; set; }
        }

        #region InterestList
        private List<Interest> chargeInterestList()
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                return db.Interests.Where(x => x.active == true).ToList();
            }
        }


        private List<Interest> chargeInterestList(int customerID)
        {
            using (LealtadEntities db = new LealtadEntities())
            {

                List<customerInterest> records = (from i in db.CustomerInterests
                                                  where i.customerid == customerID
                                                  select new customerInterest()
                                                  {
                                                      customerID = i.customerid,
                                                      interestID = i.interestid
                                                  }).ToList();

                var lista = new List<Interest>();

                foreach (var items in db.Interests.Where(x => x.active == true))
                {

                    foreach (var customerItem in records)
                    {
                        if (items.id.Equals(customerItem.interestID))
                        {
                            items.Checked = true;
                            break;
                        }

                    }

                    lista.Add(items);
                }

                return lista;

            }
        }
        #endregion

        #region sequenceID
        private int AfilliatesID()
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                if (db.Affiliates.Count() == 0)
                    return 1;
                return (db.Affiliates.Max(a => a.id) + 1);
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
        #endregion

        public Afiliado Find(string numdoc)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                //Primero se buscan los datos de AFILIADO en SumaPlazas
                //ENTIDAD Affiliado
                //ENTIDAD CLIENTE
                //ENTIDAD Status
                //ENTIDAD CustomerInterest
                //ENTIDAD Photos_Affiliate, FILEYSTEM ~/Picture/@filename@.jpg                
                Afiliado afiliado = (from a in db.Affiliates
                                     join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                     join s in db.Status on a.statusid equals s.id
                                     where a.docnumber.Equals(numdoc)
                                     select new Afiliado()
                                     {
                                         //ENTIDAD Affiliate 
                                         id = a.id == null ? 0 : (int)a.id,
                                         customerid = a.customerid, // (No se a que corresponde)                	 
                                         docnumber = a.docnumber, // +<*    
                                         clientid = a.clientid == null ? 0 : (int)a.clientid, // +(corresponde al id de WEBPLAZAS)*
                                         storeid = a.storeid,
                                         channelid = a.channelid,
                                         typeid = a.typeid,
                                         typedelivery = a.typedelivery,
                                         storeiddelivery = a.storeiddelivery == null ? 0 : (int)a.storeiddelivery,
                                         statusid = a.statusid,
                                         reasonsid = a.reasonsid == null ? 1 : (int) a.reasonsid,
                                         twitter_account = a.twitter_account, // +
                                         facebook_account = a.facebook_account, // +
                                         instagram_account = a.instagram_account, // +
                                         comments = a.comments,
                                         //ENTIDAD CLIENTE
                                         cod_estado = c.COD_ESTADO,
                                         cod_ciudad = c.COD_CIUDAD,
                                         cod_municipio = c.COD_MUNICIPIO,
                                         cod_parroquia = c.COD_PARROQUIA,
                                         cod_urbanizacion = c.COD_URBANIZACION,
                                         //ENTIDAD Status
                                         estatus = s.name,
                                     }).SingleOrDefault();
                if (afiliado == null)
                {
                    afiliado = new Afiliado();
                    afiliado.id = 0;
                    afiliado.docnumber = numdoc;
                }
                //ENTIDAD CustomerInterest
                afiliado.Intereses = chargeInterestList(afiliado.id);
                //ENTIDAD Photos_Affiliate, FILEYSTEM ~/Picture/@filename@.jpg
                afiliado.picture = AppModule.GetPathPicture().Replace("@filename@", afiliado.docnumber);
                //Segundo se buscan los datos de CLIENTE en WebPlazas
                //SERVICIO WSL.WebPlazas.getClientByNumDoc                
                string clienteWebPlazasJson = WSL.WebPlazas.getClientByNumDoc(numdoc);
                ClienteWebPlazas clienteWebPlazas = (ClienteWebPlazas)JsonConvert.DeserializeObject<ClienteWebPlazas>(clienteWebPlazasJson);
                afiliado.nationality = clienteWebPlazas.nationality; // +*
                afiliado.name = clienteWebPlazas.name; // +<*
                afiliado.name2 = clienteWebPlazas.name2; // +<*
                afiliado.lastname1 = clienteWebPlazas.lastname1; // +<*
                afiliado.lastname2 = clienteWebPlazas.lastname2; // +<*
                afiliado.birthdate = clienteWebPlazas.birthdate.Value.ToString("dd-MM-yyyy"); // +*
                afiliado.gender = clienteWebPlazas.gender; //+*
                afiliado.clientid = clienteWebPlazas.id; //
                afiliado.maritalstatus = clienteWebPlazas.maritalstatus; // +*
                afiliado.occupation = clienteWebPlazas.occupation; // +*
                afiliado.phone1 = clienteWebPlazas.phone1; // +<*
                afiliado.phone2 = clienteWebPlazas.phone2; // +*
                afiliado.phone3 = clienteWebPlazas.phone3; // +*
                afiliado.email = clienteWebPlazas.email; // +*
                afiliado.type = clienteWebPlazas.type; // +*            

                //Los estados actuales para una persona son:
                //NOCLIENTE            (no registrado en WEBPLAZAS)
                //NOAFILIADO           (no afiliado en SUMAPLAZAS)
                //CLIENTE              (registrado en WEBPLAZAS)
                //AFILIADO             (afiliado en SUMAPLAZAS)
                //El estado deseado es:
                //AFILIADO/CLIENTE     (registrado en WEBPLAZAS y afiliado en SUMAPLAZAS) 
                //Existen 4 resultados posibles para esta búsqueda
                //NOCLIENTE/NOAFILIADO -> por definir acción para crear registro de CLIENTE y crear afiliación de AFILIADO => Redireccionar a GenericView con mensaje descriptivo
                //NOCLIENTE/AFILIADO   -> por definir acción para crear registro de CLIENTE => Redireccionar a GenericView con mensaje descriptivo
                //CLIENTE/NOAFILIADO   -> acción: editar registro de CLIENTE y crear afiliación de AFILIADO => CREAR AFILIACION (retornar vista Create)
                //CLIENTE/AFILIADO     -> acción: editar registro de CLIENTE y editar afiliación de AFILIADO => REVISAR AFILIACION (Redirecciónar a acción Index ó Edit)

                if (afiliado.clientid == 0 && afiliado.id == 0)
                {
                    //NOCLIENTE/NOAFILIADO
                }
                else if (afiliado.clientid == 0 && afiliado.id != 0)
                {
                    //NOCLIENTE/AFILIADO
                }
                else if (afiliado.clientid != 0 && afiliado.id == 0)
                {
                    //CLIENTE/NOAFILIADO                    
                }
                else if (afiliado.clientid != 0 && afiliado.id != 0)
                {
                    //CLIENTE/AFILIADO
                    //Tercero se buscan los datos de Tarjeta de AFILIADO en Cards
                    //SERVICIO WSL.Cards.getClient !
                    string clienteCardsJson = WSL.Cards.getClient(afiliado.docnumber.Substring(2));
                    ClienteCards clienteCards = (ClienteCards)JsonConvert.DeserializeObject<ClienteCards>(clienteCardsJson);
                    afiliado.pan = clienteCards.pan; // !
                    afiliado.printed = clienteCards.printed; // !
                    afiliado.estatustarjeta = clienteCards.tarjeta; //afiliado.estatustarjeta = clienteCards.estatus == "1" ? "Activa" : "Inactiva"; // ! 
                }
                return afiliado;
            }
        }

        public Afiliado Find(int id)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                //Primero se buscan los datos de AFILIADO en SumaPlazas
                //ENTIDAD Affiliado
                //ENTIDAD CLIENTE
                //ENTIDAD Status
                //ENTIDAD CustomerInterest
                //ENTIDAD Photos_Affiliate, FILEYSTEM ~/Picture/@filename@.jpg                
                Afiliado afiliado = (from a in db.Affiliates
                                     join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                     join s in db.Status on a.statusid equals s.id
                                     where a.id.Equals(id)
                                     select new Afiliado()
                                     {
                                         //ENTIDAD Affiliate 
                                         id = a.id == null ? 0 : (int)a.id,
                                         customerid = a.customerid, // (No se a que corresponde)                	 
                                         docnumber = a.docnumber, // +<*    
                                         clientid = a.clientid == null ? 0 : (int)a.clientid, // +(corresponde al id de WEBPLAZAS)*
                                         storeid = a.storeid,
                                         channelid = a.channelid,
                                         typeid = a.typeid,
                                         typedelivery = a.typedelivery,
                                         storeiddelivery = a.storeiddelivery == null ? 0 : (int)a.storeiddelivery,
                                         statusid = a.statusid,
                                         reasonsid = a.reasonsid == null ? 1 : (int)a.reasonsid,
                                         twitter_account = a.twitter_account, // +
                                         facebook_account = a.facebook_account, // +
                                         instagram_account = a.instagram_account, // +
                                         comments = a.comments,
                                         //ENTIDAD CLIENTE
                                         cod_estado = c.COD_ESTADO,
                                         cod_ciudad = c.COD_CIUDAD,
                                         cod_municipio = c.COD_MUNICIPIO,
                                         cod_parroquia = c.COD_PARROQUIA,
                                         cod_urbanizacion = c.COD_URBANIZACION,
                                         //ENTIDAD Status
                                         estatus = s.name,
                                     }).Single();
                if (afiliado != null)
                {
                    //ENTIDAD CustomerInterest
                    afiliado.Intereses = chargeInterestList(afiliado.id);
                    //ENTIDAD Photos_Affiliate, FILEYSTEM ~/Picture/@filename@.jpg
                    afiliado.picture = AppModule.GetPathPicture().Replace("@filename@", afiliado.docnumber);
                }
                else
                {
                    afiliado = new Afiliado();
                    afiliado.id = 0;
                    afiliado.docnumber = "";
                }
                //Segundo se buscan los datos de CLIENTE en WebPlazas
                //SERVICIO WSL.WebPlazas.getClientByNumDoc                
                string clienteWebPlazasJson = WSL.WebPlazas.getClientByNumDoc(afiliado.docnumber);
                ClienteWebPlazas clienteWebPlazas = (ClienteWebPlazas)JsonConvert.DeserializeObject<ClienteWebPlazas>(clienteWebPlazasJson);
                afiliado.nationality = clienteWebPlazas.nationality; // +*
                afiliado.name = clienteWebPlazas.name; // +<*
                afiliado.name2 = clienteWebPlazas.name2; // +<*
                afiliado.lastname1 = clienteWebPlazas.lastname1; // +<*
                afiliado.lastname2 = clienteWebPlazas.lastname2; // +<*
                afiliado.birthdate = clienteWebPlazas.birthdate.Value.ToString("dd-MM-yyyy"); ; // +*
                afiliado.gender = clienteWebPlazas.gender; //+*
                afiliado.clientid = clienteWebPlazas.id; //
                afiliado.maritalstatus = clienteWebPlazas.maritalstatus; // +*
                afiliado.occupation = clienteWebPlazas.occupation; // +*
                afiliado.phone1 = clienteWebPlazas.phone1; // +<*
                afiliado.phone2 = clienteWebPlazas.phone2; // +*
                afiliado.phone3 = clienteWebPlazas.phone3; // +*
                afiliado.email = clienteWebPlazas.email; // +*
                afiliado.type = clienteWebPlazas.type; // +*             

                //Los estados actuales para una persona son:
                //NOCLIENTE            (no registrado en WEBPLAZAS)
                //NOAFILIADO           (no afiliado en SUMAPLAZAS)
                //CLIENTE              (registrado en WEBPLAZAS)
                //AFILIADO             (afiliado en SUMAPLAZAS)
                //El estado deseado es:
                //AFILIADO/CLIENTE     (registrado en WEBPLAZAS y afiliado en SUMAPLAZAS) 
                //Existen 4 resultados posibles para esta búsqueda
                //NOCLIENTE/NOAFILIADO -> por definir acción para crear registro de CLIENTE y crear afiliación de AFILIADO => Redireccionar a GenericView con mensaje descriptivo
                //NOCLIENTE/AFILIADO   -> por definir acción para crear registro de CLIENTE => Redireccionar a GenericView con mensaje descriptivo
                //CLIENTE/NOAFILIADO   -> acción: editar registro de CLIENTE y crear afiliación de AFILIADO => CREAR AFILIACION (retornar vista Create)
                //CLIENTE/AFILIADO     -> acción: editar registro de CLIENTE y editar afiliación de AFILIADO => REVISAR AFILIACION (Redirecciónar a acción Index ó Edit)

                if (afiliado.clientid == 0 && afiliado.id == 0)
                {
                    //NOCLIENTE/NOAFILIADO
                }
                else if (afiliado.clientid == 0 && afiliado.id != 0)
                {
                    //NOCLIENTE/AFILIADO
                }
                else if (afiliado.clientid != 0 && afiliado.id == 0)
                {
                    //CLIENTE/NOAFILIADO                    
                }
                else if (afiliado.clientid != 0 && afiliado.id != 0)
                {
                    //CLIENTE/AFILIADO
                    //Tercero se buscan los datos de Tarjeta de AFILIADO en Cards
                    //SERVICIO WSL.Cards.getClient !
                    string clienteCardsJson = WSL.Cards.getClient(afiliado.docnumber.Substring(2));
                    ClienteCards clienteCards = (ClienteCards)JsonConvert.DeserializeObject<ClienteCards>(clienteCardsJson);
                    afiliado.pan = clienteCards.pan; // !
                    afiliado.printed = clienteCards.printed; // !
                    afiliado.estatustarjeta = clienteCards.tarjeta; //afiliado.estatustarjeta = clienteCards.estatus == "1" ? "Activa" : "Inactiva"; // !                   
                }
                return afiliado;
            }
        }

        public List<Afiliado> Find(string numdoc, string name, string email)
        {
            if (name == "")
            {
                name = null;
            }
            if (email == "")
            {
                email = null;
            }
            using (LealtadEntities db = new LealtadEntities())
            {
                //Primero se buscan los datos de AFILIADO en SumaPlazas
                //ENTIDAD Affiliado
                //ENTIDAD CLIENTE
                //ENTIDAD Status
                //ENTIDAD CustomerInterest
                //ENTIDAD Photos_Affiliate, FILEYSTEM ~/Picture/@filename@.jpg
                List<Afiliado> afiliados = (from a in db.Affiliates
                                            join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                            join s in db.Status on a.statusid equals s.id
                                            where a.docnumber.Equals(numdoc) || c.E_MAIL == email || c.NOMBRE_CLIENTE1.Contains(name) || c.APELLIDO_CLIENTE1.Contains(name)
                                            select new Afiliado()
                                            {
                                                //ENTIDAD Affiliate 
                                                id = a.id == null ? 0 : (int)a.id,
                                                customerid = a.customerid, // (No se a que corresponde)                	 
                                                docnumber = a.docnumber, // +<*    
                                                clientid = a.clientid == null ? 0 : (int)a.clientid, // +(corresponde al id de WEBPLAZAS)*
                                                storeid = a.storeid,
                                                channelid = a.channelid,
                                                typeid = a.typeid,
                                                typedelivery = a.typedelivery,
                                                storeiddelivery = a.storeiddelivery == null ? 0 : (int)a.storeiddelivery,
                                                statusid = a.statusid,
                                                reasonsid = a.reasonsid == null ? 1 : (int)a.reasonsid,
                                                twitter_account = a.twitter_account, // +
                                                facebook_account = a.facebook_account, // +
                                                instagram_account = a.instagram_account, // +
                                                comments = a.comments,
                                                //ENTIDAD CLIENTE
                                                cod_estado = c.COD_ESTADO,
                                                cod_ciudad = c.COD_CIUDAD,
                                                cod_municipio = c.COD_MUNICIPIO,
                                                cod_parroquia = c.COD_PARROQUIA,
                                                cod_urbanizacion = c.COD_URBANIZACION,
                                                //ENTIDAD Status
                                                estatus = s.name,
                                            }).ToList();
                if (afiliados != null)
                {
                    foreach (var afiliado in afiliados)
                    {
                        //ENTIDAD CustomerInterest
                        afiliado.Intereses = chargeInterestList(afiliado.id);
                        //ENTIDAD Photos_Affiliate, FILEYSTEM ~/Picture/@filename@.jpg
                        afiliado.picture = AppModule.GetPathPicture().Replace("@filename@", afiliado.docnumber);
                        //Segundo se buscan los datos de CLIENTE en WebPlazas
                        //SERVICIO WSL.WebPlazas.getClientByNumDoc                
                        string clienteWebPlazasJson = WSL.WebPlazas.getClientByNumDoc(afiliado.docnumber);
                        ClienteWebPlazas clienteWebPlazas = (ClienteWebPlazas)JsonConvert.DeserializeObject<ClienteWebPlazas>(clienteWebPlazasJson);
                        afiliado.nationality = clienteWebPlazas.nationality; // +*
                        afiliado.name = clienteWebPlazas.name; // +<*
                        afiliado.name2 = clienteWebPlazas.name2; // +<*
                        afiliado.lastname1 = clienteWebPlazas.lastname1; // +<*
                        afiliado.lastname2 = clienteWebPlazas.lastname2; // +<*
                        afiliado.birthdate = clienteWebPlazas.birthdate.Value.ToString("dd-MM-yyyy"); // +*
                        afiliado.gender = clienteWebPlazas.gender; //+*
                        afiliado.clientid = clienteWebPlazas.id; //
                        afiliado.maritalstatus = clienteWebPlazas.maritalstatus; // +*
                        afiliado.occupation = clienteWebPlazas.occupation; // +*
                        afiliado.phone1 = clienteWebPlazas.phone1; // +<*
                        afiliado.phone2 = clienteWebPlazas.phone2; // +*
                        afiliado.phone3 = clienteWebPlazas.phone3; // +*
                        afiliado.email = clienteWebPlazas.email; // +*
                        afiliado.type = clienteWebPlazas.type; // +*             

                        //Los estados actuales para una persona son:
                        //NOCLIENTE            (no registrado en WEBPLAZAS)
                        //NOAFILIADO           (no afiliado en SUMAPLAZAS)
                        //CLIENTE              (registrado en WEBPLAZAS)
                        //AFILIADO             (afiliado en SUMAPLAZAS)
                        //El estado deseado es:
                        //AFILIADO/CLIENTE     (registrado en WEBPLAZAS y afiliado en SUMAPLAZAS) 
                        //Existen 4 resultados posibles para esta búsqueda
                        //NOCLIENTE/NOAFILIADO -> por definir acción para crear registro de CLIENTE y crear afiliación de AFILIADO => Redireccionar a GenericView con mensaje descriptivo
                        //NOCLIENTE/AFILIADO   -> por definir acción para crear registro de CLIENTE => Redireccionar a GenericView con mensaje descriptivo
                        //CLIENTE/NOAFILIADO   -> acción: editar registro de CLIENTE y crear afiliación de AFILIADO => CREAR AFILIACION (retornar vista Create)
                        //CLIENTE/AFILIADO     -> acción: editar registro de CLIENTE y editar afiliación de AFILIADO => REVISAR AFILIACION (Redirecciónar a acción Index ó Edit)

                        if (afiliado.clientid == 0 && afiliado.id == 0)
                        {
                            //NOCLIENTE/NOAFILIADO
                        }
                        else if (afiliado.clientid == 0 && afiliado.id != 0)
                        {
                            //NOCLIENTE/AFILIADO
                        }
                        else if (afiliado.clientid != 0 && afiliado.id == 0)
                        {
                            //CLIENTE/NOAFILIADO                    
                        }
                        else if (afiliado.clientid != 0 && afiliado.id != 0)
                        {
                            //CLIENTE/AFILIADO
                            //Tercero se buscan los datos de Tarjeta de AFILIADO en Cards
                            //SERVICIO WSL.Cards.getClient !
                            string clienteCardsJson = WSL.Cards.getClient(afiliado.docnumber.Substring(2));
                            ClienteCards clienteCards = (ClienteCards)JsonConvert.DeserializeObject<ClienteCards>(clienteCardsJson);
                            afiliado.pan = clienteCards.pan; // !
                            afiliado.printed = clienteCards.printed; // !
                            afiliado.estatustarjeta = clienteCards.tarjeta; //afiliado.estatustarjeta = clienteCards.estatus == "1" ? "Activa" : "Inactiva"; // ! 
                        }
                    }
                }
                return afiliados;
            }
        }

        private bool SaveWebPlazas(Afiliado afiliado)
        {
            RespuestaWebPlazas RespuestaWebPlazas = new RespuestaWebPlazas();
            string RespuestaWebPlazasJson = WSL.WebPlazas.UpdateClient(afiliado);
            RespuestaWebPlazas = (RespuestaWebPlazas)JsonConvert.DeserializeObject<RespuestaWebPlazas>(RespuestaWebPlazasJson);
            return (RespuestaWebPlazas.id == "0");
        }

        public bool Save(Afiliado afiliado)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                //ENTIDAD Affiliatte                   
                var Affiliate = new Affiliate()
                {
                    id = AfilliatesID(),
                    customerid = afiliado.customerid,
                    docnumber = afiliado.docnumber,
                    clientid = afiliado.clientid,
                    storeid = afiliado.storeid,
                    channelid = afiliado.channelid,
                    typeid = ID_TYPE_SUMA,//afiliado.typeid, //INITIAL_INTEGER_VALUE,
                    affiliatedate = System.DateTime.Now,
                    typedelivery = afiliado.typedelivery,
                    storeiddelivery = afiliado.storeiddelivery,
                    estimateddatedelivery = new DateTime(),
                    creationdate = DateTime.Now,
                    creationuserid = (int)HttpContext.Current.Session["userid"],
                    modifieddate = DateTime.Now,
                    modifieduserid = (int)HttpContext.Current.Session["userid"],
                    statusid = ESTATUS_ID_INICIAL,
                    reasonsid = REASONS_ID_INICIAL,
                    twitter_account = afiliado.twitter_account,
                    facebook_account = afiliado.facebook_account,
                    instagram_account = afiliado.instagram_account,
                    comments = afiliado.comments
                };
                db.Affiliates.Add(Affiliate);
                //ENTIDAD CLIENTE
                var CLIENTE = new CLIENTE()
                {
                    TIPO_DOCUMENTO = afiliado.docnumber.Substring(0, 1),
                    NRO_DOCUMENTO = afiliado.docnumber.Substring(2),
                    NACIONALIDAD = afiliado.nationality,
                    NOMBRE_CLIENTE1 = afiliado.name,
                    NOMBRE_CLIENTE2 = afiliado.name2 == null ? "" : afiliado.name2,
                    APELLIDO_CLIENTE1 = afiliado.lastname1,
                    APELLIDO_CLIENTE2 = afiliado.lastname2 == null ? "" : afiliado.lastname2,
                    FECHA_NACIMIENTO = DateTime.ParseExact(afiliado.birthdate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    SEXO = afiliado.gender,
                    EDO_CIVIL = afiliado.maritalstatus,
                    OCUPACION = afiliado.occupation == null ? "" : afiliado.occupation,
                    TELEFONO_HAB = afiliado.phone1,
                    TELEFONO_OFIC = afiliado.phone2 == null ? "" : afiliado.cod_estado,
                    TELEFONO_CEL = afiliado.phone3 == null ? "" : afiliado.cod_estado,
                    E_MAIL = afiliado.email,
                    COD_SUCURSAL = afiliado.storeid,
                    COD_ESTADO = afiliado.cod_estado == null ? "0" : afiliado.cod_estado,
                    COD_CIUDAD = afiliado.cod_ciudad == null ? "0" : afiliado.cod_ciudad,
                    COD_MUNICIPIO = afiliado.cod_municipio == null ? "0" : afiliado.cod_municipio,
                    COD_PARROQUIA = afiliado.cod_parroquia == null ? "0" : afiliado.cod_parroquia,
                    COD_URBANIZACION = afiliado.cod_urbanizacion == null ? "0" : afiliado.cod_urbanizacion,
                    FECHA_CREACION = DateTime.Now
                };
                db.CLIENTES.Add(CLIENTE);
                //ENTIDAD CustomerInterest
                foreach (var interes in afiliado.Intereses.Where(x => x.Checked == true))
                {
                    CustomerInterest customerInterest = new CustomerInterest()
                    {
                        customerid = Affiliate.id,
                        interestid = interes.id,
                        comments = ""
                    };
                    db.CustomerInterests.Add(customerInterest);
                }
                //ENTIDAD Photos_Affiliate, FILEYSTEM ~/Picture/@filename@.jpg 
                //var Photos_Affiliate = new Photos_Affiliate();
                //ENTIDAD CompanyAffiliate
                var companyaffiliate = new CompanyAffiliate()
                {
                    affiliateid = Affiliate.id,
                    companyid = ID_CORPORACION_PLAZAS,
                    begindate = DateTime.Now,
                    enddate = new DateTime(),
                    comments = afiliado.comments,
                    active = true
                };
                db.CompanyAffiliates.Add(companyaffiliate);
                //ENTIDAD AffiliateAud
                var affiliateauditoria = new AffiliateAud()
                {
                    id = AfilliateAudID(),
                    affiliateid = Affiliate.id,
                    modifieduserid = (int)HttpContext.Current.Session["userid"],
                    modifieddate = System.DateTime.Now,
                    statusid = Affiliate.statusid,
                    reasonsid = Affiliate.reasonsid == null ? 1 : (int)Affiliate.reasonsid,
                    comments = afiliado.comments
                };
                db.AffiliateAuds.Add(affiliateauditoria);
                if (SaveWebPlazas(afiliado))
                {
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool SaveChanges(Afiliado afiliado)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                // Entidad : Affiliate
                Affiliate affiliate = db.Affiliates.FirstOrDefault(a => a.id == afiliado.id);
                if (affiliate != null)
                {
                    affiliate.storeid = afiliado.storeiddelivery;
                    affiliate.channelid = afiliado.channelid;
                    affiliate.typeid = afiliado.typeid;
                    affiliate.typedelivery = afiliado.typedelivery;
                    affiliate.storeiddelivery = afiliado.storeiddelivery;
                    //affiliate.estimateddatedelivery = System.DateTime.Now;
                    affiliate.modifieduserid = (int)HttpContext.Current.Session["userid"];
                    affiliate.modifieddate = System.DateTime.Now;
                    affiliate.statusid = afiliado.statusid;
                    affiliate.reasonsid = afiliado.reasonsid;
                    affiliate.twitter_account = afiliado.twitter_account;
                    affiliate.facebook_account = afiliado.facebook_account;
                    affiliate.instagram_account = afiliado.instagram_account;
                    affiliate.comments = afiliado.comments;
                }

                // Entidad : Cliente 
                CLIENTE cliente = db.CLIENTES.FirstOrDefault(c => c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO == afiliado.docnumber);
                if (cliente != null)
                {
                    cliente.NACIONALIDAD = afiliado.nationality;
                    cliente.NOMBRE_CLIENTE1 = afiliado.name;
                    cliente.NOMBRE_CLIENTE2 = afiliado.name2;
                    cliente.APELLIDO_CLIENTE1 = afiliado.lastname1;
                    cliente.APELLIDO_CLIENTE2 = afiliado.lastname2;
                    cliente.FECHA_NACIMIENTO = DateTime.ParseExact(afiliado.birthdate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    cliente.SEXO = afiliado.gender;
                    cliente.EDO_CIVIL = afiliado.maritalstatus;
                    cliente.OCUPACION = afiliado.occupation;
                    cliente.TELEFONO_HAB = afiliado.phone1;
                    cliente.TELEFONO_OFIC = afiliado.phone2;
                    cliente.TELEFONO_CEL = afiliado.phone3;
                    cliente.COD_SUCURSAL = afiliado.storeid;
                    cliente.COD_ESTADO = afiliado.cod_estado;
                    cliente.COD_CIUDAD = afiliado.cod_ciudad;
                    cliente.COD_MUNICIPIO = afiliado.cod_municipio;
                    cliente.COD_PARROQUIA = afiliado.cod_parroquia;
                    cliente.COD_URBANIZACION = afiliado.cod_urbanizacion;
                }

                // Entidad : Status
                Status status = db.Status.FirstOrDefault(s => s.id == afiliado.statusid);
                if (status != null)
                {
                    afiliado.estatus = status.name;
                }

                // Entidad : Temas de Interés del Afiliado. 
                foreach (var m in db.CustomerInterests.Where(f => f.customerid == afiliado.id))
                {
                    db.CustomerInterests.Remove(m);
                }
                foreach (var interes in afiliado.Intereses.Where(x => x.Checked == true))
                {
                    CustomerInterest customerInterest = new CustomerInterest()
                    {
                        customerid = afiliado.id,
                        interestid = interes.id,
                        comments = ""
                    };
                    db.CustomerInterests.Add(customerInterest);
                }

                //Entidad : Auditoría del registro de Afiliado. 
                var affiliateAuditoria = new AffiliateAud()
                {
                    id = AfilliateAudID(),
                    affiliateid = afiliado.id,
                    modifieduserid = (int)HttpContext.Current.Session["userid"],
                    modifieddate = System.DateTime.Now,
                    statusid = afiliado.statusid,       //PENDIENTE
                    reasonsid = 1,      //PENDIENTE
                    comments = afiliado.comments
                };
                db.AffiliateAuds.Add(affiliateAuditoria);
                if (SaveWebPlazas(afiliado))
                {
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public SaldosMovimientos FindSaldosMovimientos(int id)
        {
            SaldosMovimientos SaldosMovimientos = new SaldosMovimientos();
            Afiliado afiliado = Find(id);
            SaldosMovimientos.DocId = afiliado.docnumber;
            string nrodocumento = SaldosMovimientos.DocId.Substring(2);
            string saldosJson = WSL.Cards.getBalance(nrodocumento);
            SaldosMovimientos.Saldos = (IEnumerable<Saldo>)JsonConvert.DeserializeObject<IEnumerable<Saldo>>(saldosJson);
            string movimientosPrepagoJson = WSL.Cards.getBatch(SaldosMovimientos.Saldos.First().accounttype, nrodocumento);
            string movimientosLealtadJson = WSL.Cards.getBatch(SaldosMovimientos.Saldos.Skip(1).First().accounttype, nrodocumento);
            SaldosMovimientos.MovimientosPrepago = (IEnumerable<Movimiento>)JsonConvert.DeserializeObject<IEnumerable<Movimiento>>(movimientosPrepagoJson);
            var MovimientosPrepagoOrdenados = SaldosMovimientos.MovimientosPrepago.OrderByDescending(x => x.batchid);
            SaldosMovimientos.MovimientosPrepago = MovimientosPrepagoOrdenados.Take(3);
            foreach (var mov in SaldosMovimientos.MovimientosPrepago)
            {
                mov.fecha = mov.fecha.Substring(6, 2) + "-" + mov.fecha.Substring(4, 2) + "-" + mov.fecha.Substring(0, 4);
            }
            SaldosMovimientos.MovimientosSuma = (IEnumerable<Movimiento>)JsonConvert.DeserializeObject<IEnumerable<Movimiento>>(movimientosLealtadJson);
            var MovimientosSumaOrdenados = SaldosMovimientos.MovimientosSuma.OrderByDescending(x => x.batchid);
            SaldosMovimientos.MovimientosSuma = MovimientosSumaOrdenados.Take(3);
            foreach (var mov in SaldosMovimientos.MovimientosSuma)
            {
                mov.fecha = mov.fecha.Substring(6, 2) + "-" + mov.fecha.Substring(4, 2) + "-" + mov.fecha.Substring(0, 4);
            }
           
            return SaldosMovimientos;
        }

        public RespuestaCards Acreditar(string numdoc, string monto)
        {
            RespuestaCards RespuestaCards = new RespuestaCards();
            string RespuestaCardsJson = WSL.Cards.addBatch(numdoc.Substring(2), monto);
            RespuestaCards = (RespuestaCards)JsonConvert.DeserializeObject<RespuestaCards>(RespuestaCardsJson);
            return RespuestaCards;
        }

        public RespuestaCards BloquearTarjeta(string numdoc)
        {
            RespuestaCards RespuestaCards = new RespuestaCards();
            string RespuestaCardsJson = WSL.Cards.addCard(numdoc.Substring(2));
            RespuestaCards = (RespuestaCards)JsonConvert.DeserializeObject<RespuestaCards>(RespuestaCardsJson);
            return RespuestaCards;
        }

        public RespuestaCards SuspenderTarjeta(string numdoc)
        {
            Afiliado afiliado = Find(numdoc);
            RespuestaCards RespuestaCards = new RespuestaCards();
            string RespuestaCardsJson = WSL.Cards.cardStatus(afiliado.docnumber.Substring(2), ID_ESTATUS_TARJETA_SUSPENDIDA);
            RespuestaCards = (RespuestaCards)JsonConvert.DeserializeObject<RespuestaCards>(RespuestaCardsJson);
            return RespuestaCards;
            //if (RespuestaCards.code == "0")
            //{
            //    afiliado.estatustarjeta = "Suspendida";
            //    SaveChanges(afiliado);
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public RespuestaCards ReactivarTarjeta(string numdoc)
        {
            Afiliado afiliado = Find(numdoc);
            RespuestaCards RespuestaCards = new RespuestaCards();
            string RespuestaCardsJson = WSL.Cards.cardActive(afiliado.docnumber.Substring(2));
            RespuestaCards = (RespuestaCards)JsonConvert.DeserializeObject<RespuestaCards>(RespuestaCardsJson);
            return RespuestaCards;
            //if (RespuestaCards.code == "0")
            //{
            //    afiliado.estatustarjeta = "Activa";
            //    SaveChanges(afiliado);
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public RespuestaCards ImprimirTarjeta(string numdoc)
        {
            RespuestaCards RespuestaCards = new RespuestaCards();
            string RespuestaCardsJson = WSL.Cards.cardActive(numdoc.Substring(2));
            RespuestaCards = (RespuestaCards)JsonConvert.DeserializeObject<RespuestaCards>(RespuestaCardsJson);
            if (RespuestaCards.code == "0")
            {
                RespuestaCardsJson = WSL.Cards.cardPrint(numdoc.Substring(2));
                RespuestaCards = (RespuestaCards)JsonConvert.DeserializeObject<RespuestaCards>(RespuestaCardsJson);
            }
            return RespuestaCards;
        }

        public bool Aprobar(Afiliado afiliado)
        {
            RespuestaCards RespuestaCards = new RespuestaCards();
            string RespuestaCardsJson = WSL.Cards.addClient(afiliado.docnumber, (afiliado.name + " " + afiliado.lastname1).ToUpper(), afiliado.phone1, "Plazas Baruta");
            RespuestaCards = (RespuestaCards)JsonConvert.DeserializeObject<RespuestaCards>(RespuestaCardsJson);
            if (RespuestaCards.code == "0" || RespuestaCards.code == "7")
            {
                afiliado.statusid = ID_ESTATUS_ACTIVA;
                return SaveChanges(afiliado);
            }
            else
            {
                return false;
            }
        }

    }
}