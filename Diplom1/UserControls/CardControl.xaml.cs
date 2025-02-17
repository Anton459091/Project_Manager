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
    public partial class CardControl : UserControl
    {
        private Point startPoint;
        private bool isDragging = false;
        private ContextMenuManager _menuManager = new ContextMenuManager();

        public CardControl()
        {
            InitializeComponent();
            _menuManager.AttachMenu(MenuButton, this, ("Удалить", ContextMenuManager.RemoveElement));
        }

        private void CardBorder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(CardBorder);
            isDragging = true;
        }
        private void CardBorder_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && isDragging)
            {
                Point mousePos = e.GetPosition(CardBorder);
                Vector diff = startPoint - mousePos;

                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    isDragging = false;


                    Card card = (Card)DataContext;

 
                    DataObject dragData = new DataObject(typeof(Card), card);


                    DragDrop.DoDragDrop(CardBorder, dragData, DragDropEffects.Move);
                }
            }
        }
    }
}