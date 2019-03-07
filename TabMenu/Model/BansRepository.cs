using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BancDelTemps.Model.Class;
using Newtonsoft.Json;

namespace BancDelTemps.Model
{
    class BansRepository
    {
        private static string ws1 = "http://172.16.12.5:45455/api/";

        public static List<Ban> GetAllBans()
        {
            List<Ban> lb = (List<Ban>)MakeRequest(string.Concat(ws1, "bans"), null, "GET", "application/json", typeof(List<Ban>));
            return lb;
        }

        public static Ban InsertBan(Pact b2Add)
        {
            Ban b = (Ban)MakeRequest(string.Concat(ws1, "bans"), b2Add, "POST", "application/json", typeof(Ban));
            return b;
        }

        public static Ban UpdateBan(Ban b2Upd)
        {
            Ban b = (Ban)MakeRequest(string.Concat(ws1, "ban/", b2Upd.Id_Ban), b2Upd, "PUT", "application/json", typeof(Ban));
            return b;
        }

        public static void DeleteBan(int id)
        {
            MakeRequest(string.Concat(ws1, "ban/", id), null, "DELETE", null, typeof(void));
        }

        public static object MakeRequest(string requestUrl, object JSONRequest, string JSONmethod, string JSONContentType, Type JSONResponseType)
        //  requestUrl: Url completa del Web Service, amb l'opció sol·licitada
        //  JSONrequest: objecte que se li passa en el body (només per a POST/PUT)
        //  JSONmethod: "GET"/"POST"/"PUT"/"DELETE"
        //  JSONContentType: "application/json" en els casos que el Web Service torni objectes
        //  JSONRensponseType:  tipus d'objecte que torna el Web Service (typeof(tipus))
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest; //WebRequest WR = WebRequest.Create(requestUrl);   
                string sb = JsonConvert.SerializeObject(JSONRequest);
                request.Method = JSONmethod;  // "GET"/"POST"/"PUT"/"DELETE";  

                if (JSONmethod != "GET")
                {
                    request.ContentType = JSONContentType; // "application/json";   
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));

                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    object objResponse = JsonConvert.DeserializeObject(strsb, JSONResponseType);
                    return objResponse;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
    }
}
