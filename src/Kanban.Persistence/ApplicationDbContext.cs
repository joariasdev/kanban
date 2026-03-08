using Kanban.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Task = Kanban.Domain.Entities.Task;

namespace Kanban.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Board> Boards { get; set; }
    public DbSet<Column> Columns { get; set; }
    public DbSet<Task> Tasks { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<Board>().Property(b => b.CreatedAt).ValueGeneratedOnAdd();
                modelBuilder.Entity<Column>().Property(b => b.CreatedAt).ValueGeneratedOnAdd();
                modelBuilder.Entity<Task>().Property(b => b.CreatedAt).ValueGeneratedOnAdd();
        }
}