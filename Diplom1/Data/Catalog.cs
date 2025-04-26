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
    
    public partial class Catalog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Catalog()
        {
            this.Board = new HashSet<Board>();
        }
    
        public int Catalog_ID { get; set; }
        public int Cards_ID { get; set; }
        public ObservableCollection<Card> Cards { get; set; } = new ObservableCollection<Card>();
        public string Title { get; set; }
        public int Position { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Board> Board { get; set; }
        public virtual Card Card { get; set; }
    }
}
