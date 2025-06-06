﻿using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json;
using Project_Manager.Data;
using System.Collections.ObjectModel;
using Project_Manager.Models;
using Project_Manager.UserControls.Authorization;

namespace Project_Manager.UserControls
{
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

        private void TextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.DataContext is BoardItem boardItem)
            {
                OnBoardTextBlockClick(boardItem.Path);
            }
        }

        private void LoadUserData()
        {
            _currentUser = UserRepository.LoadUser();
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

        private void OnBoardTextBlockClick(string boardFile)
        {
            try
            {
                var boardData = LoadBoardData(boardFile);
                if (boardData == null)
                {
                    ShowErrorMessage($"Файл {boardFile} поврежден или содержит неверные данные.");
                    return;
                }

                OpenBoardControl(boardFile, boardData);
            }

            catch (FileNotFoundException)
            {
                ShowErrorMessage($"Файл {boardFile} не найден.");
            }

            catch (JsonException ex)
            {
                ShowErrorMessage($"Ошибка при чтении JSON из файла {boardFile}: {ex.Message}");
            }

            catch (Exception ex)
            {
                ShowErrorMessage($"Неизвестная ошибка при открытии файла: {ex.Message}");
            }

        }

        private BoardData LoadBoardData(string boardFile)
        {
            using (var sr = new StreamReader(boardFile))
            {
                string jsonString = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<BoardData>(jsonString);
            }
        }
        private void SaveUserData()
        {
            UserRepository.SaveUser(_currentUser);
        }

        private void OpenBoardControl(string boardFile, BoardData boardData)
        {
            var boardControl = new BoardControl
            {
                CurrentFilePath = boardFile,
                Catalogs = new ObservableCollection<Catalog>(boardData.Catalogs)
            };

            var mainWindow = FindVisualParent<MainWindow>(this);
            var contentControl = FindVisualParent<ContentControl>(this);

            if (mainWindow != null)
            {
                mainWindow.ProjectTitle = Path.GetFileNameWithoutExtension(boardFile);
            }

            if (contentControl != null)
            {
                contentControl.Content = boardControl;
            }

            else
            {
                ShowErrorMessage("Не удалось найти ContentControl.");
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow
            {
                Owner = Window.GetWindow(this),
                IsEditMode = true,
                CurrentUser = _currentUser
            };

            if (loginWindow.ShowDialog() == true)
            {
                _currentUser = loginWindow.CurrentUser;
                UserRepository.SaveUser(_currentUser);
                DataContext = _currentUser;
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