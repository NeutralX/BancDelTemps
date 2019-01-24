using System.Collections.Generic;

namespace BancDelTemps.Model.Class
{
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Bans = new HashSet<Ban>();
            this.Posts = new HashSet<Post>();
        }

        public int Id_User { get; set; }
        public string name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public System.DateTime register_date { get; set; }
        public double time_hours { get; set; }
        public string password { get; set; }
        public System.DateTime date_of_birth { get; set; }
        public string gender { get; set; }
        public string picture_path { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ban> Bans { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }
    }
}