using Newtonsoft.Json;
using Project_Manager.Data;
using Project_Manager.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Entity;


namespace Project_Manager.Data
{
    public class BoardData
    {
        [JsonProperty("BoardName")]
        public string BoardName { get; set; }

        [JsonProperty("Catalogs")] // Указываем имя свойства в JSON
        public ObservableCollection<Catalog> Catalogs { get; set; }

        public BoardData()
        {
            Catalogs = new ObservableCollection<Catalog>();
        }
    }

    public class DataManager
    {
        public static string SaveBoardDataToJson(int boardId)
        {
            try
            {
                using (var db = new ProjectManager_Entities())
                {
                    // Получаем доску по ID
                    var board = db.Board.Include(b => b.Catalog)
                                        .FirstOrDefault(b => b.Board_ID == boardId);

                    if (board == null) return null;

                    // Создаем объект BoardData из сущности Board
                    BoardData boardData = new BoardData
                    {
                        BoardName = board.Title,
                        Catalogs = new ObservableCollection<Catalog>(board.Catalog) // Получаем каталоги
                    };

                    // Сериализуем в JSON
                    string jsonData = JsonConvert.SerializeObject(boardData, Formatting.Indented);
                    return jsonData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
                return null;
            }
        }
        // Метод для сохранения данных доски в JSON
        public static void SaveData(BoardControl boardControl, string filePath)
        {
            // Создаем объект BoardData из данных доски
            BoardData boardData = new BoardData
            {
                BoardName = boardControl.ProjectTitle,  // Используем ProjectTitle вместо Title
                Catalogs = new ObservableCollection<Catalog>(boardControl.Catalogs)  // Передаем каталоги
            };

            // Используем JsonSerializer для сериализации объекта в строку
            string jsonData = JsonConvert.SerializeObject(boardData, Formatting.Indented);

            // Сохраняем JSON строку в файл
            File.WriteAllText(filePath, jsonData);
        }


        public static void LoadBoardData(string jsonData)
        {
            try
            {
                // Десериализуем JSON в объект BoardData
                BoardData boardData = JsonConvert.DeserializeObject<BoardData>(jsonData);

                if (boardData == null) return;

                using (var db = new ProjectManager_Entities())
                {
                    // Создаем новую доску
                    Board board = new Board
                    {
                        Title = boardData.BoardName,
                        CreatedAt = DateTime.Now,
                        Position = 0 // позиция по умолчанию
                    };

                    db.Board.Add(board);
                    db.SaveChanges(); // Сохраняем доску

                    // Сохраняем каталоги для этой доски
                    foreach (var catalog in boardData.Catalogs)
                    {
                        // Создаем объект каталога
                        Catalog catalogEntity = new Catalog
                        {
                            Board_ID = board.Board_ID, // связываем с доской
                            Title = catalog.Title,
                            Position = 0 // позиция по умолчанию
                        };

                        db.Catalog.Add(catalogEntity);
                    }
                    db.SaveChanges(); // Сохраняем каталоги в БД
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        public static void AddUserToBoard(int boardId, int userId)
        {
            using (var db = new ProjectManager_Entities())
            {
                var board = db.Board.Include(b => b.User)
                                    .FirstOrDefault(b => b.Board_ID == boardId);
                var user = db.User.FirstOrDefault(u => u.User_ID == userId);

                if (board != null && user != null)
                {
                    // Добавляем пользователя в доску
                    board.User.Add(user);
                    db.SaveChanges(); // Сохраняем изменения
                }
            }
        }

    }

}
