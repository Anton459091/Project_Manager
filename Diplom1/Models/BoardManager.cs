using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Models
{
    public class BoardManager

    {
        private const string FilePath = "board.json";
        public void SaveBoard(Board board)

        {
            string json = JsonConvert.SerializeObject(board, Formatting.Indented);
            File.WriteAllText(FilePath, json);

        }

        public Board LoadBoard()
        {
            if (!File.Exists(FilePath))

            {
                return new Board(); // Возвращаем новый объект, если файл не существует
            }

            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<Board>(json);
        }
    }
}
