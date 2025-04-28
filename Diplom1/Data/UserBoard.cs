using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Project_Manager.Data
{
    public class UserBoard
    {
        [Key, Column(Order = 0)]
        [ForeignKey("User")]
        public int Users_ID { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Board")]
        public int Boards_ID { get; set; }

        public virtual User User { get; set; }
        public virtual Board Board { get; set; }
    }
}
