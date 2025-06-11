using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Project_Manager.Data;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;

namespace Project_Manager.UserControls
{
    public partial class MyBoardsControl : UserControl
    {
        private string boardsFolderPath;
        private HashSet<string> _favoriteBoards = new HashSet<string>();
        private string _favoritesFilePath;


        public MyBoardsControl()
        {
            InitializeComponent();

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            boardsFolderPath = Path.Combine(baseDirectory, "Data", "Files");
            _favoritesFilePath = Path.Combine(boardsFolderPath, "favorites.json");

            LoadFavorites();
            LoadSavedBoards();
        }

        //Загрузка досок

        private void LoadSavedBoards()
        {
            if (!Directory.Exists(boardsFolderPath))
            {
                Directory.CreateDirectory(boardsFolderPath);
            }

            // Получаем все файлы досок, исключая favorites.json
            string[] boardFiles = Directory.GetFiles(boardsFolderPath, "*.json").Where(file => !file.EndsWith("favorites.json", StringComparison.OrdinalIgnoreCase)).ToArray();

            MyBoards.Children.Clear();

            // Сортируем доски: избранные вверху
            var sortedBoardFiles = boardFiles.OrderByDescending(file => _favoriteBoards.Contains(file));

            foreach (string boardFile in sortedBoardFiles)
            {
                AddBoardUI(boardFile);
            }
        }

        private void LoadFavorites()
        {
            if (File.Exists(_favoritesFilePath))
            {
                string json = File.ReadAllText(_favoritesFilePath);
                _favoriteBoards = JsonConvert.DeserializeObject<HashSet<string>>(json) ?? new HashSet<string>();
            }
        }


        //Создания интерфейса через код
        private void AddBoardUI(string boardFile)
        {

            Border boardBorder = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fafbfc")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#646f77")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(5)
            };

            Grid boardGrid = new Grid();

