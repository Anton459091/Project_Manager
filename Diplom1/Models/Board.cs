    using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Models
{
    public class Board
    {
        [Column("Boards_ID")]
        public int Board_Id { get; set; }

        [Column("Catalog_ID")]
        public ObservableCollection<Catalog> Catalogs { get; set; } = new ObservableCollection<Catalog>(); //  связь

        [Column("Title")]
        public string Board_Title { get; set; }

        [Column("Position")]
        public int Boards_Position { get; set; }

        [Column("Created_at")]
        public DateTime Boards_CreatedAt { get; set;}

        
    }
}
