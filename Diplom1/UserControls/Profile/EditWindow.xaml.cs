using Microsoft.Win32;
using Project_Manager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Project_Manager.UserControls.Authorization
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public User CurrentUser { get; set; }
        private string _newPhotoPath;

        public EditWindow(User user)
        {
            InitializeComponent();


            CurrentUser = user;
            DataContext = CurrentUser;

            if (!string.IsNullOrEmpty(CurrentUser.PhotoPath))
            {
                LoadProfileImage(CurrentUser.PhotoPath);
            }

        }
        private void LoadProfileImage(string path)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(path);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                ProfileImage.Source = bitmap;
            }
            catch
            {
                ProfileImage.Source = new BitmapImage(
                    new Uri("pack://application:,,,/Resources/default_avatar.png"));
            }
        }


        private void UploadPhoto_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Title = "Выберите аватар"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _newPhotoPath = openFileDialog.FileName;
                LoadProfileImage(_newPhotoPath);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NewPasswordBox.Password))
            {
                if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
                {
                    MessageBox.Show("Пароли не совпадают!", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                CurrentUser.SetPassword(NewPasswordBox.Password);
            }

            if (!string.IsNullOrEmpty(_newPhotoPath))
            {
                CurrentUser.PhotoPath = _newPhotoPath;
            }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
