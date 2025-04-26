using System.Windows;
using System.Windows.Controls;
using Project_Manager.Data;

namespace Project_Manager.UserControls.Authorization
{
    public partial class LoginWindow : Window
    {
        public User CurrentUser { get; set; }
        public bool IsEditMode { get; set; }

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
            if (UserRepository.UserExists(UsernameTextBox.Text))
            {
                var user = UserRepository.LoadUser();
                if (user.CheckPassword(PasswordBox.Password))
                {
                    CurrentUser = user;
                    DialogResult = true;
                    Close();
                    return;
                }
            }

            MessageBox.Show("Неверный логин или пароль");
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsEditMode)
            {
                CurrentUser.Login = UsernameTextBox.Text;
                if (!string.IsNullOrEmpty(PasswordBox.Password))
                {
                    CurrentUser.SetPassword(PasswordBox.Password);
                }
                UserRepository.SaveUser(CurrentUser);
                DialogResult = true;
                Close();
            }
            else
            {
                var regWindow = new RegistrationWindow { Owner = this };
                if (regWindow.ShowDialog() == true)
                {
                    CurrentUser = UserRepository.LoadUser();
                    DialogResult = true;
                    Close();
                }
            }
        }
    }
}