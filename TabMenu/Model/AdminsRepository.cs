﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BancDelTemps.Model.Class;
using BancDelTemps.Properties;
using Newtonsoft.Json;

namespace BancDelTemps.Model
{
    class AdminsRepository
    {
        private static string ws1 = Strings.ipWebService;

        public static List<Admin> GetAllAdmins()
        {
            List<Admin> la = (List<Admin>)MakeRequest(string.Concat(ws1, "admins"), null, "GET", "application/json", typeof(List<Admin>));
            return la;
        }

        public static Admin InsertAdmin(Admin a2Add)
        {
            Admin a = (Admin)MakeRequest(string.Concat(ws1, "admin"), a2Add, "POST", "application/json", typeof(Admin));
            return a;
        }
        public static Admin LoginAdmin(Admin login)
        {
            var data = Encoding.UTF8.GetBytes("admin");
            byte[] hash;
            using (SHA512 shaM = new SHA512Managed())
            {
                hash = shaM.ComputeHash(data);
            }
            var str = Encoding.Default.GetString(hash);
            login.password = str;
            Admin a = (Admin)MakeRequest(string.Concat(ws1, "admin/login"), login, "PUT", "application/json", typeof(Admin));
            return a;
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
