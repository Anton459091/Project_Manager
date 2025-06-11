using Project_Manager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project_Manager.UserControls.Profile
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public partial class AdminControl : UserControl
    {
        private List<Role> _roles;

        public AdminControl()
        {
            InitializeComponent();

            LoadUsersAndRoles();
        }

        private void LoadUsersAndRoles()
        {
            _roles = ProjectRepository.GetAllRoles();
            var users = ProjectRepository.GetAllUsersWithRoles();

            // Устанавливаем источник данных для ComboBoxColumn
            var comboColumn = (DataGridComboBoxColumn)UsersDataGrid.Columns[1];
            comboColumn.ItemsSource = _roles;

            UsersDataGrid.ItemsSource = users;
        }
        private void UsersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column is DataGridComboBoxColumn && e.Row.Item is UserViewModel userVm)
            {
                var combo = e.EditingElement as ComboBox;
                if (combo != null)
                {
                    int newRoleId = (int)combo.SelectedValue;
                    ProjectRepository.UpdateUserRole(userVm.UserId, newRoleId);
                    MessageBox.Show($"Роль пользователя '{userVm.Login}' изменена.");
                }
            }
        }
    }
}
