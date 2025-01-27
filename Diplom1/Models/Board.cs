using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Models
{
    public class Board
    {
        public string Name { get; set; }
        public List<Сatalog> Lists { get; set; }

        public Board()
        {
            Lists = new List<Сatalog>();
        }
    }
}
