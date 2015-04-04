using Newtonsoft.Json;
using Suma2Lealtad.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class AfiliadoRepository
    {

        private int INITIAL_INTEGER_VALUE = 1;
        private string INITIAL_STRING_VALUE = "";

        public AfiliadoRepository() { }

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
                //Primero se buscan los datos de CLIENTE en WebPlazas
                //WSL.WebPlazas.getClientByNumDoc
                //Segundo se buscan los datos de AFILIADO en SumaPlazas
                //Affiliado
                //CLIENTE
                //Status
                //CustomerInterest
                //Photos_Affiliate, FILEYSTEM ~/Picture/@filename@.jpg                
                //WSL.Cards.getClient
                //Tarjeta.ConstruirTrackI
                //Tarjeta.ConstruirTrackII
                //SERVICIO WSL.WebPlazas.getClientByNumDoc // +

                string ClienteWebPlazasJson = WSL.WebPlazas.getClientByNumDoc(numdoc);
                ClienteWebPlazas ClienteWebPlazas = (ClienteWebPlazas)JsonConvert.DeserializeObject<ClienteWebPlazas>(ClienteWebPlazasJson);

                Afiliado record = (from a in db.Affiliates
                                   where a.docnumber.Equals(numdoc)
                                   select new Afiliado()
                                   {
                                       //ENTIDAD Affiliate 
                                       id = a.id,
                                   }).SingleOrDefault();

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

                if (ClienteWebPlazas == null && record == null)
                {
                }
                else if (ClienteWebPlazas == null && record != null)
                {
                    record.clientid = 0;
                }
                else if (ClienteWebPlazas != null && record == null)
                {
                    record = new Afiliado();
                    record.docnumber = ClienteWebPlazas.docnumber; // +<*   
                    record.clientid = ClienteWebPlazas.id; // CORRESPONDE AL ID DE CLIENTE EN WEBPLAZAS
                    record.nationality = ClienteWebPlazas.nationality; // +*
                    record.name = ClienteWebPlazas.name; // +<*
                    record.name2 = ClienteWebPlazas.name2; // +<*
                    record.lastname1 = ClienteWebPlazas.lastname1; // +<*
                    record.lastname2 = ClienteWebPlazas.lastname2; // +<*
                    record.birthdate = ClienteWebPlazas.birthdate; // +*
                    record.gender = ClienteWebPlazas.gender; //+*
                    record.maritalstatus = ClienteWebPlazas.maritalstatus; // +*
                    record.occupation = ClienteWebPlazas.occupation; // +*
                    record.phone1 = ClienteWebPlazas.phone1; // +<*
                    record.phone2 = ClienteWebPlazas.phone2; // +*
                    record.phone3 = ClienteWebPlazas.phone3; // +*
                    record.email = ClienteWebPlazas.email; // +*
                    record.type = ClienteWebPlazas.type; // +*

                    //ENTIDAD CustomerInterest
                    record.Intereses = chargeInterestList(record.id);
                }
                else if (ClienteWebPlazas != null && record != null)
                {
                    record = (from c in db.CLIENTES
                              join a in db.Affiliates on c.NRO_DOCUMENTO
                              equals a.docnumber
                              join s in db.Status on a.statusid
                              equals s.id
                              where a.id == record.id
                              select new Afiliado()
                              {
                                  //ENTIDAD Affiliate 
                                  id = a.id,
                                  customerid = a.customerid, // (No se a que corresponde)                	 
                                  docnumber = a.docnumber, // +<*    
                                  clientid = a.clientid, // +(corresponde al id de WEBPLAZAS)*
                                  storeid = a.storeid,
                                  channelid = a.channelid,
                                  typeid = a.typeid,
                                  typedelivery = a.typedelivery,
                                  storeiddelivery = a.storeiddelivery == null ? 0 : (int)a.storeiddelivery,
                                  statusid = a.statusid,
                                  reasonsid = a.reasonsid == null ? 0 : (int)a.reasonsid,
                                  twitter_account = a.twitter_account, // +
                                  facebook_account = a.facebook_account, // +
                                  instagram_account = a.instagram_account, // +
                                  comments = a.comments,
                                  //ENTIDAD CLIENTE
                                  name = c.NOMBRE_CLIENTE1,
                                  name2 = c.NOMBRE_CLIENTE2,
                                  lastname1 = c.APELLIDO_CLIENTE1,
                                  lastname2 = c.APELLIDO_CLIENTE2,
                                  birthdate = c.FECHA_NACIMIENTO,
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
                                  //ENTIDAD Status
                                  estatus = s.name
                              }).Single();

                    record.nationality = ClienteWebPlazas.nationality; // +*
                    record.name = ClienteWebPlazas.name; // +<*
                    record.name2 = ClienteWebPlazas.name2; // +<*
                    record.lastname1 = ClienteWebPlazas.lastname1; // +<*
                    record.lastname2 = ClienteWebPlazas.lastname2; // +<*
                    record.birthdate = ClienteWebPlazas.birthdate; // +*
                    record.gender = ClienteWebPlazas.gender; //+*
                    record.maritalstatus = ClienteWebPlazas.maritalstatus; // +*
                    record.occupation = ClienteWebPlazas.occupation; // +*
                    record.phone1 = ClienteWebPlazas.phone1; // +<*
                    record.phone2 = ClienteWebPlazas.phone2; // +*
                    record.phone3 = ClienteWebPlazas.phone3; // +*
                    record.email = ClienteWebPlazas.email; // +*
                    record.type = ClienteWebPlazas.type; // +*

                    //ENTIDAD CustomerInterest
                    record.Intereses = chargeInterestList(record.id);

                    //ENTIDAD Photos_Affiliate, FILEYSTEM ~/Picture/@filename@.jpg
                    record.picture = AppModule.GetPathPicture().Replace("@filename@", record.docnumber);

                    //SERVICIO WSL.Cards.getClient !
                    string ClienteCardsJson = WSL.Cards.getClient(record.docnumber.Substring(2));
                    ClienteCards ClienteCards = (ClienteCards)JsonConvert.DeserializeObject<IEnumerable<ClienteCards>>(ClienteCardsJson).FirstOrDefault();
                    record.pan = ClienteCards.pan; // !
                    record.printed = ClienteCards.printed; // !
                    record.estatustarjeta = ClienteCards.tarjeta; // !

                    //METODO Tarjeta.ConstruirTrackI
                    record.trackI = Tarjeta.ConstruirTrackI(record.pan);

                    //METODO Tarjeta.ConstruirTrackII
                    record.trackII = Tarjeta.ConstruirTrackII(record.pan);
                }
                return record;
            }
        }


        // 
        // FindSuma : Buscar el registro del Afiliado en el Modelo SumaLealtad.


        public Afiliado FindSuma(int id = 0)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                //Primero se crea el objeto record de tipo Afiliado, y se llena a partir de varios origenes de datos en el siguiente orden 
                //Affiliado
                //CLIENTE
                //Status
                //CustomerInterest
                //Photos_Affiliate, FILEYSTEM ~/Picture/@filename@.jpg
                //WSL.WebPlazas.getClientByNumDoc
                //WSL.Cards.getClient
                //Tarjeta.ConstruirTrackI
                //Tarjeta.ConstruirTrackII

                Afiliado record = (from c in db.CLIENTES
                                   join a in db.Affiliates on c.NRO_DOCUMENTO
                                   equals a.docnumber
                                   join s in db.Status on a.statusid
                                   equals s.id
                                   where a.id == id
                                   select new Afiliado()
                                   {
                                       //id = a.id,
                                       //customerid = a.customerid,
                                       //docnumber = c.NRO_DOCUMENTO,
                                       //clientid = a.clientid,
                                       //typeid = a.typeid,
                                       //nationality = c.NACIONALIDAD,
                                       //name = c.NOMBRE_CLIENTE1,
                                       //name2 = c.NOMBRE_CLIENTE2,
                                       //lastname1 = c.APELLIDO_CLIENTE1,
                                       //lastname2 = c.APELLIDO_CLIENTE2,
                                       //birthdate = c.FECHA_NACIMIENTO,
                                       //gender = c.SEXO,
                                       //maritalstatus = c.EDO_CIVIL,
                                       //occupation = c.OCUPACION,
                                       //phone1 = c.TELEFONO_HAB,
                                       //phone2 = c.TELEFONO_OFIC,
                                       //phone3 = c.TELEFONO_CEL,
                                       //email = c.E_MAIL,
                                       //storeiddelivery = (int) c.COD_SUCURSAL,
                                       //cod_estado = c.COD_ESTADO,
                                       //cod_ciudad = c.COD_CIUDAD,
                                       //cod_municipio = c.COD_MUNICIPIO,
                                       //cod_parroquia = c.COD_PARROQUIA,
                                       //cod_urbanizacion = c.COD_URBANIZACION,
                                       //facebook_account = a.facebook_account,
                                       //twitter_account = a.twitter_account,
                                       //instagram_account = a.instagram_account,
                                       //channelid = a.channelid,
                                       //typedelivery = a.typedelivery,
                                       //comments = a.comments

                                       //ENTIDAD Affiliate 
                                       id = a.id,
                                       customerid = a.customerid, // (No se a que corresponde)                	 
                                       docnumber = a.docnumber, // +<*    
                                       clientid = a.clientid, // +(corresponde al id de WEBPLAZAS)*
                                       storeid = a.storeid,
                                       channelid = a.channelid,
                                       typeid = a.typeid,
                                       typedelivery = a.typedelivery,
                                       storeiddelivery = a.storeiddelivery == null ? 0 : (int)a.storeiddelivery,
                                       statusid = a.statusid,
                                       reasonsid = a.reasonsid == null ? 0 : (int)a.reasonsid,
                                       twitter_account = a.twitter_account, // +
                                       facebook_account = a.facebook_account, // +
                                       instagram_account = a.instagram_account, // +
                                       comments = a.comments,
                                       //ENTIDAD CLIENTE
                                       name = c.NOMBRE_CLIENTE1,
                                       name2 = c.NOMBRE_CLIENTE2,
                                       lastname1 = c.APELLIDO_CLIENTE1,
                                       lastname2 = c.APELLIDO_CLIENTE2,
                                       birthdate = c.FECHA_NACIMIENTO,
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
                                       //ENTIDAD Status
                                       estatus = s.name
                                   }).Single();
                if (record != null)
                {
                    //ENTIDAD CustomerInterest
                    record.Intereses = chargeInterestList(record.id);

                    //ENTIDAD Photos_Affiliate, FILEYSTEM ~/Picture/@filename@.jpg
                    record.picture = AppModule.GetPathPicture().Replace("@filename@", record.docnumber);

                    //SERVICIO WSL.WebPlazas.getClientByNumDoc // +
                    string ClienteWebPlazasJson = WSL.WebPlazas.getClientByNumDoc(record.docnumber);
                    ClienteWebPlazas ClienteWebPlazas = (ClienteWebPlazas)JsonConvert.DeserializeObject<ClienteWebPlazas>(ClienteWebPlazasJson);
                    record.nationality = ClienteWebPlazas.nationality; // +*
                    //record.name = ClienteWebPlazas.name; // +<*
                    //record.name2 = ClienteWebPlazas.name2; // +<*
                    //record.lastname1 = ClienteWebPlazas.lastname1; // +<*
                    //record.lastname2 = ClienteWebPlazas.lastname2; // +<*
                    //record.birthdate = ClienteWebPlazas.birthdate; // +*
                    //record.gender = ClienteWebPlazas.gender; //+*
                    //record.maritalstatus = ClienteWebPlazas.maritalstatus; // +*
                    //record.occupation = ClienteWebPlazas.occupation; // +*
                    //record.phone1 = ClienteWebPlazas.phone1; // +<*
                    //record.phone2 = ClienteWebPlazas.phone2; // +*
                    //record.phone3 = ClienteWebPlazas.phone3; // +*
                    //record.email = ClienteWebPlazas.email; // +*
                    record.type = ClienteWebPlazas.type; // +*

                    //SERVICIO WSL.Cards.getClient !
                    string ClienteCardsJson = WSL.Cards.getClient(record.docnumber.Substring(2));
                    ClienteCards ClienteCards = (ClienteCards)JsonConvert.DeserializeObject<IEnumerable<ClienteCards>>(ClienteCardsJson).FirstOrDefault();
                    record.pan = ClienteCards.pan; // !
                    record.printed = ClienteCards.printed; // !
                    record.estatustarjeta = ClienteCards.tarjeta; // !

                    //METODO Tarjeta.ConstruirTrackI
                    record.trackI = Tarjeta.ConstruirTrackI(record.pan);

                    //METODO Tarjeta.ConstruirTrackII
                    record.trackII = Tarjeta.ConstruirTrackII(record.pan);
                }
                else
                {
                    //Excepción: No existe afiliado con ese id
                }
                return record;
            }
        }


        public List<Afiliado> FindSuma(string numdoc, string name, string email)
        {
            if (name == "")
                name = null;
            if (email == "")
                email = null;
            using (LealtadEntities db = new LealtadEntities())
            {
                List<Afiliado> records = (from c in db.CLIENTES
                                          join a in db.Affiliates on c.NRO_DOCUMENTO
                                          equals a.docnumber
                                          join s in db.Status on a.statusid
                                          equals s.id
                                          where c.NRO_DOCUMENTO.Equals(numdoc) || c.E_MAIL == email || c.NOMBRE_CLIENTE1.Contains(name)
                                          select new Afiliado()
                                          {
                                              id = a.id,
                                              docnumber = a.docnumber,
                                              name = c.NOMBRE_CLIENTE1 + " " + c.APELLIDO_CLIENTE1,
                                              name2 = c.NOMBRE_CLIENTE2,
                                              lastname1 = c.APELLIDO_CLIENTE1,
                                              lastname2 = c.APELLIDO_CLIENTE2,
                                              email = c.E_MAIL,
                                              estatus = s.name
                                          }).ToList();

                if (records != null)
                {
                    foreach (var record in records)
                    {
                        //SERVICIO WSL.Cards.getClient !
                        string ClienteCardsJson = WSL.Cards.getClient(record.docnumber.Substring(2));
                        ClienteCards ClienteCards = (ClienteCards)JsonConvert.DeserializeObject<IEnumerable<ClienteCards>>(ClienteCardsJson).FirstOrDefault();
                        record.pan = ClienteCards.pan; // !
                        record.printed = ClienteCards.printed; // !
                        record.estatustarjeta = ClienteCards.tarjeta; // !
                    }
                }
                return records;
            }
        }


        //
        // Save : Almacenar registro del afiliado en el Modelo de SumaLealtad.

        public bool Save(Afiliado AfiliadoSuma)
        {

            using (LealtadEntities db = new LealtadEntities())
            {

                var result = db.CLIENTES.SingleOrDefault(c => c.NRO_DOCUMENTO == AfiliadoSuma.docnumber);

                // Caso : El afiliado está registrado en PlazasWeb pero no está registrado en SumaLealtad.
                if (result == null)
                {

                    var cliente = new CLIENTE()
                    {
                        TIPO_DOCUMENTO = INITIAL_INTEGER_VALUE.ToString(),
                        NRO_DOCUMENTO = AfiliadoSuma.docnumber,
                        NACIONALIDAD = AfiliadoSuma.nationality,
                        NOMBRE_CLIENTE1 = AfiliadoSuma.name,
                        NOMBRE_CLIENTE2 = AfiliadoSuma.name2,
                        APELLIDO_CLIENTE1 = AfiliadoSuma.lastname1,
                        APELLIDO_CLIENTE2 = AfiliadoSuma.lastname2,
                        FECHA_NACIMIENTO = AfiliadoSuma.birthdate,
                        SEXO = AfiliadoSuma.gender,
                        EDO_CIVIL = AfiliadoSuma.maritalstatus,
                        OCUPACION = AfiliadoSuma.occupation,
                        TELEFONO_HAB = AfiliadoSuma.phone1,
                        TELEFONO_OFIC = AfiliadoSuma.phone2,
                        TELEFONO_CEL = AfiliadoSuma.phone3,
                        E_MAIL = AfiliadoSuma.email,
                        COD_SUCURSAL = AfiliadoSuma.storeiddelivery,
                        COD_ESTADO = AfiliadoSuma.cod_estado == null ? "0" : AfiliadoSuma.cod_estado,
                        COD_CIUDAD = AfiliadoSuma.cod_ciudad == null ? "0" : AfiliadoSuma.cod_ciudad,
                        COD_MUNICIPIO = AfiliadoSuma.cod_municipio == null ? "0" : AfiliadoSuma.cod_municipio,
                        COD_PARROQUIA = AfiliadoSuma.cod_parroquia == null ? "0" : AfiliadoSuma.cod_parroquia,
                        COD_URBANIZACION = AfiliadoSuma.cod_urbanizacion == null ? "0" : AfiliadoSuma.cod_urbanizacion,
                        FECHA_CREACION = System.DateTime.Now
                    };

                    var affiliate = new Affiliate()
                    {
                        id = AfilliatesID(),
                        customerid = AfiliadoSuma.id,
                        docnumber = AfiliadoSuma.docnumber,
                        clientid = AfiliadoSuma.clientid,
                        storeid = AfiliadoSuma.storeiddelivery,
                        channelid = AfiliadoSuma.channelid,
                        typeid = INITIAL_INTEGER_VALUE,
                        affiliatedate = System.DateTime.Now,
                        typedelivery = AfiliadoSuma.typedelivery,
                        storeiddelivery = AfiliadoSuma.storeiddelivery,
                        estimateddatedelivery = System.DateTime.Now,
                        creationdate = System.DateTime.Now,
                        creationuserid = (int)HttpContext.Current.Session["userid"],
                        modifieddate = System.DateTime.Now,
                        modifieduserid = (int)HttpContext.Current.Session["userid"],
                        statusid = INITIAL_INTEGER_VALUE,
                        reasonsid = INITIAL_INTEGER_VALUE,
                        twitter_account = AfiliadoSuma.twitter_account,
                        facebook_account = AfiliadoSuma.facebook_account,
                        instagram_account = AfiliadoSuma.instagram_account,
                        comments = AfiliadoSuma.comments
                    };

                    var companyaffiliate = new CompanyAffiliate()
                    {
                        affiliateid = affiliate.id,
                        companyid = INITIAL_INTEGER_VALUE,
                        begindate = System.DateTime.Now,
                        enddate = System.DateTime.Now,
                        comments = affiliate.comments,
                        active = true
                    };

                    db.CLIENTES.Add(cliente);

                    db.Affiliates.Add(affiliate);

                    db.CompanyAffiliates.Add(companyaffiliate);

                    foreach (var interes in AfiliadoSuma.Intereses.Where(x => x.Checked == true))
                    {

                        CustomerInterest customerInterest = new CustomerInterest()
                        {
                            customerid = affiliate.id,
                            interestid = interes.id,
                            comments = INITIAL_STRING_VALUE
                        };

                        db.CustomerInterests.Add(customerInterest);

                    }

                    var affiliateauditoria = new AffiliateAud()
                    {
                        id = AfilliateAudID(),
                        affiliateid = affiliate.id,
                        modifieduserid = (int)HttpContext.Current.Session["userid"],
                        modifieddate = System.DateTime.Now,
                        statusid = INITIAL_INTEGER_VALUE,
                        reasonsid = INITIAL_INTEGER_VALUE,
                        comments = affiliate.comments
                    };

                    db.AffiliateAuds.Add(affiliateauditoria);

                    db.SaveChanges();

                }
                return true;
            }

        }


        //
        // SaveChanges : Actualizar registro del afiliado en el Modelo de SumaLealtad.

        public bool SaveChanges(Afiliado afiliado)
        {

            using (LealtadEntities db = new LealtadEntities())
            {

                // Entidad : Cliente 
                CLIENTE cliente = db.CLIENTES.FirstOrDefault(c => c.NRO_DOCUMENTO == afiliado.docnumber);

                if (cliente != null)
                {

                    cliente.TIPO_DOCUMENTO = afiliado.typeid.ToString();
                    cliente.NRO_DOCUMENTO = afiliado.docnumber;
                    cliente.NACIONALIDAD = afiliado.nationality;
                    cliente.NOMBRE_CLIENTE1 = afiliado.name;
                    cliente.NOMBRE_CLIENTE2 = afiliado.name2;
                    cliente.APELLIDO_CLIENTE1 = afiliado.lastname1;
                    cliente.APELLIDO_CLIENTE2 = afiliado.lastname2;
                    cliente.FECHA_NACIMIENTO = afiliado.birthdate;
                    cliente.SEXO = afiliado.gender;
                    cliente.EDO_CIVIL = afiliado.maritalstatus;
                    cliente.OCUPACION = afiliado.occupation;
                    cliente.TELEFONO_HAB = afiliado.phone1;
                    cliente.TELEFONO_OFIC = afiliado.phone2;
                    cliente.TELEFONO_CEL = afiliado.phone3;
                    cliente.E_MAIL = afiliado.email;
                    cliente.COD_SUCURSAL = afiliado.storeiddelivery;
                    cliente.COD_ESTADO = afiliado.cod_estado;
                    cliente.COD_CIUDAD = afiliado.cod_ciudad;
                    cliente.COD_MUNICIPIO = afiliado.cod_municipio;
                    cliente.COD_PARROQUIA = afiliado.cod_parroquia;
                    cliente.COD_URBANIZACION = afiliado.cod_urbanizacion;
                    cliente.FECHA_CREACION = System.DateTime.Now;

                }

                // Entidad : Afiliado 
                Affiliate affiliate = db.Affiliates.FirstOrDefault(a => a.id == afiliado.id);

                if (affiliate != null)
                {

                    affiliate.id = afiliado.id;
                    affiliate.customerid = afiliado.customerid;
                    affiliate.docnumber = afiliado.docnumber;
                    affiliate.clientid = afiliado.clientid;
                    affiliate.storeid = afiliado.storeiddelivery;
                    affiliate.channelid = afiliado.channelid;
                    affiliate.typeid = afiliado.typeid;
                    affiliate.affiliatedate = System.DateTime.Now;
                    affiliate.typedelivery = afiliado.typedelivery;
                    affiliate.storeiddelivery = afiliado.storeiddelivery;
                    affiliate.estimateddatedelivery = System.DateTime.Now;
                    //affiliate.creationdate = System.DateTime.Now;
                    //affiliate.creationuserid = (int)HttpContext.Current.Session["userid"];
                    affiliate.modifieduserid = (int)HttpContext.Current.Session["userid"];
                    affiliate.modifieddate = System.DateTime.Now;
                    //affiliate.statusid = 1;
                    //affiliate.reasonsid = 1;
                    affiliate.twitter_account = afiliado.twitter_account;
                    affiliate.facebook_account = afiliado.facebook_account;
                    affiliate.instagram_account = afiliado.instagram_account;
                    affiliate.comments = afiliado.comments;

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

                //// Entidad : Auditoría del registro de Afiliado. 
                //var affiliateAuditoria = new AffiliateAud()
                //{
                //    id = AfilliateAudID(),
                //    affiliateid = afiliado.id,
                //    modifieduserid = (int) HttpContext.Current.Session["userid"],
                //    modifieddate = System.DateTime.Now,
                //    statusid = 1,       //PENDIENTE
                //    reasonsid = 1,      //PENDIENTE
                //    comments = afiliado.comments
                //};

                //db.AffiliateAuds.Add(affiliateAuditoria);

                db.SaveChanges();

            }

            return true;

        }

    }

}