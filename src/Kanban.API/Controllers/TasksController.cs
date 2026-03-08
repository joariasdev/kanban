using Kanban.API.Models.DTOs;
using Kanban.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Task = Kanban.Domain.Entities.Task;

namespace Kanban.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskRepository _taskRepository;
    private readonly ColumnRepository _columnRepository;
    public TasksController(TaskRepository taskRepository, ColumnRepository columnRepository)
    {
        _taskRepository = taskRepository;
        _columnRepository = columnRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TaskDTO>> GetAll()
    {
        var taskDTOs = _taskRepository.GetAll().Select(t => new TaskDTO { Id = t.Id, Name = t.Name, Description = t.Description, ColumnId = t.ColumnId }).ToList();
        return Ok(taskDTOs);
    }

    [HttpGet("{id}")]
    public ActionResult<TaskDTO> GetById(int id)
    {
        var task = _taskRepository.GetById(id);

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

        var column = _columnRepository.GetById(taskRequest.ColumnId);

        if (column == null)
        {
            return BadRequest("Invalid column");
        }

        var task = new Task
        {
            Name = taskRequest.Name,
            Description = taskRequest.Description,
            ColumnId = taskRequest.ColumnId
        };

        _taskRepository.Create(task);

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

        var existingTask = _taskRepository.GetById(id);

        if (existingTask == null)
        {
            return NotFound();
        }

        existingTask.Name = task.Name;
        existingTask.Description = task.Description;

        if (task.ColumnId != existingTask.ColumnId)
        {
            var column = _columnRepository.GetById(task.ColumnId);

            if (column == null)
            {
                return BadRequest("Invalid column");
            }

            existingTask.ColumnId = task.ColumnId;
        }

        _taskRepository.Update(existingTask);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var task = _taskRepository.GetById(id);
        if (task == null)
        {
            return NotFound();
        }
        _taskRepository.Delete(id);

        return NoContent();
    }
}