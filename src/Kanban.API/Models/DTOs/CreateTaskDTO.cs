
namespace Kanban.API.Models.DTOs;

public class CreateTaskDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int ColumnId { get; set; }
}
