using System;

namespace BancDelTemps.Model.Class
{
    public class Ban
    {
        public int Id_Ban { get; set; }
        public System.DateTime ban_date { get; set; }
        public DateTime? ban_finish_date { get; set; }
        public int UserId_User { get; set; }

        public virtual User User { get; set; }
    }
}