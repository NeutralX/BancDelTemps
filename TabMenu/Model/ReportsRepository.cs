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
    class ReportsRepository
    {
        private static string ws1 = Strings.ipWebService;

        public static List<Report> GetAllReports()
        {
            List<Report> lr = (List<Report>)MakeRequest(string.Concat(ws1, "reports"), null, "GET", "application/json", typeof(List<Report>));
            return lr;
        }

        public static Report InsertReport(Report r2Add)
        {
            Report r = (Report)MakeRequest(string.Concat(ws1, "report"), r2Add, "Report", "application/json", typeof(Report));
            return r;
        }

        public static Report UpdateReport(Report r2Upd)
        {
            Report r = (Report)MakeRequest(string.Concat(ws1, "report/", r2Upd.Id_Report), r2Upd, "PUT", "application/json", typeof(Report));
            return r;
        }

        public static void DeleteReport(int id)
        {
            MakeRequest(string.Concat(ws1, "report/", id), null, "DELETE", null, typeof(void));
        }

        public static List<Report> GetReportsByState(bool state)
        {
            List<Report> lr = (List<Report>)MakeRequest(string.Concat(ws1, "reportsRevised/", state), null, "GET", "application/json", typeof(List<Report>));
            return lr;
        }

        public static List<Report> GetReportsByDescription(string description)
        {
            List<Report> lr = (List<Report>)MakeRequest(string.Concat(ws1, "reportsDesc/", description), null, "GET", "application/json", typeof(List<Report>));
            return lr;
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
