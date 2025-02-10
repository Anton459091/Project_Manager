using Project_Manager.Models;
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

namespace Project_Manager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPanelOpen = true;
        public string ProjectTitle { get; set; } = "Название проекта";

        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new BoardControl();
            DataContext = this;

        }
        private void ProfileBtn_Click(object sender, RoutedEventArgs e)

        {
            MainContent.Content = new ProfileControl();
        }
        private void MyBoardsBtn_Click(object sender, RoutedEventArgs e)
        {

            MainContent.Content = new MyBoardsControl();
        }
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BoardBtn_Click(object sender, RoutedEventArgs e)
        {

            MainContent.Content = new BoardControl();

        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPanelOpen)
            {
                // Закрыть панель
                AnimatePanel(165, 0);
            }
            else
            {
                // Открыть панель
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
    }
}

/*
Project Manager /
├── App.xaml
├── App.xaml.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── Models /
│   ├── Board.cs
│   ├── Card.cs
│   ├── List.cs
└── UserControls /
    ├── BoardControl.xaml
    ├── CardControl.xaml
    ├── СatalogControl.xaml
    ├── ProfileControl.xaml
    ├── MyBoardsControl.xaml
    └── Controls /
        └── ContextMenuManager.cs
    */