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
    class CategoriesRepository
    {
        private static string ws1 = Strings.ipWebService;

        public static List<Category> GetAllCategories()
        {
            List<Category> lr = (List<Category>)MakeRequest(string.Concat(ws1, "categories"), null, "GET", "application/json", typeof(List<Category>));
            return lr;
        }

        public static List<String> GetAllCategoriyNames()
        {
            List<String> lr = (List<String>)MakeRequest(string.Concat(ws1, "categoriesString"), null, "GET", "application/json", typeof(List<String>));
            return lr;
        }

        public static Category GetSingleCategory(int id)
        {
            Category r = (Category)MakeRequest(string.Concat(ws1, "category/", id), null,"GET", "application/json", typeof(Category));
            return r;
        }

        public static int GetCategoryIdByString(string name)
        {
            int id = (int)MakeRequest(string.Concat(ws1, "categoryId/", name), null, "GET", "application/json", typeof(int));
            return id;
        }

        public static Category InsertCategory(Category r2Add)
        {
            Category r = (Category)MakeRequest(string.Concat(ws1, "category"), r2Add, "Category", "application/json", typeof(Category));
            return r;
        }

        public static Category UpdateCategory(Category r2Upd)
        {
            Category r = (Category)MakeRequest(string.Concat(ws1, "category/", r2Upd.Id_Category), r2Upd, "PUT", "application/json", typeof(Category));
            return r;
        }

        public static void DeleteCategory(int id)
        {
            MakeRequest(string.Concat(ws1, "category/", id), null, "DELETE", null, typeof(void));
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
