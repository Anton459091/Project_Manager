using System;
using System.IO;
using System.Text.Json;
using Diplom1.Models;

namespace Diplom1.Services
{
    public class JsonDataService
    {
        private readonly string _filePath;

        public JsonDataService(string filePath)
        {
            _filePath = filePath;
            if (!Directory.Exists(Path.GetDirectoryName(_filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            }
        }

        public void SaveData(Board board)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(board, options);
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving {ex.Message}");
            }
        }

        public Board LoadData()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string jsonString = File.ReadAllText(_filePath);
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<Board>(jsonString, options);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading {ex.Message}");
            }
            return new Board();
        }
    }
}