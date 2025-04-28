using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json;
using Project_Manager.Data;
using System.Collections.ObjectModel;
using Project_Manager.UserControls.Authorization;

namespace Project_Manager.UserControls
{
    public static class UserSession
    {
        public static string LoggedInUsername { get; set; }
    }

    public class BoardItem
    {
        public string Path { get; set; }
        public string Name { get; set; }
    }

    public partial class ProfileControl : UserControl
    {

        private User _currentUser;
        private readonly string _userDataPath;
        private readonly string boardsFolderPath;

        public ProfileControl()
        {
            InitializeComponent();
            boardsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Files");
            _userDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "user.json");

            LoadUserData();
            LoadBoards();
            DataContext = _currentUser;
        }


        private void LoadUserData()
        {
            if (!string.IsNullOrEmpty(UserSession.LoggedInUsername))
            {
                _currentUser = ProjectRepository.LoadUser(UserSession.LoggedInUsername);
            }
            else
            {
                _currentUser = new User { Login = "Гость" };
            }

            DataContext = _currentUser;
        }

        private void LoadBoards()
        {
            EnsureBoardsDirectoryExists();

            var boardFiles = GetBoardFiles();
            var boardItems = boardFiles.Select(file => new BoardItem
            {
                Path = file,
                Name = Path.GetFileNameWithoutExtension(file)
            }).ToList();

            BoardsItemsControl.ItemsSource = boardItems;
        }

        private void EnsureBoardsDirectoryExists()
        {
            if (!Directory.Exists(boardsFolderPath))
            {
                Directory.CreateDirectory(boardsFolderPath);
            }
        }

        private string[] GetBoardFiles()
        {
            return Directory.GetFiles(boardsFolderPath, "*.json")
                .Where(file => !file.EndsWith("favorites.json", StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }


        private BoardData LoadBoardData(string boardFile)
        {
            using (var sr = new StreamReader(boardFile))
            {
                string jsonString = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<BoardData>(jsonString);
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditWindow(_currentUser)
            {
                Owner = Window.GetWindow(this)
            };

            if (editWindow.ShowDialog() == true)
            {
                try
                {
                    _currentUser = editWindow.CurrentUser;
                    ProjectRepository.SaveUser(_currentUser);

                    DataContext = null;
                    DataContext = _currentUser;

                    if (!string.IsNullOrEmpty(_currentUser.PhotoPath))
                    {
                        // Ваш код для обновления изображения профиля
                    }

                    MessageBox.Show("Профиль успешно обновлен!", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var confirmResult = MessageBox.Show("Вы уверены, что хотите выйти из аккаунта?",
                                                "Подтверждение выхода",
                                                MessageBoxButton.YesNo,
                                                MessageBoxImage.Question);

            if (confirmResult != MessageBoxResult.Yes)
                return;

            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Hide(); // временно скрываем главное окно

                var loginWindow = new LoginWindow();
                bool? result = loginWindow.ShowDialog(); // показываем как диалог

                if (result == true)
                {
                    // Переоткрываем новое главное окно
                    var newMainWindow = new MainWindow();
                    newMainWindow.Show();
                }
                else
                {
                    // Если логин неудачен — вырубаем всё приложение
                    Application.Current.Shutdown();
                }

                // Закрываем старое окно (можно позже, если не нужно)
                mainWindow.Close();
            }
        }


        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            return parentObject is T parent ? parent : FindVisualParent<T>(parentObject);
        }
    }

}