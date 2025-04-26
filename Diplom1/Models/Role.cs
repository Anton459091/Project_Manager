using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Models
{
    public class Role
    {
        [Column("Role_ID")]
        public int Role_Id { get; set; }

        [Column("RoleName")]
        public string Role_Name { get; set; }
    }
}
