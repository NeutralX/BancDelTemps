using System;

namespace BancDelTemps.Model.Class
{
    public class Pact
    {
        public int Id_Pact { get; set; }
        public DateTime? date_created { get; set; }
        public DateTime? date_finished { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public int Posts_Id_Post { get; set; }
        public int Reports_Id_Report { get; set; }

        public virtual Post Post { get; set; }
        public virtual Report Report { get; set; }
    }
}