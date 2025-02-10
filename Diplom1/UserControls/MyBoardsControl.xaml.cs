using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Project_Manager.Data;
using System.Collections.ObjectModel;
using Project_Manager.Models;
using System.Windows.Media;

namespace Project_Manager.UserControls
{
    public partial class MyBoardsControl : UserControl
    {
        private string boardsFolderPath;

        public MyBoardsControl()
        {
            InitializeComponent();

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            boardsFolderPath = System.IO.Path.Combine(baseDirectory, "Data", "Files");

            LoadSavedBoards();
        }

        private void LoadSavedBoards()
        {
            if (!Directory.Exists(boardsFolderPath))
            {
                Directory.CreateDirectory(boardsFolderPath);
            }

            string[] boardFiles = Directory.GetFiles(boardsFolderPath, "*.json");
            MyBoards.Children.Clear(); // Очищаем перед загрузкой

            foreach (string boardFile in boardFiles)
            {
                AddBoardUI(boardFile);
            }
        }

        private void AddBoardUI(string boardFile)
        {
            // Создаем Border для фона, обводки и скругления углов
            Border boardBorder = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f9f6f2")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#646f77")), // Обводка
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10), // Скругление углов
                Margin = new Thickness(5) // Margin для всего элемента
            };

            // Создаем Grid для размещения элементов управления
            Grid boardGrid = new Grid();

            // Определяем колонки
            ColumnDefinition col1 = new ColumnDefinition { Width = GridLength.Auto }; // Для TextBox
            ColumnDefinition col2 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }; // Для пустого пространства
            ColumnDefinition col3 = new ColumnDefinition { Width = GridLength.Auto }; // Для кнопок

            boardGrid.ColumnDefinitions.Add(col1);
            boardGrid.ColumnDefinitions.Add(col2);
            boardGrid.ColumnDefinitions.Add(col3);

            // Создаем StackPanel для кнопок (размещаем их горизонтально)
            StackPanel buttonsPanel = new StackPanel { Orientation = Orientation.Horizontal };

            // Создаем TextBox для названия доски
            TextBox boardNameTextBox = new TextBox
            {
                Text = System.IO.Path.GetFileNameWithoutExtension(boardFile),
                Width = 500,
                Margin = new Thickness(5)
            };

            // Создаем кнопку "Открыть"
            Button boardButton = new Button
            {
                Content = "Открыть",
                Style = (Style)FindResource("BtnMain"),
                Width = 100,
                Margin = new Thickness(5)

            };

            // Создаем кнопку "Сохранить"
            Button saveButton = new Button
            {
                Content = "Сохранить",
                Style = (Style)FindResource("BtnMain"),
                Width = 100,
                Margin = new Thickness(5)
            };

            // Создаем кнопку "Удалить"
            Button deleteButton = new Button
            {
                Content = "Удалить",
                Style = (Style)FindResource("BtnMain"),
                Width = 100,
                Margin = new Thickness(5)
            };

            // Привязываем обработчики событий
            boardButton.Click += (sender, e) => BoardButton_Click(sender, e, boardFile);
            saveButton.Click += (sender, e) => SaveBoardNameButton_Click(sender, e, boardFile, boardNameTextBox.Text);
            deleteButton.Click += (sender, e) => DeleteBoardButton_Click(sender, e, boardFile, boardBorder);

            // Добавляем кнопки в StackPanel
            buttonsPanel.Children.Add(boardButton);
            buttonsPanel.Children.Add(saveButton);
            buttonsPanel.Children.Add(deleteButton);

            // Размещаем элементы в Grid
            Grid.SetColumn(boardNameTextBox, 0);
            Grid.SetColumn(buttonsPanel, 2);

            boardGrid.Children.Add(boardNameTextBox);
            boardGrid.Children.Add(buttonsPanel);

            // Устанавливаем Grid в качестве контента для Border
            boardBorder.Child = boardGrid;

            // Добавляем Border в MyBoards
            MyBoards.Children.Add(boardBorder);
        }
        private void AddBoardButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Создаем пустые данные для новой доски
            BoardData newBoardData = new BoardData
            {
                Catalogs = new System.Collections.ObjectModel.ObservableCollection<Catalog>()
            };

            // 2. Создаем BoardControl из пустых данных
            BoardControl newBoardControl = CreateBoardControlFromData(newBoardData);

            // 3. Генерируем имя файла
            string newFileName = GenerateUniqueFileName("Доска", ".json");
            string newFilePath = System.IO.Path.Combine(boardsFolderPath, newFileName);

            // 4. Сохраняем новую доску в файл
            DataManager.SaveData(newBoardControl, newFilePath);

            // 5. Добавляем элементы UI
            AddBoardUI(newFilePath);
        }

        private void BoardButton_Click(object sender, RoutedEventArgs e, string boardFile)
        {
            try
            {
                // 1. Читаем данные из файла
                BoardData boardData;
                using (StreamReader sr = new StreamReader(boardFile))
                {
                    string jsonString = sr.ReadToEnd();
                    boardData = JsonConvert.DeserializeObject<BoardData>(jsonString);
                }

                if (boardData == null)
                {
                    MessageBox.Show($"Файл {boardFile} поврежден или содержит неверные данные.");
                    return;
                }

                // 2. Создаем BoardControl
                BoardControl boardControl = new BoardControl();
                boardControl.CurrentFilePath = boardFile;
                boardControl.Catalogs = new ObservableCollection<Catalog>(boardData.Catalogs);

                // 3. Находим ContentControl и MainWindow
                ContentControl contentControl = FindVisualParent<ContentControl>(this);
                MainWindow mainWindow = FindVisualParent<MainWindow>(this);

                // 4. Устанавливаем заголовок и контент
                if (mainWindow != null)
                {
                    mainWindow.ProjectTitle = System.IO.Path.GetFileNameWithoutExtension(boardFile);
                }
                if (contentControl != null)
                {
                    contentControl.Content = boardControl;
                }
                else
                {
                    MessageBox.Show("Не удалось найти ContentControl.");
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show($"Файл {boardFile} не найден.");
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Ошибка при чтении JSON из файла {boardFile}: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка при открытии файла: {ex.Message}");
            }
        }

        private void SaveBoardNameButton_Click(object sender, RoutedEventArgs e, string filePath, string newName)
        {
            try
            {
                // 1. Формируем новый путь
                string directory = System.IO.Path.GetDirectoryName(filePath);
                string extension = System.IO.Path.GetExtension(filePath);
                string newFilePath = System.IO.Path.Combine(directory, newName + extension);

                // 2. Переименовываем файл
                File.Move(filePath, newFilePath);

                // 3. Обновляем UI
                MyBoards.Children.Clear();
                LoadSavedBoards();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении имени файла: {ex.Message}");
            }
        }

        private void DeleteBoardButton_Click(object sender, RoutedEventArgs e, string filePath, Border boardBorder)
        {
            try
            {
                // 1. Удаляем файл
                File.Delete(filePath);

                // 2. Удаляем Border (вместе с содержимым) из MyBoards
                MyBoards.Children.Remove(boardBorder);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении файла: {ex.Message}");
            }
        }

        private BoardControl CreateBoardControlFromData(BoardData boardData)
        {
            // Создаем BoardControl
            BoardControl boardControl = new BoardControl();

            // Заполняем BoardControl данными из BoardData
            if (boardData != null && boardData.Catalogs != null)
            {
                boardControl.Catalogs = new ObservableCollection<Catalog>(boardData.Catalogs);
            }
            else
            {
                // Обработка случая, когда boardData или boardData.Catalogs равны null
                MessageBox.Show("Данные доски не загружены корректно.");
            }

            return boardControl;
        }

        private string GenerateUniqueFileName(string baseName, string extension)
        {
            int counter = 1;
            string fileName;

            do
            {
                fileName = $"{baseName}{counter}{extension}";
                counter++;
            }
            while (File.Exists(System.IO.Path.Combine(boardsFolderPath, fileName)));

            return fileName;
        }

        // Вспомогательный метод для поиска родительского элемента определенного типа в визуальном дереве
        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = System.Windows.Media.VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindVisualParent<T>(parentObject);
            }
        }
    }
}