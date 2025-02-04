using Newtonsoft.Json;
using Project_Manager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Data
{
    public class BoardData
    {
        [JsonProperty("Catalogs")] // Указываем имя свойства в JSON
        public ObservableCollection<Catalog> Catalogs { get; set; }

        public BoardData()
        {
            Catalogs = new ObservableCollection<Catalog>();
        }
    }
}
