using System;
using Kanban.API.Data;
using Kanban.API.Models.DTOs;
using Kanban.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public BoardsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<BoardDTO>> GetAll()
    {
        var boardDTOs = _context.Boards.Select(b => new BoardDTO { Id = b.Id, Name = b.Name }).ToList();
        return Ok(boardDTOs);
    }

    [HttpGet("{id}")]
    public ActionResult<BoardDTO> GetById(int id)
    {
        var board = _context.Boards.Find(id);

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

        _context.Boards.Add(board);
        _context.SaveChanges();

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

        var existingBoard = _context.Boards.Find(id);

        if (existingBoard == null)
        {
            return NotFound();
        }

        existingBoard.Name = board.Name;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var board = _context.Boards.Find(id);
        if (board == null)
        {
            return NotFound();
        }
        _context.Boards.Remove(board);
        _context.SaveChanges();
        return NoContent();
    }
}