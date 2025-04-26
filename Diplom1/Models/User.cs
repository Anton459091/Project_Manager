using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

public class User : INotifyPropertyChanged
{
    [Column("Users_ID")]
    public int User_Id { get; set; }

    [Column("Login")]
    public string User_login { get; set; }

    [Column("PhotoPath")]
    public string User_photoPath { get; set; }

    [Column("PasswordHash")]
    public string User_passwordHash; // Хэш пароля для безопасности

    [Column("Role")]
    public string User_role { get; set; }


    public string Login
    {
        get => User_login;
        set
        {
            User_login = value;
            OnPropertyChanged(nameof(Login));
        }
    }

    public string PhotoPath
    {
        get => User_photoPath;
        set
        {
            User_photoPath = value;
            OnPropertyChanged(nameof(PhotoPath));
        }
    }

    public string PasswordHash
    {
        get => User_passwordHash;
        set
        {
            User_passwordHash = value;
            OnPropertyChanged(nameof(PasswordHash));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

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