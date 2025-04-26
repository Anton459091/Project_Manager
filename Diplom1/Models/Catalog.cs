    using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Models
{
    public class Catalog
    { 
        public Catalog()
        {
            Cards = new ObservableCollection<Card>();
        }

        //[Column("Cards_ID")]
        public ObservableCollection<Card> Cards { get; set; } = new ObservableCollection<Card>(); // public int Cards_Id { get; set; }
        
        [Column("Catalog_ID")]
        public int Catalog_Id { get; set; }

        [Column("Title")]
        public string Catalog_Title { get; set; }

        [Column("Position")]
        public int Catalog_Position { get; set; }

    }
}