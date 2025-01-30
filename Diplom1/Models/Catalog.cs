    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Models
{
    public class Catalog
    {
        public string Title { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
    }
}
