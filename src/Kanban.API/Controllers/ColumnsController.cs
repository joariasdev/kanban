using Kanban.API.Data;
using Kanban.API.Models.DTOs;
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
    public ActionResult<IEnumerable<ColumnDTO>> GetAll()
    {
        var columns = _context.Columns.Select(c => new Column { Id = c.Id, Name = c.Name, BoardId = c.BoardId }).ToList();
        return Ok(columns);
    }

    [HttpGet("{id}")]
    public ActionResult<ColumnDTO> GetById(int id)
    {
        var column = _context.Columns.Find(id);

        if (column == null)
        {
            return NotFound();
        }
        var columnDTO = new Column { Id = column.Id, Name = column.Name, BoardId = column.BoardId };

        return Ok(columnDTO);
    }

    [HttpPost]
    public ActionResult<ColumnDTO> Create(CreateColumnDTO columnRequest)
    {
        if (string.IsNullOrWhiteSpace(columnRequest.Name))
        {
            return BadRequest("Name is required.");
        }

        var board = _context.Boards.Find(columnRequest.BoardId);

        if (board == null)
        {
            return BadRequest("Invalid board");
        }

        var column = new Column
        {
            Name = columnRequest.Name,
            BoardId = columnRequest.BoardId
        };

        _context.Columns.Add(column);
        _context.SaveChanges();

        var columnDto = new ColumnDTO { Id = column.Id, Name = column.Name, BoardId = column.BoardId };

        return CreatedAtAction(nameof(GetById), new { id = column.Id }, columnDto);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, ColumnDTO column)
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