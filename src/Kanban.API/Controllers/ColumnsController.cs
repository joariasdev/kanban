using Kanban.Application.DTOs;
using Kanban.Application.Responses;
using Kanban.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColumnsController : ControllerBase
{
    private readonly ColumnService _columnService;
    public ColumnsController(ColumnService columnService)
    {
        _columnService = columnService;
    }

    [HttpGet]
    public ApiResponse<List<ColumnDTO>> GetAll()
    {
        return _columnService.GetAll();
       
    }

    [HttpGet("{id}")]
    public ApiResponse<ColumnDTO> GetById(int id)
    {
       return _columnService.GetById(id);
    }

    [HttpPost]
    public ApiResponse<ColumnDTO> Create(CreateColumnDTO columnRequest)
    {
       return _columnService.Create(columnRequest);
    }

    [HttpPut("{id}")]
    public ApiResponse<ColumnDTO> Update(int id, ColumnDTO column)
    {
      return _columnService.Update(id, column);
    }

    [HttpDelete("{id}")]
    public ApiResponse<ColumnDTO> Delete(int id)
    {
       return _columnService.Delete(id);
    }
}