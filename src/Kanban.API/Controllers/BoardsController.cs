using Kanban.API.Models.DTOs;
using Kanban.Domain.Entities;
using Kanban.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly BoardRepository _boardRepository;
    public BoardsController(BoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<BoardDTO>> GetAll()
    {
        var boardDTOs = _boardRepository.GetAll().Select(b => new BoardDTO { Id = b.Id, Name = b.Name }).ToList();
        return Ok(boardDTOs);
    }

    [HttpGet("{id}")]
    public ActionResult<BoardDTO> GetById(int id)
    {
        var board = _boardRepository.GetById(id);

        if (board == null)
        {
            return NotFound();
        }

        var boardDTO = new BoardDTO { Id = board.Id, Name = board.Name };

        return Ok(boardDTO);
    }

    [HttpPost]
    public ActionResult<BoardDTO> Create(CreateBoardDTO boardRequest)
    {
        if (string.IsNullOrWhiteSpace(boardRequest.Name))
        {
            return BadRequest("Name is required.");
        }

        var board = new Board
        {
            Name = boardRequest.Name
        };

        _boardRepository.Create(board);

        var boardDTO = new BoardDTO
        {
            Id = board.Id,
            Name = board.Name
        };

        return CreatedAtAction(nameof(GetById), new { id = board.Id }, boardDTO);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, BoardDTO board)
    {
        if (id != board.Id)
        {
            return BadRequest("Incorrect Id");
        }

        if (string.IsNullOrWhiteSpace(board.Name))
        {
            return BadRequest("Name is required");
        }

        var existingBoard = _boardRepository.GetById(id);

        if (existingBoard == null)
        {
            return NotFound();
        }

        existingBoard.Name = board.Name;

        _boardRepository.Update(existingBoard);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var board = _boardRepository.GetById(id);
        if (board == null)
        {
            return NotFound();
        }
        _boardRepository.Delete(id);
        return NoContent();
    }
}