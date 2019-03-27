using System.Collections.Generic;

namespace BancDelTemps.Model.Class
{
    public partial class Report
    {

        public int Id_Report { get; set; }
        public string description { get; set; }
        public string is_revised { get; set; }
        public int Post_Id_Post { get; set; }
        public int Id_Reporter { get; set; }
        public int Id_Reported { get; set; }


        public virtual Post Post { get; set; }
    }
}