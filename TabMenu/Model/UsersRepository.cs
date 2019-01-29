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
    class UsersRepository
    {
        private static string ws1 = "http://localhost:60608/api/";

        public static List<User> GetAllUsers()
        {
            List<User> lu = (List<User>)MakeRequest(string.Concat(ws1, "users/"), null, "GET", "application/json", typeof(List<User>));
            return lu;
        }
        //public static List<contacte> GetAllContactesTot()
        //{
        //    List<contacte> lc = (List<contacte>)MakeRequest(string.Concat(ws1, "contactesTot/"), null, "GET", "application/json", typeof(List<contacte>));
        //    return lc;
        //}

        //public static contacte GetContacte(int id)
        //{
        //    contacte c = (contacte)MakeRequest(string.Concat(ws1, "contacteTot/", id), null, "GET", "application/json", typeof(contacte));
        //    return c;
        //}

        //public static contacte InsertContacte(contacte c2Add)
        //{
        //    contacte c = (contacte)MakeRequest(string.Concat(ws1, "contacte/"), c2Add, "POST", "application/json", typeof(contacte));
        //    return c;
        //}

        //public static contacte UpdateContacte(contacte c2Upd)
        //{
        //    contacte c = (contacte)MakeRequest(string.Concat(ws1, "contacte/", c2Upd.contacteId), c2Upd, "PUT", "application/json", typeof(contacte));
        //    return c;
        //}

        //public static void DeleteContacte(int id)
        //{
        //    MakeRequest(string.Concat(ws1, "contacte/", id), null, "DELETE", null, typeof(void));
        //}

        //public static List<telefon> GetAllTelefons()
        //{
        //    List<telefon> lt = (List<telefon>)MakeRequest(string.Concat(ws1, "telefons/"), null, "GET", "application/json", typeof(List<telefon>));
        //    return lt;
        //}

        //public static telefon GetTelefon(int id)
        //{
        //    telefon t = (telefon)MakeRequest(string.Concat(ws1, "telefon/", id), null, "GET", "application/json", typeof(telefon));
        //    return t;
        //}

        //public static telefon InsertTelefon(telefon t2Add)
        //{
        //    telefon t = (telefon)MakeRequest(string.Concat(ws1, "telefon/"), t2Add, "POST", "application/json", typeof(telefon));
        //    return t;
        //}

        //public static telefon UpdateTelefon(telefon t2Upd)
        //{
        //    telefon t = (telefon)MakeRequest(string.Concat(ws1, "telefon/", t2Upd.contacteId), t2Upd, "PUT", "application/json", typeof(telefon));
        //    return t;
        //}

        //public static void DeleteTelefon(int id)
        //{
        //    MakeRequest(string.Concat(ws1, "telefon/", id), null, "DELETE", null, typeof(void));
        //}

        //public static List<email> GetAllEmails()
        //{
        //    List<email> le = (List<email>)MakeRequest(string.Concat(ws1, "emails/"), null, "GET", "application/json", typeof(List<email>));
        //    return le;
        //}

        //public static email GetEmail(int id)
        //{
        //    email e = (email)MakeRequest(string.Concat(ws1, "email/", id), null, "GET", "application/json", typeof(email));
        //    return e;
        //}

        //public static email InsertEmail(email e2Add)
        //{
        //    email e = (email)MakeRequest(string.Concat(ws1, "email/"), e2Add, "POST", "application/json", typeof(email));
        //    return e;
        //}

        //public static email UpdateEmail(email e2Upd)
        //{
        //    email t = (email)MakeRequest(string.Concat(ws1, "email/", e2Upd.contacteId), e2Upd, "PUT", "application/json", typeof(email));
        //    return t;
        //}

        //public static void DeleteEmail(int id)
        //{
        //    MakeRequest(string.Concat(ws1, "email/", id), null, "DELETE", null, typeof(void));
        //}

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
