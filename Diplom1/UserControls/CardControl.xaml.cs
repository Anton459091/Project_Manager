using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Diplom1.UserControls
{
    public partial class CardControl : UserControl
    {
        private bool _titleTextBoxFocused = false;
        private bool _descriptionTextBoxFocused = false;


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
                target = System.Windows.Media.VisualTreeHelper.GetParent(target);
            }
            if (target is StackPanel)
                return -1;
            int index = 0;
            if (target is CardControl targetCard)
            {
                if (this == targetCard)
                    return -1;
                if (targetCard.Parent is StackPanel cardStack)
                    index = cardStack.Children.IndexOf(targetCard);
            }
            return index;
        }
    }
}