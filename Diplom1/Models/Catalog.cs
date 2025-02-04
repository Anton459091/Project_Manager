    using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Models
{
    public class Catalog
    {
        public string Name { get; set; }
        public ObservableCollection<Card> Cards { get; set; } = new ObservableCollection<Card>();

        public Catalog() 
        {
            Cards = new ObservableCollection<Card>();
        }

    }
}