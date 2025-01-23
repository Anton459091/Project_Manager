using System;
using System.Windows;
using System.Windows.Controls;

namespace Diplom1.UserControls.Controls
{
    public class MenuManager
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
        // Добавляем статичный метод RemoveElement для всех элементов
        public static void RemoveElement(FrameworkElement element)
        {
            if (element.Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(element);
            }
        }
    }
}