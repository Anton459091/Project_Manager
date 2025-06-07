using System.Windows;
using System.Windows.Controls;
using Project_Manager.Data;

namespace Project_Manager.UserControls.Authorization
{
    public partial class LoginWindow : Window
    {
        public User CurrentUser { get; set; }
        public bool IsEditMode { get; set; }
        public string LoggedInUsername { get; private set; }


        public LoginWindow()
        {
            InitializeComponent();

            if (IsEditMode)
            {
                Title = "Редактирование профиля";
                UsernameTextBox.Text = CurrentUser?.Login;
                RegisterButton.Content = "Сохранить";
                LoginButton.Visibility = Visibility.Collapsed;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            var user = ProjectRepository.LoadUserLogin(username);

            if (user != null && user.CheckPassword(PasswordBox.Password))
            {
                UserSession.LoggedInUsername = username; // 💾 сохраняем логин
                UserSession.LoggedInUserRole = user.Role;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsEditMode)
            {
                // Редактирование существующего пользователя
                CurrentUser.Login = UsernameTextBox.Text;
                if (!string.IsNullOrEmpty(PasswordBox.Password))
                    CurrentUser.SetPassword(PasswordBox.Password);

                ProjectRepository.SaveUser(CurrentUser);
                DialogResult = true;
                Close();
            }
            else
            {
                // Регистрация нового пользователя
                var regWindow = new RegistrationWindow { Owner = this };
                if (regWindow.ShowDialog() == true)
                {
                    // Загружаем ТОЛЬКО что зарегистрированного пользователя
                    CurrentUser = ProjectRepository.LoadUserLogin(regWindow.UsernameTxtBx.Text);
                    DialogResult = true;
                    Close();
                }
            }
        }
    }
}