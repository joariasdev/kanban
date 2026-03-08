using Kanban.API.Models.DTOs;
using Kanban.Domain.Entities;
using Kanban.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColumnsController : ControllerBase
{
    private readonly ColumnRepository _columnRepository;
    private readonly BoardRepository _boardRepository;
    public ColumnsController(ColumnRepository columnRepository, BoardRepository boardRepository)
    {
        _columnRepository = columnRepository;
        _boardRepository = boardRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ColumnDTO>> GetAll()
    {
        var columns = _columnRepository.GetAll().Select(c => new ColumnDTO { Id = c.Id, Name = c.Name, BoardId = c.BoardId }).ToList();
        return Ok(columns);
    }

    [HttpGet("{id}")]
    public ActionResult<ColumnDTO> GetById(int id)
    {
        var column = _columnRepository.GetById(id);

        if (column == null)
        {
            return NotFound();
        }
        var columnDTO = new ColumnDTO { Id = column.Id, Name = column.Name, BoardId = column.BoardId };

        return Ok(columnDTO);
    }

    [HttpPost]
    public ActionResult<ColumnDTO> Create(CreateColumnDTO columnRequest)
    {
        if (string.IsNullOrWhiteSpace(columnRequest.Name))
        {
            return BadRequest("Name is required.");
        }

        var board = _boardRepository.GetById(columnRequest.BoardId);

        if (board == null)
        {
            return BadRequest("Invalid board");
        }

        var column = new Column
        {
            Name = columnRequest.Name,
            BoardId = columnRequest.BoardId
        };

        _columnRepository.Create(column);

        var columnDto = new ColumnDTO { Id = column.Id, Name = column.Name, BoardId = column.BoardId };

        return CreatedAtAction(nameof(GetById), new { id = column.Id }, columnDto);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, ColumnDTO column)
    {
        if (id != column.Id)
        {
            return BadRequest("Incorrect Id");
        }

        if (string.IsNullOrWhiteSpace(column.Name))
        {
            return BadRequest("Name is required");
        }

        var board = _boardRepository.GetById(column.BoardId);

        if (board == null)
        {
            return BadRequest("Invalid board");
        }

        var existingColumn = _columnRepository.GetById(id);

        if (existingColumn == null)
        {
            return NotFound();
        }

        existingColumn.Name = column.Name;

        _columnRepository.Update(existingColumn);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var column = _columnRepository.GetById(id);
        if (column == null)
        {
            return NotFound();
        }
        _columnRepository.Delete(id);

        return NoContent();
    }
}