using System.ComponentModel.DataAnnotations.Schema;

namespace Kanban.API.Models.Entities;

public class Column
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime? CreatedAt { get; set; }

    public int BoardId { get; set; }

    public Board? Board { get; set; }
}

