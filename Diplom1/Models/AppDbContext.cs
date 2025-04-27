using System.Data.Entity;
using Project_Manager.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Manager.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=ProjectManagerConnection")
        {
            // Настройки инициализации
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<UserBoard> UserBoards { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserBoard>()
                .HasKey(ub => new { ub.Users_ID, ub.Boards_ID });


            modelBuilder.Entity<User>()
                .HasMany(u => u.UserBoards)
                .WithRequired(ub => ub.User)
                .HasForeignKey(ub => ub.Users_ID);


            modelBuilder.Entity<Board>()
                .HasMany(b => b.UserBoards)
                .WithRequired(ub => ub.Board)
                .HasForeignKey(ub => ub.Boards_ID);

        }
    }
}