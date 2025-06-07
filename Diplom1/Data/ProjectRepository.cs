using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Project_Manager.Data;
using Project_Manager.UserControls.Profile;

public static class ProjectRepository
{
    private static readonly ProjectManager_Entities _dbContext = new ProjectManager_Entities();

    public static User LoadUserLogin(string login)
    {
        return _dbContext.User.FirstOrDefault(u => u.Login == login);
    }

    public static User LoadUserRole(int role)
    {
        return _dbContext.User.FirstOrDefault(u => u.Role == role);
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
    public static List<Role> GetAllRoles()
    {
        using (var db = new ProjectManager_Entities())
        {
            return db.Role.ToList();
        }
    }


    public static List<UserViewModel> GetAllUsersWithRoles()
    {
        using (var db = new ProjectManager_Entities())
        {
            var query = from user in db.User
                        join role in db.Role on user.Role equals role.Role_ID
                        select new UserViewModel
                        {
                            UserId = user.User_ID,
                            Login = user.Login,
                            RoleId = role.Role_ID,
                            RoleName = role.RoleName
                        };

            return query.ToList();
        }
    }

    public static void UpdateUserRole(int userId, int newRoleId)
    {
        using (var db = new ProjectManager_Entities())
        {
            var user = db.User.FirstOrDefault(u => u.User_ID == userId);
            if (user != null)
            {
                user.Role = newRoleId;
                db.SaveChanges();
            }
        }
    }

}