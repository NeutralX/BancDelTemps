﻿namespace BancDelTemps.Model.Class
{
    public partial class Admin
    {
        public Admin(string username, string password)
        {
            this.Id_Admin = 1;
            this.username = username;
            this.password = password;
        }

        public int Id_Admin { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}