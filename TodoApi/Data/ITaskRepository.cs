using TodoApi.Models;

namespace TodoApi.Data;

public interface ITaskRepository
{
    Task<TaskItem> CreateAsync(TaskItem task);
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem?> UpdateAsync(int id, TaskItem task);
    Task<bool> DeleteAsync(int id);
}