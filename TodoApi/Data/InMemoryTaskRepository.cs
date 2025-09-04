using System.Collections.Concurrent;
using TodoApi.Models;

namespace TodoApi.Data;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly ConcurrentDictionary<int, TaskItem> _tasks = new();
    private int _nextId = 1;

    public async Task<TaskItem> CreateAsync(TaskItem task)
    {
        task.Id = Interlocked.Increment(ref _nextId);
        _tasks[task.Id] = task;
        return await System.Threading.Tasks.Task.FromResult(task);
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await System.Threading.Tasks.Task.FromResult(_tasks.Values);
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        _tasks.TryGetValue(id, out var task);
        return await System.Threading.Tasks.Task.FromResult(task);
    }

    public async Task<TaskItem?> UpdateAsync(int id, TaskItem task)
    {
        if (_tasks.TryGetValue(id, out _))
        {
            task.Id = id;
            _tasks[id] = task;
            return task;
        }
        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await System.Threading.Tasks.Task.FromResult(_tasks.TryRemove(id, out _));
    }
}