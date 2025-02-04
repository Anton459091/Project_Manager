using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Project_Manager.Controls
{
    public class DragDropHelper
    {
        private object _draggedItem;

        public void StartDrag(FrameworkElement element)
        {
            _draggedItem = element.DataContext;
            DragDrop.DoDragDrop(element, _draggedItem, DragDropEffects.Move);
        }

        public void OnDragOver(DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(_draggedItem.GetType()) ? DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }

        public void OnDrop(DragEventArgs e, ItemsControl targetItemsControl)
        {
            if (e.Data.GetDataPresent(_draggedItem.GetType()))
            {
                var targetItem = e.OriginalSource as FrameworkElement;
                var sourceItem = e.Data.GetData(_draggedItem.GetType());

                // Логика перемещения элемента
                if (sourceItem != null && targetItem != null)
                {
                    // Здесь вы можете добавить логику для перемещения элемента в коллекции
                    // Например, если это карточка, переместите её в другую колонку
                }
            }
        }
    }
}
