using System;
using System.Collections.Generic;

namespace BancDelTemps.Model.Class
{
    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            this.Pacts = new HashSet<Pact>();
            this.Reports = new HashSet<Report>();
        }

        public int Id_Post { get; set; }
        public System.DateTime date_created { get; set; }
        public Nullable<System.DateTime> date_finished { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string title { get; set; }
        public int UserId_User { get; set; }
        public int Category_Id_Category { get; set; }

        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pact> Pacts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report> Reports { get; set; }
        public virtual User User { get; set; }
    }
}