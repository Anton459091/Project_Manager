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
using Project_Manager.Models;
using Project_Manager.UserControls;

namespace Project_Manager.UserControls
{
    /// <summary>
    /// Логика взаимодействия для BoardControl.xaml
    /// </summary>
    public partial class BoardControl : UserControl
    {
        private bool _enterKeyPressed = false;

        public BoardControl()
        {
            InitializeComponent();

        }
        private void AddСatalogButton_Click(object sender, RoutedEventArgs e)
        {
            AddСatalogButton.Visibility = Visibility.Collapsed;
            TextBox textBox = new TextBox();
            textBox.Width = 250;
            textBox.LostFocus += TextBox_LostFocus;
            textBox.KeyDown += TextBox_KeyDown;
            AddСatalogStackPanel.Children.Add(textBox);
            textBox.Focus();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _enterKeyPressed = true;
                TextBox textBox = (TextBox)sender;
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                {
                    AddСatalogControl(textBox.Text);
                }
                AddСatalogStackPanel.Children.Remove(textBox);
                AddСatalogButton.Visibility = Visibility.Visible;
                e.Handled = true;
            }
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!string.IsNullOrWhiteSpace(textBox.Text) && !_enterKeyPressed)
            {
                AddСatalogControl(textBox.Text);
            }
            _enterKeyPressed = false;
            AddСatalogStackPanel.Children.Remove(textBox);
            AddСatalogButton.Visibility = Visibility.Visible;
        }
        private void AddСatalogControl(string СatalogName)
        {
            СatalogControl СatalogControl = new СatalogControl();
            СatalogControl.СatalogName = СatalogName;
            СatalogStackPanel.Children.Add(СatalogControl);
        }



    }
}
