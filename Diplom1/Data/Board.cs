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
    public partial class Board
    {

        public Board()
        {
            this.User = new HashSet<User>();
        }
    
        public int Boards_ID { get; set; }
        public int Catalog_ID { get; set; }
        public string Title { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public int Position { get; set; }
    
        public virtual Catalog Catalog { get; set; }

        public virtual ICollection<User> User { get; set; } = new ObservableCollection<User>();
    }
}
