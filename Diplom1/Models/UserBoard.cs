using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Models
{
    public class UserBoard
    {
        [Column("ID_Users")]
        public int Users_Id { get; set; }

        [Column("ID_Boards")]
        public int Boards_Id { get; set; }
    }



}
