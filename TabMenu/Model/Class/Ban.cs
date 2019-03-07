using System;

namespace BancDelTemps.Model.Class
{
    public class Ban
    {
        public int Id_Ban { get; set; }
        public string ban_date { get; set; }
        public string ban_finish_date { get; set; }
        public int UserId_User { get; set; }

        public virtual User User { get; set; }
    }
}