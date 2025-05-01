using System;
using System.Data.Entity;
using System.Linq;
using Project_Manager.Data;

public static class ProjectRepository
{
    private static readonly ProjectManager_Entities _dbContext = new ProjectManager_Entities();

    public static User LoadUser(string login)
    {
        return _dbContext.User.FirstOrDefault(u => u.Login == login);
    }

    public static void SaveUser(User user)
    {
        using (var db = new ProjectManager_Entities())
        {

            int? maxId = db.User.Max(u => (int?)u.User_ID);

            if (maxId == null)
            {
                user.User_ID = 1;  
            }
            else
            {
                user.User_ID = maxId.Value + 1;  
            }


            var role = db.Role.Find(1);
            if (role == null)
            {
  
                role = new Role { Role_ID = 1, RoleName = "Пользователь" };
                db.Role.Add(role);
                db.SaveChanges();  
            }


            user.Role = role.Role_ID;  


            db.User.Add(user);
            db.SaveChanges();
        }
    }




    public static bool UserExists(string login)
    {
        using (var db = new ProjectManager_Entities())
        {
            return db.User.Any(u => u.Login == login);
        }
    }

    public static User GetUser(string login)
    {
        using (var db = new ProjectManager_Entities())
        {
            return db.User.FirstOrDefault(u => u.Login == login);
        }
    }

}