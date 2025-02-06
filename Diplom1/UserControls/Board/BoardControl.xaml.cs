using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.Win32;
using Project_Manager.Data;
using Project_Manager.Models;
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
        public ObservableCollection<Catalog> Catalogs { get; set; } = new ObservableCollection<Catalog>(); // Коллекция Catalog

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
        private void AddСatalogControl(string catalogName)
        {
            Catalogs.Add(new Catalog { Name = catalogName, Cards = new ObservableCollection<Card>() });
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                DataManager.SaveData(this, saveFileDialog.FileName);
                MessageBox.Show("Данные сохранены!");
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                BoardData loadedData = DataManager.LoadData(openFileDialog.FileName);
                if (loadedData != null)
                {
                    Catalogs.Clear(); // Очищаем текущую коллекцию
                    if (loadedData.Catalogs != null)
                    {
                        foreach (var catalog in loadedData.Catalogs)
                        {
                            Catalogs.Add(catalog); // Заполняем коллекцию загруженными данными
                        }
                    }
                    MessageBox.Show("Данные загружены!");
                }
            }
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
                int index = GetDropIndex(CatalogItemsControl, e.GetPosition(CatalogItemsControl));

                if (catalog != null && catalogs != null)
                {
                    catalogs.Remove(catalog);
                    catalogs.Insert(index, catalog);
                }
            }
        }

        private int GetDropIndex(ItemsControl target, Point dropPoint)
        {
            for (int i = 0; i < target.Items.Count; i++)
            {
                UIElement element = target.ItemContainerGenerator.ContainerFromIndex(i) as UIElement;
                if (element == null) continue;

                Rect rect = new Rect(element.TranslatePoint(new Point(0, 0), target), element.RenderSize);
                if (rect.Contains(dropPoint))
                {
                    return i;
                }
            }

            return target.Items.Count;
        }

    }
}
