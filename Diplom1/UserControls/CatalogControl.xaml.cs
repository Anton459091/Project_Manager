using Project_Manager.Models;
using Project_Manager.UserControls.Controls;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Project_Manager.UserControls
{
    public partial class СatalogControl : UserControl
    {
        private Point startPoint;
        private bool isDragging = false;
        private bool _enterKeyPressed = false;


        private Card draggedCard;
     
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
               ("Изменить", ContextMenuManager.MakeEditable), 
               ("Удалить", ContextMenuManager.RemoveElement)
           );

            CatalogNameTextBox.LostFocus += CatalogNameTextBox_LostFocus; 
        }
        private void CatalogNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CatalogNameTextBox.IsReadOnly = true; 
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
        private void AddCardsControl(string cardsName)
        {
            Catalog catalog = (Catalog)DataContext;
            Card card = new Card { Title = cardsName, Description = "Описание карточки" };
            catalog.Cards.Add(card);
        }

        // Фокусировка
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


        //Drag and Drop
        private void TransparentBorder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition((UIElement)sender);
            isDragging = true;
        }

        private void TransparentBorder_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && isDragging)
            {
                Point mousePos = e.GetPosition((UIElement)sender);
                Vector diff = startPoint - mousePos;

                // Вычисляем абсолютное значение смещения по X и Y
                double deltaX = Math.Abs(diff.X);
                double deltaY = Math.Abs(diff.Y);

                // Проверяем, достаточно ли большое смещение для начала перетаскивания
                if (deltaX > SystemParameters.MinimumHorizontalDragDistance || deltaY > SystemParameters.MinimumVerticalDragDistance)
                {
                    isDragging = false; 
                    Catalog catalog = (Catalog)DataContext;
                    DataObject dragData = new DataObject(typeof(Catalog), catalog);
                    DragDrop.DoDragDrop(CatalogTextBoxBorder, dragData, DragDropEffects.Move);
                }
            }
        }
        private void CatalogBorder_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Card)))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        private void CatalogBorder_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Card)))
            {
                // Проверяем, что перетаскиваемая карточка не является текущей карточкой
                draggedCard = (Card)e.Data.GetData(typeof(Card));
                if (draggedCard != null && ((Catalog)DataContext).Cards.Contains(draggedCard))
                {
                    e.Effects = DragDropEffects.Move; // Разрешаем перемещение
                }
                else
                {
                    e.Effects = DragDropEffects.Move;  // Разрешаем перемещение
                }
            }
        }

        private void CatalogBorder_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Card)))
            {
                Card card = (Card)e.Data.GetData(typeof(Card));
                Catalog targetCatalog = (Catalog)DataContext; 
                ObservableCollection<Card> cards = targetCatalog.Cards; 


                var boardControl = FindVisualParent<BoardControl>(this);

                if (boardControl != null)
                {
                    foreach (Catalog catalog in boardControl.Catalogs)
                    {
                        if (catalog != targetCatalog && catalog.Cards.Contains(card))
                        {
                            catalog.Cards.Remove(card);
                            break;
                        }
                    }
                }

                int dropIndex = GetDropIndex(CardItemsControl, e.GetPosition(CardItemsControl));

                if (dropIndex >= 0 && dropIndex <= cards.Count)
                {
                    if (dropIndex == cards.Count)
                    {
                        cards.Remove(card);
                        cards.Add(card);
                    }
                    else
                    {
                        if (!cards.Contains(card))
                        {
                            cards.Insert(dropIndex, card); 
                        }
                        else
                        {
                            cards.Remove(card);
                            cards.Insert(dropIndex, card);
                        }
                    }
                }
            }
        }

            private int GetDropIndex(ItemsControl itemsControl, Point dropPosition)
        {
            int index = 0;
            for (int i = 0; i < itemsControl.Items.Count; i++)
            {
                if (itemsControl.ItemContainerGenerator.ContainerFromIndex(i) is UIElement itemContainer && itemContainer is FrameworkElement item)
                {
                    Point position = dropPosition;
                    double itemCenter = item.TranslatePoint(new Point(item.ActualWidth / 2, item.ActualHeight / 2), itemsControl).Y;
                    if (position.Y < itemCenter)
                    {
                        index = i;
                        break;
                    }
                    else
                    {
                        index = i + 1;
                    }
                }
            }
            return index;
        }

        // Вспомогательный метод для поиска родительского элемента определенного типа в визуальном дереве
        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindVisualParent<T>(parentObject);
            }
        }


    }
}