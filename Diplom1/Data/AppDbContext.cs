using Project_Manager.Models;
using System.Data.Entity;

public class AppDbContext : DbContext
{
    // Имя подключения из App.config
    public AppDbContext() : base("name=ProjectManager") { }

    // Добавляем DbSet для каждой таблицы
    public DbSet<User> Users { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<UserBoard> UsersBoards { get; set; }
    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Role> UserRole { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        // Настройка связей

    }

}

