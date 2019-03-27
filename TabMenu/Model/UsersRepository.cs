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
    class UsersRepository
    {
        private static string ws1 = Strings.ipWebService;

        public static List<User> GetAllUsers()
        {
            List<User> lu = (List<User>)MakeRequest(string.Concat(ws1, "users"), null, "GET", "application/json", typeof(List<User>));
            return lu;
        }

        public static User InsertUser(User u2Add)
        {
            User u = (User)MakeRequest(string.Concat(ws1, "user"), u2Add, "POST", "application/json", typeof(User));
            return u;
        }

        public static User UpdateUser(User u2Upd)
        {
            User c = (User)MakeRequest(string.Concat(ws1, "user/", u2Upd.Id_User), u2Upd, "PUT", "application/json", typeof(User));
            return c;
        }

        public static void DeleteUser(int id)
        {
            MakeRequest(string.Concat(ws1, "user/", id), null, "DELETE", null, typeof(void));
        }

        public static List<User> GetUsersByEmail(string email)
        {
            List<User> lu = (List <User>)MakeRequest(string.Concat(ws1, "usersEmail/", email), null, "GET", "application/json", typeof(List<User>));
            return lu;
        }

        public static List<User> GetUsersByName(string name)
        {
            List<User> lu = (List<User>)MakeRequest(string.Concat(ws1, "usersName/", name), null, "GET", "application/json", typeof(List<User>));
            return lu;
        }

        public static User GetUser(int id)
        {
            User u = (User)MakeRequest(string.Concat(ws1, "userSi/", id), null, "GET", "application/json", typeof(User));
            return u;
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
