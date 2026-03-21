using Kanban.Application.DTOs;
using Kanban.Application.Responses;
using Kanban.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskService _taskService;
    public TasksController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public ApiResponse<List<TaskDTO>> GetAll()
    {
        return _taskService.GetAll();
    }

    [HttpGet("{id}")]
    public ApiResponse<TaskDTO> GetById(int id)
    {
        return _taskService.GetById(id);
    }

    [HttpPost]
    public ApiResponse<TaskDTO> Create(CreateTaskDTO taskRequest)
    {
        return _taskService.Create(taskRequest);
    }

    [HttpPut("{id}")]
    public ApiResponse<TaskDTO> Update(int id, TaskDTO taskDTO)
    {
        return _taskService.Update(id, taskDTO);
    }

    [HttpDelete("{id}")]
    public ApiResponse<TaskDTO> Delete(int id)
    {
        return _taskService.Delete(id);
    }
}