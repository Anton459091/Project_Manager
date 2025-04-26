using System;
using System.IO;
using Newtonsoft.Json;

public static class UserRepository
{
    private static readonly string UserFilePath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Data",
        "user.json");

    public static User LoadUser()
    {
        if (!File.Exists(UserFilePath))
        {
            return new User
            {
                Login = "Гость",
                
                PhotoPath = null
            };
        }

        try
        {
            var json = File.ReadAllText(UserFilePath);
            return JsonConvert.DeserializeObject<User>(json);
        }
        catch
        {
            return new User
            {
                Login = "Гость",
                
                PhotoPath = null
            };
        }
    }

    public static void SaveUser(User user)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(UserFilePath));
        var json = JsonConvert.SerializeObject(user, Formatting.Indented);
        File.WriteAllText(UserFilePath, json);
    }

    public static bool UserExists(string login)
    {
        var user = LoadUser();
        return user != null && user.Login == login;
    }
}