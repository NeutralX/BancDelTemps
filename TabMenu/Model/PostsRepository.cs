using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using BancDelTemps.Model.Class;
using Newtonsoft.Json;

namespace BancDelTemps.Model
{
    class PostsRepository
    {
        private static string ws1 = "https://wsbancdeltemps.azurewebsites.net/api/";
        //private static string ws1 = "http://localhost:60608/api/";

        public static List<Post> GetAllPosts()
        {
            List<Post> lp = (List<Post>)MakeRequest(string.Concat(ws1, "posts"), null, "GET", "application/json", typeof(List<Post>));
            return lp;
        }

        public static List<Post> GetPostsByCategory(int idCategory)
        {
            List<Post> lp = (List<Post>)MakeRequest(string.Concat(ws1, "postsCategory/", idCategory), null, "GET", "application/json", typeof(List<Post>));
            return lp;
        }

        public static List<Post> GetPostsByDateCreated(DateTime dateCreated)
        {
            var kek = dateCreated.ToString("yy-MM-dd");
            var kek2 = DateTime.Parse(kek);
            List<Post> lp = (List<Post>)MakeRequest(string.Concat(ws1, "postsDateCreated/", kek), null, "GET", "application/json", typeof(List<Post>));
            return lp;
        }

        public static List<Post> GetPostsByDateFinished(DateTime dateFinished)
        {
            List<Post> lp = (List<Post>)MakeRequest(string.Concat(ws1, "postsDateFinished/", dateFinished), null, "GET", "application/json", typeof(List<Post>));
            return lp;
        }

        public static List<Post> GetPostsByLocation(string location)
        {
            List<Post> lp = (List<Post>)MakeRequest(string.Concat(ws1, "postsLocation/", location), null, "GET", "application/json", typeof(List<Post>));
            return lp;
        }

        public static List<Post> GetPostsByTitle(string title)
        {
            List<Post> lp = (List<Post>)MakeRequest(string.Concat(ws1, "postsTitle/", title), null, "GET", "application/json", typeof(List<Post>));
            return lp;
        }

        public static List<Post> GetPostsByUser(string userName)
        {
            List<Post> lp = (List<Post>)MakeRequest(string.Concat(ws1, "postsUser/", userName), null, "GET", "application/json", typeof(List<Post>));
            return lp;
        }

        public static Post GetPostById(int idPost)
        {
            Post p = (Post)MakeRequest(string.Concat(ws1, "post/", idPost), null, "POST", "application/json", typeof(Post));
            return p;
        }

        public static Post InsertPost(Post u2Add)
        {
            Post p = (Post)MakeRequest(string.Concat(ws1, "post"), u2Add, "POST", "application/json", typeof(Post));
            return p;
        }

        public static Post UpdatePost(Post p2Upd)
        {
            Post p = (Post)MakeRequest(string.Concat(ws1, "post/", p2Upd.Id_Post), p2Upd, "PUT", "application/json", typeof(Post));
            return p;
        }

        public static void DeletePost(int id)
        {
            MakeRequest(string.Concat(ws1, "post/", id), null, "DELETE", null, typeof(void));
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
