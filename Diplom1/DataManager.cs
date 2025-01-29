using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Project_Manager.Models;


namespace Project_Manager
{
    public class DataManager

    {
        private const string FilePath = "board.json";

        public void SaveData(Board board)
        {
            var json = JsonConvert.SerializeObject(board, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
        public Board LoadData()

        {
            if (!File.Exists(FilePath))
                return new Board(); // Возвращаем пустую доску, если файл не существует
            var json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<Board>(json);
        }
    }
}
