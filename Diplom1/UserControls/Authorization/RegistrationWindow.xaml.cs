using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Project_Manager.Data;

namespace Project_Manager.UserControls.Authorization
{
    public partial class RegistrationWindow : Window
    {
        public string AvatarPath { get; private set; }
        public string RegisteredUsername { get; private set; }


        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void UploadPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Title = "Выберите аватар"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                AvatarPath = openFileDialog.FileName;
                ProfileImage.Source = new BitmapImage(new System.Uri(AvatarPath));
            }
        }
private void RegisterButton_Click(object sender, RoutedEventArgs e)
{
    string username = UsernameTxtBx.Text;
    
    // Проверка на пустое имя пользователя
    if (string.IsNullOrWhiteSpace(username))
    {
        ShowError("Логин не может быть пустым");
        return;
    }
    
    if (ProjectRepository.UserExists(username))
    {
        ShowError("Пользователь с таким логином уже существует");
        return;
    }

    // Проверка пароля (можно добавить дополнительные проверки)
    if (string.IsNullOrWhiteSpace(PasswordBox.Password))
    {
        ShowError("Пароль не может быть пустым");
        return;
    }

    var newUser = new User
    {
        Login = username, // Используем очищенное имя пользователя
        PhotoPath = AvatarPath ?? "/Resources/default_avatar.png"
    };
    newUser.SetPassword(PasswordBox.Password);

    ProjectRepository.SaveUser(newUser);
    UserSession.LoggedInUsername = username; // Сохраняем логин в сессии, как при входе
    DialogResult = true;
    Close();
}

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }
    }
}