using Kanban.Domain.Entities;
using Kanban.Persistence;

namespace Kanban.Infrastructure.Repositories;

public class ColumnRepository
{
    private readonly ApplicationDbContext _context;

    public ColumnRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Column> GetAll()
    {
        var columns = _context.Columns.ToList();
        return columns;
    }

    public Column? GetById(int id)
    {
        var column = _context.Columns.Find(id);
        return column;
    }

    public void Create(Column column)
    {
        _context.Columns.Add(column);
        _context.SaveChanges();
    }

    public void Update(Column column)
    {
        _context.Columns.Update(column);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var column = _context.Columns.Find(id);

        if (column != null)
        {
            _context.Columns.Remove(column);
            _context.SaveChanges();
        }
    }
}
