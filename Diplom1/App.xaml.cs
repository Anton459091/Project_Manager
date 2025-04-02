using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Project_Manager.UserControls;
using Project_Manager.UserControls.Authorization;

namespace Project_Manager
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создаём MainWindow сразу, но не показываем его
            var mainWindow = new MainWindow();

            // Создаём и показываем окно логина
            var loginWindow = new LoginWindow();

            if (loginWindow.ShowDialog() == true)
            {
                // Если авторизация успешна, показываем MainWindow
                mainWindow.Show();
            }
            else
            {
                // Если авторизация не удалась, выводим сообщение и закрываем приложение
                MessageBox.Show("Не удалось войти. Приложение закрывается.");
                Shutdown();
            }
        }
    }

}