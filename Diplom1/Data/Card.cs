using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_Manager.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Card
    {
        public Card()
        {
            this.Catalog = new HashSet<Catalog>();
        }
    
        public int Cards_ID { get; set; }
        public int Position { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<Catalog> Catalog { get; set; } = new ObservableCollection<Catalog>();
    }
}
