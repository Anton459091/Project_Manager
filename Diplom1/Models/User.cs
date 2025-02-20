using System.ComponentModel;

public class User : INotifyPropertyChanged
{
    private string _login;
    private string _description;
    private string _photoPath;

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

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}