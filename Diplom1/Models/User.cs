using System.ComponentModel;
using System.Security;

public class User : INotifyPropertyChanged
{
    private string _login;
    private string _description;
    private string _photoPath;
    private string _passwordHash; // Хэш пароля для безопасности

    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            OnPropertyChanged(nameof(Login));
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged(nameof(Description));
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