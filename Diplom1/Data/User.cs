using System.ComponentModel;
using System.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_Manager.Data
{


    public partial class User
    {

        public User()
        {
            this.Board = new HashSet<Board>();
        }

        public int User_ID { get; set; }
        public int Role { get; set; }
        public string Login { get; set; }
        public string PhotoPath { get; set; }
        public string PasswordHash { get; set; }

        public virtual Role Role1 { get; set; }

        public virtual ICollection<Board> Board { get; set; }

        // Метод для проверки пароля
        public bool CheckPassword(string password)
        {
            // Это упрощенный пример
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }

        // Метод для установки пароля с хэшированием
        public void SetPassword(string password)
        {
            // Генерируем соль и хэшируем пароль
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}

