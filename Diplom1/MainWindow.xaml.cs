using Project_Manager.Data;
using Project_Manager;
using Project_Manager.UserControls;
using Project_Manager.UserControls.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using Project_Manager.UserControls.Profile;

namespace Project_Manager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool isPanelOpen = true;
        private string _projectTitle = "Мои доски";


        public string ProjectTitle
        {
            get => _projectTitle;
            set
            {
                _projectTitle = value;
                OnPropertyChanged(nameof(ProjectTitle));
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new MyBoardsControl();
            DataContext = this;

        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void UpdateTitleBasedOnContent()
        {
            if (MainContent.Content is ProfileControl)
            {
                ProjectTitle = "Профиль";
                SaveButton.Visibility = Visibility.Collapsed; // Скрыть кнопку
            }
            else if (MainContent.Content is MyBoardsControl)
            {
                ProjectTitle = "Мои доски";
                SaveButton.Visibility = Visibility.Collapsed; // Скрыть кнопку
            }
            else if (MainContent.Content is AdminControl)
            {
                ProjectTitle = "Панель Администратора";
                SaveButton.Visibility = Visibility.Collapsed; // Скрыть кнопку
            }
            else if (MainContent.Content is BoardControl)
            {
                ProjectTitle = "Название проекта";
                SaveButton.Visibility = Visibility.Visible; // Показать кнопку
            }
            else
            {
                ProjectTitle = "Название проекта";
                SaveButton.Visibility = Visibility.Collapsed; // Скрыть кнопку
            }
        }

        private void ProfileBtn_Click(object sender, RoutedEventArgs e)

        {
            MainContent.Content = new ProfileControl();
            UpdateTitleBasedOnContent();
        }

        private void MyBoardsBtn_Click(object sender, RoutedEventArgs e)
        {

            MainContent.Content = new MyBoardsControl();
            UpdateTitleBasedOnContent();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BoardBtn_Click(object sender, RoutedEventArgs e)
        {

            MainContent.Content = new BoardControl();
            UpdateTitleBasedOnContent();
        }


        private void AdminBtn_Click(object sender, RoutedEventArgs e)
        {

            MainContent.Content = new AdminControl();
            UpdateTitleBasedOnContent();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPanelOpen)
            {
                AnimatePanel(165, 0);
            }

            else
            {
                AnimatePanel(0, 165);
            }

            isPanelOpen = !isPanelOpen;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            AnimatePanel(0, 200);
            isPanelOpen = false;
        }

        private void AnimatePanel(double from, double to)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromMilliseconds(300))
            };

            SidePanel.BeginAnimation(WidthProperty, animation);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainContent.Content is BoardControl boardControl)
            {
                boardControl.SaveProject();
            }
        }

    }
}
