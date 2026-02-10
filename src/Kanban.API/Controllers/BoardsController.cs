using System;
using Kanban.API.Data;
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
    public ActionResult<IEnumerable<Board>> GetAll()
    {
        var boards = _context.Boards.ToList();
        return Ok(boards);
    }

    [HttpGet("{id}")]
    public ActionResult<Board> GetById(int id)
    {
        var board = _context.Boards.Find(id);

        if (board == null)
        {
            return NotFound();
        }

        return Ok(board);
    }

    [HttpPost]
    public ActionResult<Board> Create(Board board)
    {
        if (string.IsNullOrWhiteSpace(board.Name))
        {
            return BadRequest("Name is required.");
        }
        _context.Boards.Add(board);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = board.Id }, board);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Board board)
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