using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Web;

namespace Suma2Lealtad.Modules
{
    /* Capa de Acceso a Servicios Web */
    public class WSL
    {
        //servicios cards
        private const string WSL_CARDS_ADDCLIENT = "addclient/{numdoc}/{name}/{phone}/{address}";
        private const string WSL_CARDS_GETCLIENT = "getclient/{numdoc}";
        private const string WSL_CARDS_GETBALANCE = "getbalance/{numdoc}";
        private const string WSL_CARDS_GETBATCH = "getbatch/{accounttype}/{numdoc}";
        private const string WSL_CARDS_CARD_PRINT = "card/print/{numdoc}";
        private const string WSL_CARDS_CARD_ACTIVE = "active/{numdoc}";
        private const string WSL_CARDS_CARD_INACTIVE = "card/inactive/{numdoc}";
        //servicios web
        private const string WSL_WEB_GETCLIENTBYNUMDOC = "getclientbynumdoc/{docnumber}";
        private const string WSL_WEB_UPDCLIENT = "updclient/{id}/{docnumber}/{nationality}/{name}/{name2}/{lastname1}/{lastname2}/{birthdate}/{gender}/{maritalstatus}/{occupation}/{phone1}/{phone2}/{phone3}/{email}/{type}";

        // retornar la respuesta en JSON.
        private static string GetResponseJSON(string uri)
        {
            HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
            HttpWebRequest.DefaultCachePolicy = policy;
            WebRequest request = WebRequest.Create(uri);
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;
            WebResponse response = request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }

        // retornar excepcion JSON.
        private static string GetExcepcionJSON(string ExcepcionMessage)
        {
            return "{\"code\":\"100\",\"detail\":\" @ex.Message@\", \"source\": \"WebServices PlazasWeb\" }".Replace("@ex.Message@", ExcepcionMessage);
        }

        //implementa los servicios cards
        public class Cards
        {
            // consumir los servicios asociados a Cards.
            public static string ConsumirServicioCards(string nameService)
            {
                try
                {
                    string str = AppModule.CardsServerRoute() + nameService;
                    return GetResponseJSON(str.Trim());
                }
                catch (Exception ex)
                {
                    return GetExcepcionJSON(ex.Message);
                }
            }

            public static string addClient(string numdoc, string name, string phone, string address)
            {
                string req = WSL_CARDS_ADDCLIENT;
                req = req.Replace("{numdoc}", numdoc);
                req = req.Replace("{name}", name);
                req = req.Replace("{phone}", phone);
                req = req.Replace("{address}", address);
                return ConsumirServicioCards(req);
            }

            public static string getClient(string numdoc)
            {
                string req = WSL_CARDS_GETCLIENT;
                req = req.Replace("{numdoc}", numdoc);
                return ConsumirServicioCards(req);
            }

            public static string getBalance(string numdoc)
            {
                string req = WSL_CARDS_GETBALANCE;
                req = req.Replace("{numdoc}", numdoc);
                return ConsumirServicioCards(req);
            }

            public static string getBatch(string accounttype, string numdoc)
            {
                string req = WSL_CARDS_GETBATCH;
                req = req.Replace("{accounttype}", accounttype);
                req = req.Replace("{numdoc}", numdoc);
                return ConsumirServicioCards(req);
            }

            public static string cardPrint(string numdoc)
            {
                string req = WSL_CARDS_CARD_PRINT;
                req = req.Replace("{numdoc}", numdoc);
                return ConsumirServicioCards(req);
            }

            public static string cardActive(string numdoc)
            {
                string req = WSL_CARDS_CARD_ACTIVE;
                req = req.Replace("{numdoc}", numdoc);
                return ConsumirServicioCards(req);
            }

            public static string cardInactive(string numdoc)
            {
                string req = WSL_CARDS_CARD_INACTIVE;
                req = req.Replace("{numdoc}", numdoc);
                return ConsumirServicioCards(req);
            }

        }

        //implementa los servicios web
        public class PlazasWeb
        {
            // consumir los servicios asociados a PlazasWeb.
            private static string ConsumirServicioPlazasWeb(string nameService)
            {
                try
                {
                    string str = AppModule.WebServerRoute() + nameService;
                    return GetResponseJSON(str.Trim());
                }
                catch (Exception ex)
                {
                    return GetExcepcionJSON(ex.Message);
                }
            }

            public static string getClientByNumDoc(string docnumber)
            {
                string req = WSL_WEB_GETCLIENTBYNUMDOC;
                req = req.Replace("{docnumber}", docnumber);
                return ConsumirServicioPlazasWeb(req);
            }

            public static string UpdateClient(Suma2Lealtad.Models.AfiliadoSuma record)
            {
                string req = WSL_WEB_UPDCLIENT;
                req = req.Replace("{id}", record.id);
                req = req.Replace("{type}", record.type + "");
                req = req.Replace("{docnumber}", record.docnumber);
                req = req.Replace("{email}", record.email);
                req = req.Replace("{name}", record.name);
                //req = req.Replace("{name2}", record.name2 + "");
                req = req.Replace("{name2}", record.name2 == null ? "NULO" : record.name2);
                req = req.Replace("{lastname1}", record.lastname1);
                //req = req.Replace("{lastname2}", record.lastname2);
                req = req.Replace("{lastname2}", record.lastname2 == null ? "NULO" : record.lastname2);
                req = req.Replace("{phone1}", record.phone1);
                //req = req.Replace("{phone2}", record.phone2);
                req = req.Replace("{phone2}", record.phone2 == null ? "NULO" : record.phone2);
                //req = req.Replace("{phone3}", record.phone3);
                req = req.Replace("{phone3}", record.phone3 == null ? "NULO" : record.phone3);
                req = req.Replace("{birthdate}", "19361217"); //record.birthdate + "");
                req = req.Replace("{occupation}", record.occupation + "");
                req = req.Replace("{nationality}", record.nationality + "");
                req = req.Replace("{maritalstatus}", record.maritalstatus + "");
                req = req.Replace("{gender}", record.gender + "");
                return ConsumirServicioPlazasWeb(req);
            }

        }                

    }
}
