using System.Collections.Generic;

namespace BancDelTemps.Model.Class
{
    public partial class Report
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Report()
        {
            this.Pacts = new HashSet<Pact>();
        }

        public int Id_Report { get; set; }
        public string description { get; set; }
        public string is_revised { get; set; }
        public int Post_Id_Post { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pact> Pacts { get; set; }
        public virtual Post Post { get; set; }
    }
}