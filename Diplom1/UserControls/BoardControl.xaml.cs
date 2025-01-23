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
using Diplom1.UserControls;

namespace Diplom1.UserControls
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
        private void AddListButton_Click(object sender, RoutedEventArgs e)
        {
            AddListButton.Visibility = Visibility.Collapsed;
            TextBox textBox = new TextBox();
            textBox.Width = 250;
            textBox.LostFocus += TextBox_LostFocus;
            textBox.KeyDown += TextBox_KeyDown;
            AddListsStackPanel.Children.Add(textBox);
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
                    AddListControl(textBox.Text);
                }
                AddListsStackPanel.Children.Remove(textBox);
                AddListButton.Visibility = Visibility.Visible;
                e.Handled = true;
            }
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!string.IsNullOrWhiteSpace(textBox.Text) && !_enterKeyPressed)
            {
                AddListControl(textBox.Text);
            }
            _enterKeyPressed = false;
            AddListsStackPanel.Children.Remove(textBox);
            AddListButton.Visibility = Visibility.Visible;
        }
        private void AddListControl(string listName)
        {
            ListControl listControl = new ListControl();
            listControl.ListName = listName;
            ListsStackPanel.Children.Add(listControl);
        }

    }
}
