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
        public Catalog()
        {
            this.Board = new HashSet<Board>();
        }
    
        public int Catalog_ID { get; set; }
        public int Cards_ID { get; set; }
        public string Title { get; set; }
        public int Position { get; set; }

        public virtual ICollection<Board> Board { get; set; } = new ObservableCollection<Board>();

        public ObservableCollection<Card> Cards { get; set; } = new ObservableCollection<Card>();
        public virtual Card Card { get; set; }
    }
}
