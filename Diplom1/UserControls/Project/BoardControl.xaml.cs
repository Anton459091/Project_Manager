using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;
using Microsoft.Win32;
using Project_Manager.Data;
using Project_Manager.UserControls;
using Project_Manager.UserControls.Controls;

namespace Project_Manager.UserControls
{
    /// <summary>
    /// Логика взаимодействия для BoardControl.xaml
    /// </summary>
    public partial class BoardControl : UserControl
    {
        
        private bool _enterKeyPressed = false;    
        public ObservableCollection<Catalog> Catalogs { get; set; } = new ObservableCollection<Catalog>();
        public string CurrentFilePath { get; set; }

        public BoardControl()
        {
            InitializeComponent();
            DataContext = this;
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
            Catalogs.Add(new Catalog { Title = catalogName, Cards = new ObservableCollection<Card>() });
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

        //Сохранение

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S) // Ctrl + S
            {

            }
        }

        //Drag and Drop

        private int GetDropIndex(ItemsControl itemsControl, Point dropPosition, Catalog catalog) // index-1 < item > index+1
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
                            return -1; // Запретить перенос на самого себя
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
                ObservableCollection<Catalog> catalogs = Catalogs;
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
                }
            }
        }

        // Вспомогательный метод для поиска родительского элемента определенного типа в визуальном дереве
        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = System.Windows.Media.VisualTreeHelper.GetParent(child);

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
