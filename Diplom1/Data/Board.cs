using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Board
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Board()
        {
            this.User = new HashSet<User>();
        }
    
        public int Boards_ID { get; set; }
        public int Catalog_ID { get; set; }
        public ObservableCollection<Catalog> Catalogs { get; set; } = new ObservableCollection<Catalog>();
        public string Title { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public int Position { get; set; }
    
        public virtual Catalog Catalog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> User { get; set; }
    }
}
