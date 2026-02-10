using Kanban.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kanban.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Board> Boards { get; set; }
    public DbSet<Column> Columns { get; set; }
}