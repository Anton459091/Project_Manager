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
using Project_Manager.UserControls;
using Project_Manager.UserControls.Controls;

namespace Project_Manager.UserControls
{
    public partial class СatalogControl : UserControl
    {
        private ContextMenuManager _menuManager = new ContextMenuManager();
        private bool _enterKeyPressed = false;

        public СatalogControl()
        {
            InitializeComponent();
            _menuManager.AttachMenu(MenuButton, this,
            ("Удалить", ContextMenuManager.RemoveElement)
            );
        }

        public string СatalogName
        {
            get { return СatalogNameTextBox.Text; }
            set { СatalogNameTextBox.Text = value; }
        }

        private void СatalogBorder_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(CardControl)) is CardControl cardControl)
            {
                int index = cardControl.GetDropIndex(e);
                if (index == -1)
                {
                    if (cardControl.Parent is StackPanel oldPanel)
                    {
                        oldPanel.Children.Remove(cardControl);
                    }
                    CardsStackPanel.Children.Add(cardControl);
                }
                else
                {
                    if (cardControl.Parent is StackPanel oldPanel)
                    {
                        oldPanel.Children.Remove(cardControl);
                    }
                    CardsStackPanel.Children.Insert(index, cardControl);
                }
            }
        }


        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            AddCardButton.Visibility = Visibility.Collapsed;
            TextBox textBox = new TextBox();
            textBox.LostFocus += TextBox_LostFocus;
            textBox.KeyDown += TextBox_KeyDown;
            AddCardsStackPanel.Children.Insert(1, textBox);
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
                    AddCardsControl(textBox.Text);
                }
                AddCardsStackPanel.Children.Remove(textBox);
                AddCardButton.Visibility = Visibility.Visible;
                e.Handled = true;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!string.IsNullOrWhiteSpace(textBox.Text) && !_enterKeyPressed)
            {
                AddCardsControl(textBox.Text);
            }
            _enterKeyPressed = false;
            AddCardsStackPanel.Children.Remove(textBox);
            AddCardButton.Visibility = Visibility.Visible;
        }

        private void AddCardsControl(string cardsName)
        {
            CardControl cardControl = new CardControl();
            cardControl.Title = cardsName;
            CardsStackPanel.Children.Insert(0, cardControl);
        }

    }
}