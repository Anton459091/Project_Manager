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

        public CardControl()
        {
            InitializeComponent();
            _menuManager.AttachMenu(MenuButton, this, ("Удалить", ContextMenuManager.RemoveElement));
        }

    }
}