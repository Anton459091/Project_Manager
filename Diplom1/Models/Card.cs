using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Models
{
    public class Card
    {
        public Card() { }

        [Column("Cards_ID")]
        public int Cards_Id { get; set; }

        [Column("Title")]
        public string Cards_Title { get; set; }

        [Column("Description")]
        public string Cards_Description { get; set; }

        [Column("Position")]
        public int Cards_Position { get; set; }

    }
}
