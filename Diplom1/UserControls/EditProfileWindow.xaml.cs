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
using Microsoft.Win32;
using Project_Manager.Models;

namespace Project_Manager.UserControls
{
    public partial class EditProfileWindow : Window
    {
        public User EditedUser { get; private set; }

        public EditProfileWindow(User user)
        {
            InitializeComponent();
            EditedUser = new User
            {
                Login = user.Login,
                Description = user.Description,
                PhotoPath = user.PhotoPath
            };
            DataContext = EditedUser;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void UploadPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg",
                Title = "Выберите фото профиля"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Сохраняем путь к выбранному файлу
                EditedUser.PhotoPath = openFileDialog.FileName;

                // Обновляем изображение в интерфейсе
                ProfileImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
    }
}