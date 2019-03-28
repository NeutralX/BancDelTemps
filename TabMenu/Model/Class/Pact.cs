using System;

namespace BancDelTemps.Model.Class
{
    public class Pact
    {
        public int Id_Pact { get; set; }
        public string date_created { get; set; }
        public string date_finished { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public int Posts_Id_Post { get; set; }
        public int Id_Author { get; set; }
        public int Id_NoAuthor { get; set; }

        public virtual Post Post { get; set; }
    }
}