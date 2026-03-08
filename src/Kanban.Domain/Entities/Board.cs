using System.ComponentModel.DataAnnotations.Schema;

namespace Kanban.Domain.Entities;

public class Board
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public List<Column> Columns { get; set; } = new List<Column>();
}