using System.ComponentModel.DataAnnotations.Schema;

namespace Kanban.Domain.Entities;

public class Column
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public int BoardId { get; set; }
    public Board? Board { get; set; }
    public List<Task> Tasks { get; set; } = new List<Task>();
}

