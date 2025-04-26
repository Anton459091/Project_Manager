using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Card
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Card()
        {
            this.Catalog = new HashSet<Catalog>();
        }
    
        public int Cards_ID { get; set; }
        public int Position { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Catalog> Catalog { get; set; }
    }
}
