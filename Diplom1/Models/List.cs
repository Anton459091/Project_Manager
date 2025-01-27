using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Models
{
    public class List
    {
        public string Name { get; set; }
        public List<Card> Cards { get; set; }
        public List()
        {
            Cards = new List<Card>();
        }
    }
}
