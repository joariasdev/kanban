using Kanban.Domain.Entities;
using Kanban.Persistence;

namespace Kanban.Infrastructure.Repositories;

public class BoardRepository
{
    private readonly ApplicationDbContext _context;

    public BoardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Board> GetAll()
    {
        var boards = _context.Boards.ToList();
        return boards;
    }

    public Board? GetById(int id)
    {
        var board = _context.Boards.Find(id);
        return board;
    }

    public void Create(Board board)
    {
        _context.Boards.Add(board);
        _context.SaveChanges();
    }

    public void Update(Board board)
    {
        _context.Boards.Update(board);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var board = _context.Boards.Find(id);

        if (board != null)
        {
            _context.Boards.Remove(board);
            _context.SaveChanges();
        }
    }
}
