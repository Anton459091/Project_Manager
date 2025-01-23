using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom1.Models
{
    public class Board
    {
        public string Name { get; set; }
        public List<List> Lists { get; set; }

        public Board()
        {
            Lists = new List<List>();
        }
    }
}
