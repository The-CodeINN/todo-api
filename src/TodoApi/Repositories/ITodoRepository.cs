using TodoApi.Entities;

namespace TodoApi.Repositories;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> GetAllAsync();
    Task<TodoItem> GetByIdAsync(int id);
    Task<TodoItem> AddAsync(TodoItem item);
    Task<TodoItem> UpdateAsync(TodoItem item);
    Task DeleteAsync(int id);
}