            ColumnDefinition col1 = new ColumnDefinition { Width = GridLength.Auto }; // Для звездочки
            ColumnDefinition col2 = new ColumnDefinition { Width = GridLength.Auto }; // Для TextBox
            ColumnDefinition col3 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }; // Для пустого пространства
            ColumnDefinition col4 = new ColumnDefinition { Width = GridLength.Auto }; // Для кнопок

            boardGrid.ColumnDefinitions.Add(col1);
            boardGrid.ColumnDefinitions.Add(col2);
            boardGrid.ColumnDefinitions.Add(col3);
            boardGrid.ColumnDefinitions.Add(col4);


            Button favoriteButton = new Button
            {
                Content = new TextBlock { Text = "⭐", FontSize = 16 },
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Brushes.Gray // По умолчанию звездочка серая
            };

            UpdateFavoriteButton(favoriteButton, boardFile);

            favoriteButton.Click += (sender, e) => FavoriteButton_Click(sender, e, boardFile);

            TextBox boardNameTextBox = new TextBox
            {
                Text = Path.GetFileNameWithoutExtension(boardFile),
                Width = Double.NaN,
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center,
                MaxLength = 100
            };

            StackPanel buttonsPanel = new StackPanel { Orientation = Orientation.Horizontal };

            Button boardButton = new Button
            {
                Content = "Открыть",
                Style = (Style)FindResource("BtnMain"),
                Width = 100,
                Margin = new Thickness(5)
            };

            Button saveButton = new Button
            {
                Content = "Сохранить",
                Style = (Style)FindResource("BtnMain"),
                Width = 100,
                Margin = new Thickness(5)
            };

            Button deleteButton = new Button
            {
                Content = "Удалить",
                Style = (Style)FindResource("BtnMain"),
                Width = 100,
                Margin = new Thickness(5)
            };

            boardButton.Click += (sender, e) => BoardButton_Click(sender, e, boardFile);
            saveButton.Click += (sender, e) => SaveBoardNameButton_Click(sender, e, boardFile, boardNameTextBox.Text);
            deleteButton.Click += (sender, e) => DeleteBoardButton_Click(sender, e, boardFile, boardBorder);

            buttonsPanel.Children.Add(boardButton);
            buttonsPanel.Children.Add(saveButton);
            buttonsPanel.Children.Add(deleteButton);

            Grid.SetColumn(favoriteButton, 0);
            Grid.SetColumn(boardNameTextBox, 1);
            Grid.SetColumn(buttonsPanel, 3);

            boardGrid.Children.Add(favoriteButton);
            boardGrid.Children.Add(boardNameTextBox);
            boardGrid.Children.Add(buttonsPanel);

            boardBorder.Child = boardGrid;

            MyBoards.Children.Add(boardBorder);
        }

        // Кнопочки
        private void AddBoardButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserSession.LoggedInUserRole == 1)
            {
                MessageBox.Show("Недостаточно прав для добавления проекта.");
                return;
            }

            BoardData newBoardData = new BoardData
            {
                Catalogs = new ObservableCollection<Catalog>()
            };

            BoardControl newBoardControl = CreateBoardControlFromData(newBoardData);

            string newFileName = GenerateUniqueFileName("Доска", ".json");
            string newFilePath = System.IO.Path.Combine(boardsFolderPath, newFileName);

            DataManager.SaveData(newBoardControl, newFilePath);

            AddBoardUI(newFilePath);
        }

        private void BoardButton_Click(object sender, RoutedEventArgs e, string boardFile)
        {
            try
            {
                // 1. Читаем данные из файла
                Board boardData;
                using (StreamReader sr = new StreamReader(boardFile))
                {
                    string jsonString = sr.ReadToEnd();
                    boardData = JsonConvert.DeserializeObject<Board>(jsonString); // Десериализуем в объект Board
                }

                if (boardData == null)
                {
                    MessageBox.Show($"Файл {boardFile} поврежден или содержит неверные данные.");
                    return;
                }

                // 2. Создаем BoardControl
                BoardControl boardControl = new BoardControl();
                boardControl.CurrentFilePath = boardFile;
                boardControl.ProjectTitle = boardData.Title; // Заполняем заголовок доски
                boardControl.Catalogs = new ObservableCollection<Catalog>(boardData.Catalog); // Заполняем каталоги

                // 3. Убедимся, что у каждого каталога есть карточки
                foreach (var catalog in boardControl.Catalogs)
                {
                    foreach (var card in catalog.Card)
                    {
                        // Важно: При загрузке карточек также связываем их с каталогом
                        card.Catalog = catalog; // Это нужно для корректного отображения данных
                    }
                }

                // 4. Находим ContentControl и MainWindow
                ContentControl contentControl = FindVisualParent<ContentControl>(this);
                MainWindow mainWindow = FindVisualParent<MainWindow>(this);

                // 5. Устанавливаем заголовок и контент
                if (mainWindow != null)
                {
                    mainWindow.ProjectTitle = System.IO.Path.GetFileNameWithoutExtension(boardFile);
                }
                if (contentControl != null)
                {
                    contentControl.Content = boardControl;
                    mainWindow.SaveButton.Visibility = Visibility.Visible;
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
                File.Delete(filePath);

                MyBoards.Children.Remove(boardBorder);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении файла: {ex.Message}");
            }
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e, string boardFile)
        {
            Button favoriteButton = (Button)sender;

            if (_favoriteBoards.Contains(boardFile))
            {
                // Если доска уже в избранном, удаляем её
                _favoriteBoards.Remove(boardFile);
            }
            else
            {
                // Если доска не в избранном, добавляем её
                _favoriteBoards.Add(boardFile);
            }


            UpdateFavoriteButton(favoriteButton, boardFile);
            SaveFavorites();
            LoadSavedBoards();
        }

        private void UpdateFavoriteButton(Button favoriteButton, string boardFile)
        {
            if (_favoriteBoards.Contains(boardFile))
            {
                favoriteButton.Foreground = Brushes.Gold;
            }
            else
            {
                favoriteButton.Foreground = Brushes.Gray;
            }
        }

        //

        private void SaveFavorites()
        {
            string json = JsonConvert.SerializeObject(_favoriteBoards, Formatting.Indented);
            File.WriteAllText(_favoritesFilePath, json);
        }


        private BoardControl CreateBoardControlFromData(BoardData boardData)
        {
            BoardControl boardControl = new BoardControl();


            if (boardData != null && boardData.Catalogs != null)
            {
                boardControl.Catalogs = new ObservableCollection<Catalog>(boardData.Catalogs);
            }
            else
            {
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