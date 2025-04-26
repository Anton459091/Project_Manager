using System;
using System.Data.Entity;
using System.Linq;
using Project_Manager.Data;

public static class UserRepository
{
    private static readonly ProjectManagerEntities _dbContext = new ProjectManagerEntities();

    public static User LoadUser(string login)
    {
        return _dbContext.User.FirstOrDefault(u => u.Login == login);
    }

    public static void SaveUser(User user)
    {
        using (var db = new ProjectManagerEntities())
        {

            int? maxId = db.User.Max(u => (int?)u.Users_ID);

            if (maxId == null)
            {
                user.Users_ID = 1;  
            }
            else
            {
                user.Users_ID = maxId.Value + 1;  
            }


            var role = db.Role.Find(1);
            if (role == null)
            {
  
                role = new Role { Role_ID = 1, RoleName = "Admin" };
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
        using (var db = new ProjectManagerEntities())
        {
            return db.User.Any(u => u.Login == login);
        }
    }

    public static User GetUser(string login)
    {
        using (var db = new ProjectManagerEntities())
        {
            return db.User.FirstOrDefault(u => u.Login == login);
        }
    }

}