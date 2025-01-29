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

namespace Project_Manager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new BoardControl();
            
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