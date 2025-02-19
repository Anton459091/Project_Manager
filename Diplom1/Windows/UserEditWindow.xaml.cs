using System.ComponentModel;
using System.Windows.Input;

public class UserEditViewModel : INotifyPropertyChanged
{
    public string WindowTitle => IsEditMode ? "Редактирование профиля" : "Регистрация";
    public bool IsEditMode { get; set; }

    private string _username;
    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    private string _email;
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    private string _password;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    private string _confirmPassword;
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            OnPropertyChanged(nameof(ConfirmPassword));
        }
    }

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
            OnPropertyChanged(nameof(HasErrors)); // Уведомляем об изменении состояния ошибок
        }
    }

    public bool HasErrors => !string.IsNullOrEmpty(ErrorMessage);

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand ChangeAvatarCommand { get; }

    public UserEditViewModel()
    {
        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
        ChangeAvatarCommand = new RelayCommand(ChangeAvatar);
    }

    private void Save()
    {
        // Логика сохранения
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email))
        {
            ErrorMessage = "Логин и Email не могут быть пустыми.";
            return;
        }

        if (IsEditMode && string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Пароль не может быть пустым при редактировании.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Пароли не совпадают.";
            return;
        }

        // Здесь можно добавить логику для сохранения данных пользователя
        ErrorMessage = string.Empty; // Сброс ошибки при успешном сохранении
    }

    private void Cancel()
    {
        // Логика отмены
        // Например, можно сбросить поля или закрыть окно
        Username = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;
        ErrorMessage = string.Empty;
    }

    private void ChangeAvatar()
    {
        // Логика смены аватарки
        // Например, открыть диалог выбора файла
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}