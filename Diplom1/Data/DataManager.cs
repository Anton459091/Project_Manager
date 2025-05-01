using Newtonsoft.Json;
using System.IO;
using Project_Manager.Data;
using Project_Manager.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Manager.Data
{
    public class DataManager
    {
        public static void SaveData(BoardControl boardControl, string filePath)
        {
            BoardData boardData = new BoardData { Catalogs = boardControl.Catalogs };

            // Используем JsonSerializer из Newtonsoft.Json
            string jsonData = JsonConvert.SerializeObject(boardData, Newtonsoft.Json.Formatting.Indented);

            // Записываем JSON в файл
            File.WriteAllText(filePath, jsonData);
        }

        public static BoardData LoadData(string filePath)
        {
            try
            {
                // Читаем JSON из файла
                string jsonData = File.ReadAllText(filePath);

                // Десериализуем JSON в объект BoardData
                return JsonConvert.DeserializeObject<BoardData>(jsonData);
            }
            catch
            {
                return null; // Обработка ошибок (например, если файл не существует)
            }
        }
    }
}