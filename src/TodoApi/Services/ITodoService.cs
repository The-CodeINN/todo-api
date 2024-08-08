using TodoApi.DTOs;

namespace TodoApi.Services;

public interface ITodoService
{
    Task<Response<IEnumerable<TodoItemDto>>> GetAllAsync();
    Task<Response<TodoItemDto>> GetByIdAsync(int id);
    Task<Response<TodoItemDto>> AddAsync(TodoItemCreateDto itemDto);
    Task<Response<TodoItemDto>> UpdateAsync(TodoItemDto itemDto);
    Task<Response<string>> DeleteAsync(int id);
}
