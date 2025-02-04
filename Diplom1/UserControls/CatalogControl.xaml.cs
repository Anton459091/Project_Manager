using Project_Manager.Models;
using Project_Manager.UserControls.Controls;
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
        private ContextMenuManager _menuManager = new ContextMenuManager();

        public СatalogControl()
        {
            InitializeComponent();
            Loaded += СatalogControl_Loaded;
        }
        private void СatalogControl_Loaded(object sender, RoutedEventArgs e)
        {
            _menuManager.AttachMenu(MenuButton, this,
               ("Изменить", ContextMenuManager.MakeEditable), // Добавляем пункт "Изменить"
               ("Удалить", ContextMenuManager.RemoveElement)
           );

            CatalogNameTextBox.LostFocus += CatalogNameTextBox_LostFocus; // Подписываемся на событие LostFocus
        }
        private void CatalogNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CatalogNameTextBox.IsReadOnly = true; // Делаем TextBox снова ReadOnly
        }

        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            AddCardButton.Visibility = Visibility.Collapsed;
            TextBox textBox = new TextBox();
            textBox.MinWidth = 170;
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
            // Получаем Catalog из DataContext
            Catalog catalog = (Catalog)DataContext;

            // Создаем объект Card и добавляем его в коллекцию Cards объекта Catalog
            Card card = new Card { Title = cardsName, Description = "Описание карточки" };
            catalog.Cards.Add(card);
        }
    }
}