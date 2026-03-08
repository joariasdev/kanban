using Kanban.Domain.Entities;
using Kanban.Persistence;
using Task = Kanban.Domain.Entities.Task;

namespace Kanban.Infrastructure.Repositories;

public class TaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Task> GetAll()
    {
        var tasks = _context.Tasks.ToList();
        return tasks;
    }

    public Task? GetById(int id)
    {
        var task = _context.Tasks.Find(id);
        return task;
    }

    public void Create(Task task)
    {
        _context.Tasks.Add(task);
        _context.SaveChanges();
    }

    public void Update(Task task)
    {
        _context.Tasks.Update(task);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var task = _context.Tasks.Find(id);

        if (task != null)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }
    }
}
