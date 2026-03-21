namespace Kanban.Application.DTOs;

public class ColumnDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int BoardId { get; set; }
}
