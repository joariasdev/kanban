
using Kanban.Application.DTOs;
using Kanban.Application.Responses;
using Kanban.Domain.Entities;
using Kanban.Infrastructure.Repositories;

namespace Kanban.Application.Services;

public class BoardService
{
    private readonly BoardRepository _boardRepository;
    public BoardService(BoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }
    public ApiResponse<List<BoardDTO>> GetAll()
    {
        var boardDTOs = _boardRepository.GetAll().Select(b => new BoardDTO { Id = b.Id, Name = b.Name }).ToList();
        return ApiResponse<List<BoardDTO>>.SuccessResponse(boardDTOs);
    }

   
    public ApiResponse<BoardDTO> GetById(int id)
    {
        var board = _boardRepository.GetById(id);

        if (board == null)
        {
            return ApiResponse<BoardDTO>.FailureResponse($"Board with {id} not found", 404);
        }

        var boardDTO = new BoardDTO { Id = board.Id, Name = board.Name };

        return ApiResponse<BoardDTO>.SuccessResponse(boardDTO);
    }

   
    public ApiResponse<BoardDTO> Create(CreateBoardDTO boardRequest)
    {
        if (string.IsNullOrWhiteSpace(boardRequest.Name))
        {
            return ApiResponse<BoardDTO>.FailureResponse("Name is required");
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

        return ApiResponse<BoardDTO>.SuccessResponse(boardDTO);
    }

    public ApiResponse<BoardDTO> Update(int id, BoardDTO board)
    {
        if (id != board.Id)
        {
            return ApiResponse<BoardDTO>.FailureResponse("Incorrect Id");
        }

        if (string.IsNullOrWhiteSpace(board.Name))
        {
           return ApiResponse<BoardDTO>.FailureResponse("Name is required");
        }

        var existingBoard = _boardRepository.GetById(id);

        if (existingBoard == null)
        {
            return ApiResponse<BoardDTO>.FailureResponse($"Board with {id} not found", 404);
        }

        existingBoard.Name = board.Name;

        _boardRepository.Update(existingBoard);
        
        return ApiResponse<BoardDTO>.SuccessResponse(null);
    }
   
    public ApiResponse<BoardDTO> Delete(int id)
    {
        var board = _boardRepository.GetById(id);
        if (board == null)
        {
            return ApiResponse<BoardDTO>.FailureResponse($"Board with {id} not found", 404);
        }
        _boardRepository.Delete(id);
        return ApiResponse<BoardDTO>.SuccessResponse(null);
    }
}