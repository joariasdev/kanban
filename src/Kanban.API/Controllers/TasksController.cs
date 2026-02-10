using Kanban.API.Data;
using Kanban.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public TasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Models.Entities.Task>> GetAll()
    {
        var tasks = _context.Tasks.ToList();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public ActionResult<Models.Entities.Task> GetById(int id)
    {
        var task = _context.Tasks.Find(id);

        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpPost]
    public ActionResult<Models.Entities.Task> Create(Models.Entities.Task task)
    {
        if (string.IsNullOrWhiteSpace(task.Name))
        {
            return BadRequest("Name is required");
        }

        var column = _context.Columns.Find(task.ColumnId);

        if (column == null)
        {
            return BadRequest("Invalid column");
        }

        _context.Tasks.Add(task);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Models.Entities.Task task)
    {
        if (id != task.Id)
        {
            return BadRequest("Incorrect Id");
        }

        if (string.IsNullOrWhiteSpace(task.Name))
        {
            return BadRequest("Name is required");
        }

        var existingTask = _context.Tasks.Find(id);

        if (existingTask == null)
        {
            return NotFound();
        }

        existingTask.Name = task.Name;

        if (task.ColumnId != existingTask.ColumnId)
        {
            var column = _context.Columns.Find(task.ColumnId);

            if (column == null)
            {
                return BadRequest("Invalid column");
            }

            existingTask.ColumnId = task.ColumnId;
        }

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var task = _context.Tasks.Find(id);
        if (task == null)
        {
            return NotFound();
        }
        _context.Tasks.Remove(task);
        _context.SaveChanges();
        return NoContent();
    }
}