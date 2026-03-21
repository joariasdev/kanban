using Kanban.Application.DTOs;
using Kanban.Application.Responses;
using Kanban.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly BoardService _boardService;
    public BoardsController(BoardService boardService)
    {
        _boardService = boardService;
    }

    [HttpGet]
    public ApiResponse<List<BoardDTO>> GetAll()
    {
        return _boardService.GetAll();
        
    }

    [HttpGet("{id}")]
    public ApiResponse<BoardDTO> GetById(int id)
    {
        return _boardService.GetById(id);
    }

    [HttpPost]
    public ApiResponse<BoardDTO> Create(CreateBoardDTO boardRequest)
    {
        return _boardService.Create(boardRequest);
    }

    [HttpPut("{id}")]
    public ApiResponse<BoardDTO> Update(int id, BoardDTO board)
    {
       return _boardService.Update(id, board);
    }

    [HttpDelete("{id}")]
    public ApiResponse<BoardDTO>  Delete(int id)
    {
       return _boardService.Delete(id);
    }
}