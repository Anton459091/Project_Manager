using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.Entity;
using Project_Manager.Data; // Для контекста и сущностей

namespace Project_Manager.UserControls
{
    public partial class BoardControl : UserControl
    {
        private bool _enterKeyPressed = false;
        private ProjectManager_Entities _context = new ProjectManager_Entities();
        public Board CurrentBoard { get; set; }
        public string CurrentFilePath { get; set; }

        public BoardControl(Board board )
        {
            InitializeComponent();
            CurrentBoard = board;
            DataContext = CurrentBoard;
            LoadData();
        }

        private void LoadData()
        {
            // Загружаем данные каталога с карточками из базы данных
            var catalogs = _context.Catalog.Include(c => c.Card).ToList();
            CatalogItemsControl.ItemsSource = catalogs; // Привязываем данные к контролу отображения
        }

        private void AddСatalogButton_Click(object sender, RoutedEventArgs e)
        {
            AddСatalogButton.Visibility = Visibility.Collapsed;
            TextBox textBox = new TextBox();
            textBox.Width = 250;
            textBox.LostFocus += TextBox_LostFocus;
            textBox.KeyDown += TextBox_KeyDown;
            BoardStackPanel.Children.Add(textBox);
            textBox.Focus();
        }

        private void AddСatalogControl(string catalogName)
        {
            var catalog = new Catalog { Title = catalogName };
            _context.Catalog.Add(catalog);
            _context.SaveChanges();

            // После добавления нового каталога обновляем UI
            LoadData(); // Повторно загружаем данные из базы данных
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
                BoardStackPanel.Children.Remove(textBox);
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
            BoardStackPanel.Children.Remove(textBox);
            AddСatalogButton.Visibility = Visibility.Visible;
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S) // Ctrl + S
            {
                // Сохранение всех изменений в базе данных
                _context.SaveChanges();
            }
        }

        // Для работы с перетаскиванием элементов
        private int GetDropIndex(ItemsControl itemsControl, Point dropPosition, Catalog catalog)
        {
            int index = 0;
            for (int i = 0; i < itemsControl.Items.Count; i++)
            {
                if (itemsControl.ItemContainerGenerator.ContainerFromIndex(i) is UIElement itemContainer && itemContainer is FrameworkElement item)
                {
                    Point position = dropPosition;
                    double itemCenter = item.TranslatePoint(new Point(item.ActualWidth / 2, item.ActualHeight / 2), itemsControl).X;

                    if (position.X < itemCenter)
                    {
                        if (catalog == itemsControl.Items[i])
                        {
                            return -1;
                        }
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

        private void CatalogItemsControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Catalog)))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        private void CatalogItemsControl_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Catalog)))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        private void CatalogItemsControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Catalog)))
            {
                Catalog catalog = (Catalog)e.Data.GetData(typeof(Catalog));
                var catalogs = _context.Catalog.ToList(); // Загрузка всех каталогов из БД
                int index = GetDropIndex(CatalogItemsControl, e.GetPosition(CatalogItemsControl), catalog);

                if (catalog != null && catalogs != null && index != -1)
                {
                    int oldIndex = catalogs.IndexOf(catalog);

                    if (oldIndex == -1) return;

                    catalogs.Remove(catalog);

                    if (index > oldIndex)
                    {
                        index--;
                    }

                    if (index > catalogs.Count)
                    {
                        index = catalogs.Count;
                    }

                    catalogs.Insert(index, catalog);
                    _context.SaveChanges(); // Обновление БД после изменения порядка
                    LoadData(); // Обновляем UI
                }
            }
        }
    }
}
