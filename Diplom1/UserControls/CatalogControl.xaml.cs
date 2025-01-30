using Project_Manager.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Project_Manager.UserControls
{
    public partial class СatalogControl : UserControl
    {
        private bool _enterKeyPressed = false;

        public ObservableCollection<Card> Cards { get; set; } = new ObservableCollection<Card>();

        public string СatalogName
        {
            get { return CatalogNameTextBox.Text; }
            set { CatalogNameTextBox.Text = value; }
        }

        public СatalogControl()
        {
            InitializeComponent();
            DataContext = this; // Устанавливаем DataContext для привязки
        }

        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            AddCardButton.Visibility = Visibility.Collapsed;
            TextBox textBox = new TextBox();
            textBox.LostFocus += TextBox_LostFocus;
            textBox.KeyDown += TextBox_KeyDown;
            CatalogStackPanel.Children.Add(textBox);
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
                CatalogStackPanel.Children.Remove(textBox);
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
            CatalogStackPanel.Children.Remove(textBox);
            AddCardButton.Visibility = Visibility.Visible;
        }

        private void AddCardsControl(string cardsName)
        {
            // Создаем объект Card и добавляем его в коллекцию
            Cards.Add(new Card { Title = cardsName, Description = "Описание карточки" });

        }



    }
}