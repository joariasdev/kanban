using Kanban.API.Data;
using Kanban.API.Models.DTOs;
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
    public ActionResult<IEnumerable<TaskDTO>> GetAll()
    {
        var taskDTOs = _context.Tasks.Select(t => new TaskDTO { Id = t.Id, Name = t.Name, Description = t.Description, ColumnId = t.ColumnId }).ToList();
        return Ok(taskDTOs);
    }

    [HttpGet("{id}")]
    public ActionResult<TaskDTO> GetById(int id)
    {
        var task = _context.Tasks.Find(id);

        if (task == null)
        {
            return NotFound();
        }

        var taskDTO = new TaskDTO { Id = task.Id, Name = task.Name, Description = task.Description, ColumnId = task.ColumnId };

        return Ok(taskDTO);
    }

    [HttpPost]
    public ActionResult<TaskDTO> Create(CreateTaskDTO taskRequest)
    {
        if (string.IsNullOrWhiteSpace(taskRequest.Name))
        {
            return BadRequest("Name is required");
        }

        var column = _context.Columns.Find(taskRequest.ColumnId);

        if (column == null)
        {
            return BadRequest("Invalid column");
        }

        var task = new Models.Entities.Task
        {
            Name = taskRequest.Name,
            Description = taskRequest.Description,
            ColumnId = taskRequest.ColumnId
        };

        _context.Tasks.Add(task);
        _context.SaveChanges();

        var taskDto = new TaskDTO { Id = task.Id, Name = task.Name, Description = task.Description, ColumnId = task.ColumnId };

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, taskDto);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, TaskDTO task)
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
        existingTask.Description = task.Description;

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