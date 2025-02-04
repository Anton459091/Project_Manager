using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections;

namespace Project_Manager.UserControls.Controls
{
    public class ContextMenuManager
    {
        private ContextMenu _contextMenu;
        private Button _menuButton;

        public void AttachMenu(Button menuButton, FrameworkElement element, params (string header, Action<FrameworkElement> action)[] items)
        {
            _menuButton = menuButton;
            _menuButton.Click += (sender, e) => MenuButton_Click(element);

            _contextMenu = CreateContextMenu(element, items);
        }

        private ContextMenu CreateContextMenu(FrameworkElement element, params (string header, Action<FrameworkElement> action)[] items)
        {
            var contextMenu = new ContextMenu();

            foreach (var (header, action) in items)
            {
                var menuItem = new MenuItem { Header = header, DataContext = element };
                menuItem.Click += (sender, e) =>
                {
                    if (e.OriginalSource is MenuItem item && item.DataContext is FrameworkElement dataContext)
                    {
                        action(dataContext);
                    }
                };
                contextMenu.Items.Add(menuItem);
            }
            return contextMenu;
        }

        private void MenuButton_Click(FrameworkElement element)
        {
            if (_contextMenu != null)
            {
                _contextMenu.PlacementTarget = (UIElement)element;
                _contextMenu.IsOpen = true;
            }
        }

        public static void RemoveElement(FrameworkElement element)
        {
            ItemsControl parentItemsControl = FindParentItemsControl(element);

            if (parentItemsControl != null && parentItemsControl.ItemsSource is System.Collections.IList itemsSource)
            {
                // Получаем CatalogControl из DataContext
                if (element is СatalogControl catalogControl)
                {
                    // Получаем объект Catalog из DataContext CatalogControl
                    object catalog = catalogControl.DataContext;

                    // Удаляем объект Catalog из коллекции
                    itemsSource.Remove(catalog);
                }
                else // Обработка удаления CardControl остается без изменений
                {
                    itemsSource.Remove(element is CardControl cardControl ? cardControl.DataContext : element);
                }
            }
        }

        private static ItemsControl FindParentItemsControl(FrameworkElement element)
        {
            DependencyObject parent = element;

            while (parent != null)
            {
                if (parent is ItemsControl itemsControl && FindParentBoardControl(itemsControl) != null) // Ищем BoardControl вместо CatalogControl
                {
                    return itemsControl;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        private static BoardControl FindParentBoardControl(DependencyObject element) // Ищем BoardControl вместо CatalogControl
        {
            while (element != null)
            {
                if (element is BoardControl boardControl) // Ищем BoardControl вместо CatalogControl
                {
                    return boardControl;
                }
                element = VisualTreeHelper.GetParent(element);
            }
            return null;
        }
        public static void MakeEditable(FrameworkElement element)
        {
            if (element is СatalogControl catalogControl)
            {
                catalogControl.CatalogNameTextBox.IsReadOnly = false;
                catalogControl.CatalogNameTextBox.Focus();
                catalogControl.CatalogNameTextBox.SelectAll(); // Выделяем весь текст для удобства редактирования
            }
        }

    }
}