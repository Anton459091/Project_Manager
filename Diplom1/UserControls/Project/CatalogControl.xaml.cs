using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Project_Manager.Data;
using System.Data.Entity;

namespace Project_Manager.UserControls
{
    public partial class CatalogControl : UserControl
    {
        private ProjectManager_Entities _context = new ProjectManager_Entities();

        public CatalogControl()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var catalogs = _context.Catalog.Include(c => c.Card).ToList();
            CatalogsListView.ItemsSource = catalogs;
        }

        // Обработчик события для кнопки добавления каталога
        private void AddCatalogButton_Click(object sender, RoutedEventArgs e)
        {
            string newCatalogName = NewCatalogTextBox.Text;
            if (!string.IsNullOrWhiteSpace(newCatalogName))
            {
                var newCatalog = new Catalog { Title = newCatalogName };
                _context.Catalog.Add(newCatalog);
                _context.SaveChanges();
                LoadData(); // Перезагружаем данные после добавления
            }
        }

        // Обработчик события для DragEnter
        private void CatalogBorder_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Catalog)))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        // Обработчик события для DragDrop
        private void CatalogBorder_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Catalog)))
            {
                Catalog droppedCatalog = (Catalog)e.Data.GetData(typeof(Catalog));
                // Реализуйте логику обработки перетаскивания
            }
        }

        // Обработчик события при потере фокуса в текстовом поле
        private void NewCatalogTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AddCatalogButton.Visibility = Visibility.Visible;
            var textBox = sender as TextBox;
            if (textBox != null && !string.IsNullOrWhiteSpace(textBox.Text))
            {
                AddCatalogButton_Click(sender, e);
            }
        }
    }
}
