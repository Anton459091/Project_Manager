using System.ComponentModel;
using System.Security;
using System;
using System.Collections.Generic;
    
namespace Project_Manager.Data
{
    public class User : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private string _login;
        private string _photoPath;
        private string _passwordHash; // Хэш пароля для безопасности

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        public User()
        {
            this.Board = new HashSet<Board>();
        }
    
        public int Users_ID { get; set; }
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string PhotoPath
        {
            get => _photoPath;
            set
            {
                _photoPath = value;
                OnPropertyChanged(nameof(PhotoPath));
            }
        }
        public string PasswordHash
        {
            get => _passwordHash;
            set
            {
                _passwordHash = value;
                OnPropertyChanged(nameof(PasswordHash));
            }
        }
        public int Role { get; set; }
    
        public virtual Role Role1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Board> Board { get; set; }

        //||\\

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Метод для проверки пароля
        public bool CheckPassword(string password)
        {
            // В реальном приложении используйте хэширование!
            // Это упрощенный пример - не используйте в production!
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
