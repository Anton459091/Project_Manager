using Project_Manager.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Project_Manager.UserControls
{
    public partial class CardControl : UserControl
    {
        private Point startPoint;
        private bool isDragging = false;
        private ProjectManager_Entities _context = new ProjectManager_Entities();

        public CardControl()
        {
            InitializeComponent();
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
