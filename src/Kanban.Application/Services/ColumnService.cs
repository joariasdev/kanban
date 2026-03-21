using Kanban.Application.DTOs;
using Kanban.Application.Responses;
using Kanban.Domain.Entities;
using Kanban.Infrastructure.Repositories;

namespace Kanban.Application.Services;

public class ColumnService
{
    private readonly ColumnRepository _columnRepository;
    private readonly BoardRepository _boardRepository;
    public ColumnService(ColumnRepository columnRepository, BoardRepository boardRepository)
    {
        _columnRepository = columnRepository;
        _boardRepository = boardRepository;
    }


    public ApiResponse<List<ColumnDTO>> GetAll()
    {
        var columns = _columnRepository.GetAll().Select(c => new ColumnDTO { Id = c.Id, Name = c.Name, BoardId = c.BoardId }).ToList();
        return ApiResponse<List<ColumnDTO>>.SuccessResponse(columns);
    }


    public ApiResponse<ColumnDTO> GetById(int id)
    {
        var column = _columnRepository.GetById(id);

        if (column == null)
        {
            return ApiResponse<ColumnDTO>.FailureResponse($"Column with {id} not found", 404);
        }
        var columnDTO = new ColumnDTO { Id = column.Id, Name = column.Name, BoardId = column.BoardId };

        return ApiResponse<ColumnDTO>.SuccessResponse(columnDTO);
    }


    public ApiResponse<ColumnDTO> Create(CreateColumnDTO columnRequest)
    {
        if (string.IsNullOrWhiteSpace(columnRequest.Name))
        {
            return ApiResponse<ColumnDTO>.FailureResponse("Name is required");
        }

        var board = _boardRepository.GetById(columnRequest.BoardId);

        if (board == null)
        {
            return ApiResponse<ColumnDTO>.FailureResponse("Invalid Board");
        }

        var column = new Column
        {
            Name = columnRequest.Name,
            BoardId = columnRequest.BoardId
        };

        _columnRepository.Create(column);

        var columnDto = new ColumnDTO { Id = column.Id, Name = column.Name, BoardId = column.BoardId };

        return ApiResponse<ColumnDTO>.SuccessResponse(columnDto);
    }


    public ApiResponse<ColumnDTO> Update(int id, ColumnDTO column)
    {
        if (id != column.Id)
        {
            return ApiResponse<ColumnDTO>.FailureResponse("Incorrect Id");
        }

        if (string.IsNullOrWhiteSpace(column.Name))
        {
            return ApiResponse<ColumnDTO>.FailureResponse("Name is required");
        }

        var board = _boardRepository.GetById(column.BoardId);

        if (board == null)
        {
            return ApiResponse<ColumnDTO>.FailureResponse("Invalid Board");
        }

        var existingColumn = _columnRepository.GetById(id);

        if (existingColumn == null)
        {
            return ApiResponse<ColumnDTO>.FailureResponse($"Column with {id} not found", 404);
        }

        existingColumn.Name = column.Name;

        _columnRepository.Update(existingColumn);

        return ApiResponse<ColumnDTO>.SuccessResponse(null);
    }

    public ApiResponse<ColumnDTO> Delete(int id)
    {
        var column = _columnRepository.GetById(id);
        if (column == null)
        {
            return ApiResponse<ColumnDTO>.FailureResponse($"Column with {id} not found", 404);
        }
        _columnRepository.Delete(id);

        return ApiResponse<ColumnDTO>.SuccessResponse(null);
    }
}