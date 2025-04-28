using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Project_Manager.Data; // Ваши модели данных

namespace Project_Manager.UserControls
{
    public partial class MyBoardsControl : UserControl
    {
        // Коллекция досок, которая будет привязана к DataGrid
        public ObservableCollection<Board> Boards { get; set; }

        public MyBoardsControl()
        {
            InitializeComponent();

            // Инициализация коллекции досок
            Boards = new ObservableCollection<Board>();
            // Привязка коллекции к DataGrid
            BoardsDataGrid.ItemsSource = Boards;

            // Загрузка существующих досок из базы данных
            LoadBoards();
        }
        private void AddBoardButton_Click(object sender, RoutedEventArgs e)
        {
            string boardName = "Новая доска"; // Можно добавить логику для получения имени доски

            // Получаем максимальный Id из базы данных, чтобы создать новый с максимальным Id + 1
            int newId = 1;
            using (var context = new ProjectManager_Entities())  // Здесь ApplicationDbContext — это контекст вашего DbContext
            {
                var maxId = context.Board.Max(b => (int?)b.Board_ID) ?? 0; // Получаем максимальный Id, если нет, то используем 0
                newId = maxId + 1;
            }

            // Создаем объект новой доски с новым Id
            Board newBoard = new Board
            {
                Board_ID = newId,
                Title = boardName,
                CreatedAt = DateTime.Now
            };

            // Добавляем новую доску в контекст данных
            using (var context = new ProjectManager_Entities())
            {
                context.Board.Add(newBoard);
                context.SaveChanges();
            }

            // Обновляем UI после добавления доски
            RefreshBoardList();
        }

        private void LoadBoards()
        {
            using (var context = new ProjectManager_Entities())
            {
                var boardsList = context.Board.ToList();
                foreach (var board in boardsList)
                {
                    Boards.Add(board); // Добавление каждой доски в коллекцию
                }
            }
        }

        private void RefreshBoardList()
        {
            using (var context = new ProjectManager_Entities())
            {
                // Получаем все доски
                var boards = context.Board.ToList();

                // Привязываем данные к DataGrid
                BoardsDataGrid.ItemsSource = boards;
            }
        }

        // Обработчик правой кнопки мыши для открытия доски
        private void BoardsDataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var selectedBoard = (Board)BoardsDataGrid.SelectedItem;
            if (selectedBoard != null)
            {
                // Создаем новый BoardControl с выбранной доской
                var boardControl = new BoardControl(selectedBoard);

                // Присваиваем BoardControl в MainContent
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.MainContent.Content = boardControl;
                }
            }
        }



        // Обработчик изменений в DataGrid
        private void BoardsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Получаем редактируемую строку
            var editedBoard = e.Row.Item as Board;
            if (editedBoard != null)
            {
                // Сохраняем изменения только для Title (остальные поля игнорируем)
                using (var context = new ProjectManager_Entities())
                {
                    context.Entry(editedBoard).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }
    }
}
