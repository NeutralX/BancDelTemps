using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BancDelTemps.Model.Class;
using BancDelTemps.Properties;
using Newtonsoft.Json;

namespace BancDelTemps.Model
{
    class PactsRepository
    {
        private static string ws1 = Strings.ipWebService;

        public static List<Pact> GetAllPacts()
        {
            List<Pact> lp = (List<Pact>)MakeRequest(string.Concat(ws1, "pacts"), null, "GET", "application/json", typeof(List<Pact>));
            return lp;
        }

        public static Pact InsertPact(Pact p2Add)
        {
            Pact p = (Pact)MakeRequest(string.Concat(ws1, "pact"), p2Add, "POST", "application/json", typeof(Pact));
            return p;
        }

        public static Pact UpdatePact(Pact p2Upd)
        {
            Pact p = (Pact)MakeRequest(string.Concat(ws1, "pact/", p2Upd.Id_Pact), p2Upd, "PUT", "application/json", typeof(Pact));
            return p;
        }

        public static void DeletePact(int id)
        {
            MakeRequest(string.Concat(ws1, "pact/", id), null, "DELETE", null, typeof(void));
        }

        public static List<Pact> GetPactsByTitle(string titlePact)
        {
            List<Pact> lp = (List<Pact>)MakeRequest(string.Concat(ws1, "pactsTitle/", titlePact), null, "GET", "application/json", typeof(List<Pact>));
            return lp;
        }

        public static List<Pact> GetPactsByDescription(string descriptionPact)
        {
            List<Pact> lp = (List<Pact>)MakeRequest(string.Concat(ws1, "pactsDesc/", descriptionPact), null, "GET", "application/json", typeof(List<Pact>));
            return lp;
        }

        public static List<Pact> GetPactsByDateCreated(DateTime dateCreated)
        {
            string dateString = dateCreated.ToString("dd-MM-yyyy");
            List<Pact> lp = (List<Pact>)MakeRequest(string.Concat(ws1, "pactsDateCreated/", dateString), null, "GET", "application/json", typeof(List<Pact>));
            return lp;
        }

        public static List<Pact> GetPactsByDateFinished(DateTime dateFinished)
        {
            string dateString = dateFinished.ToString("dd-MM-yyyy");
            List<Pact> lp = (List<Pact>)MakeRequest(string.Concat(ws1, "pactsDateFinished/", dateString), null, "GET", "application/json", typeof(List<Pact>));
            return lp;
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
