using Kanban.API.Data;
using Kanban.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColumnsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public ColumnsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Column>> GetAll()
    {
        var columns = _context.Columns.ToList();
        return Ok(columns);
    }

    [HttpGet("{id}")]
    public ActionResult<Column> GetById(int id)
    {
        var column = _context.Columns.Find(id);

        if (column == null)
        {
            return NotFound();
        }

        return Ok(column);
    }

    [HttpPost]
    public ActionResult<Column> Create(Column column)
    {
        if (string.IsNullOrWhiteSpace(column.Name))
        {
            return BadRequest("Name is required.");
        }

        var board = _context.Boards.Find(column.BoardId);

        if (board == null)
        {
            return BadRequest("Invalid board");
        }

        _context.Columns.Add(column);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = column.Id }, column);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Column column)
    {
        if (id != column.Id)
        {
            return BadRequest("Incorrect Id");
        }

        if (string.IsNullOrWhiteSpace(column.Name))
        {
            return BadRequest("Name is required");
        }

        var board = _context.Boards.Find(column.BoardId);

        if (board == null)
        {
            return BadRequest("Invalid board");
        }

        var existingColumn = _context.Columns.Find(id);

        if (existingColumn == null)
        {
            return NotFound();
        }

        existingColumn.Name = column.Name;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var column = _context.Columns.Find(id);
        if (column == null)
        {
            return NotFound();
        }
        _context.Columns.Remove(column);
        _context.SaveChanges();
        return NoContent();
    }
}