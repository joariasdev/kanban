using System;

namespace Kanban.API.Models.DTOs;

public class TaskDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int ColumnId { get; set; }
}
