using Kanban.Application.DTOs;
using Kanban.Application.Responses;
using Kanban.Infrastructure.Repositories;
using Task = Kanban.Domain.Entities.Task;

namespace Kanban.Application.Services;

public class TaskService
{
    private readonly TaskRepository _taskRepository;
    private readonly ColumnRepository _columnRepository;
    public TaskService(TaskRepository taskRepository, ColumnRepository columnRepository)
    {
        _taskRepository = taskRepository;
        _columnRepository = columnRepository;
    }

    public ApiResponse<List<TaskDTO>> GetAll()
    {
        var taskDTOs = _taskRepository.GetAll().Select(t => new TaskDTO { Id = t.Id, Name = t.Name, Description = t.Description, ColumnId = t.ColumnId }).ToList();
        return ApiResponse<List<TaskDTO>>.SuccessResponse(taskDTOs);
    }


    public ApiResponse<TaskDTO> GetById(int id)
    {
        var task = _taskRepository.GetById(id);

        if (task == null)
        {
            return ApiResponse<TaskDTO>.FailureResponse($"Task with {id} not found", 404);
        }

        var taskDTO = new TaskDTO { Id = task.Id, Name = task.Name, Description = task.Description, ColumnId = task.ColumnId };

        return ApiResponse<TaskDTO>.SuccessResponse(taskDTO);
    }


    public ApiResponse<TaskDTO> Create(CreateTaskDTO taskRequest)
    {
        if (string.IsNullOrWhiteSpace(taskRequest.Name))
        {
            return ApiResponse<TaskDTO>.FailureResponse("Name is required");
        }

        var column = _columnRepository.GetById(taskRequest.ColumnId);

        if (column == null)
        {
            return ApiResponse<TaskDTO>.FailureResponse("Invalid Board");
        }

        var task = new Task
        {
            Name = taskRequest.Name,
            Description = taskRequest.Description,
            ColumnId = taskRequest.ColumnId
        };

        _taskRepository.Create(task);

        var taskDto = new TaskDTO { Id = task.Id, Name = task.Name, Description = task.Description, ColumnId = task.ColumnId };

        return ApiResponse<TaskDTO>.SuccessResponse(taskDto);
    }


    public ApiResponse<TaskDTO> Update(int id, TaskDTO task)
    {
        if (id != task.Id)
        {
            return ApiResponse<TaskDTO>.FailureResponse("Incorrect Id");
        }

        if (string.IsNullOrWhiteSpace(task.Name))
        {
            return ApiResponse<TaskDTO>.FailureResponse("Name is required");
        }

        var existingTask = _taskRepository.GetById(id);

        if (existingTask == null)
        {
            return ApiResponse<TaskDTO>.FailureResponse($"Task with {id} not found", 404);
        }

        existingTask.Name = task.Name;
        existingTask.Description = task.Description;

        if (task.ColumnId != existingTask.ColumnId)
        {
            var column = _columnRepository.GetById(task.ColumnId);

            if (column == null)
            {
                return ApiResponse<TaskDTO>.FailureResponse("Invalid Column");
            }

            existingTask.ColumnId = task.ColumnId;
        }

        _taskRepository.Update(existingTask);

        return ApiResponse<TaskDTO>.SuccessResponse(null);
    }

    public ApiResponse<TaskDTO> Delete(int id)
    {
        var task = _taskRepository.GetById(id);
        if (task == null)
        {
            return ApiResponse<TaskDTO>.FailureResponse($"Task with {id} not found", 404);
        }
        _taskRepository.Delete(id);

        return ApiResponse<TaskDTO>.SuccessResponse(null);
    }
}