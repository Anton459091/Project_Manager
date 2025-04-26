using System.Data.Entity;

public class AppDbContext : DbContext
{
    public AppDbContext() : base("name=MyDbConnection") { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Login)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .IsRequired();
    }
}