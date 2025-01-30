using Project_Manager.Models;
using Project_Manager.UserControls.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Project_Manager.UserControls
{
    public partial class CardControl : UserControl
    {
        private ContextMenuManager _menuManager = new ContextMenuManager();

        public ObservableCollection<Card> Cards { get; set; } = new ObservableCollection<Card>();

        public CardControl()
        {
            InitializeComponent();
            MouseDown += CardControl_MouseDown;
            _menuManager.AttachMenu(MenuButton, this, ("Удалить", ContextMenuManager.RemoveElement));
        }

        private void CardControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
            }
        }

        internal int GetDropIndex(DragEventArgs e)
        {
            var target = e.OriginalSource as DependencyObject;

            while (target != null && !(target is CardControl || target is ItemsControl))
            {
                target = VisualTreeHelper.GetParent(target);
            }

            if (target is ItemsControl)
                return -1;

            if (target is CardControl targetCard && this != targetCard)
            {
                if (targetCard.Parent is ItemsControl cardItemsControl)
                {
                    return cardItemsControl.Items.IndexOf(targetCard.DataContext);
                }
            }

            return -1; // Возвращаем -1, если не нашли подходящий индекс
        }

        public void AddCard(string title, string description)
        {
            Cards.Add(new Card { Title = title, Description = description });
        }
    }
}