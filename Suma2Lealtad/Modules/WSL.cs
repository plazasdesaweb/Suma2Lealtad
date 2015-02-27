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

        // consumir los servicios asociados a Cards.
        public static string Cards(string nameService)
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

        public static string PlazasWeb(string nameService)
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


        // retornar la respuesta en JSON.
        private static string GetResponseJSON(string uri)
        {

            HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
            HttpWebRequest.DefaultCachePolicy = policy;
            // Create the request.
            WebRequest request = WebRequest.Create(uri);
            // Define a cache policy for this request only. 
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;
            WebResponse response = request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                return reader.ReadToEnd();
            }

        }

        private static string GetExcepcionJSON(string ExcepcionMessage)
        {

            return "{\"code\":\"100\",\"detail\":\" @ex.Message@\", \"source\": \"WebServices PlazasWeb\" }".Replace("@ex.Message@", ExcepcionMessage);

        }

    }

}