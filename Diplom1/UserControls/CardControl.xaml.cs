using Project_Manager.UserControls.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Project_Manager.UserControls
{
    public partial class CardControl : UserControl
    {
        private ContextMenuManager _menuManager = new ContextMenuManager();

        public string Title
        {
            get { return TitleTextBox.Text; }
            set { TitleTextBox.Text = value; }
        }

        public string Description
        {
            get { return DescriptionTextBox.Text; }
            set { DescriptionTextBox.Text = value; }
        }
        public CardControl()
        {
            InitializeComponent();
            MouseDown += CardControl_MouseDown;
            _menuManager.AttachMenu(MenuButton, this,
            ("Удалить", ContextMenuManager.RemoveElement)
            );
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

            while (target != null && !(target is CardControl || target is StackPanel))
            {
                target = VisualTreeHelper.GetParent(target);
            }
            if (target is StackPanel)
                return -1;
            if (target is CardControl targetCard && this != targetCard)

            {
                if (targetCard.Parent is StackPanel cardStack)

                {
                    return cardStack.Children.IndexOf(targetCard);
                }
            }
            return -1; // Возвращаем -1, если не нашли подходящий индекс
        }
    }
}