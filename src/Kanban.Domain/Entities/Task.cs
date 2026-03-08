using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanban.Domain.Entities;

public class Task
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }

    public int ColumnId { get; set; }

    public Column? Column { get; set; }
}