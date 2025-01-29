    using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Models
{
    public class Board
    {
        public string BoardName { get; set; }
        public ObservableCollection<Catalog> Catalogs { get; set; } = new ObservableCollection<Catalog>();
    }
}
