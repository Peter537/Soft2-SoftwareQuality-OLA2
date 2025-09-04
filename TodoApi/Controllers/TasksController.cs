using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;

    public TasksController(ITaskRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> PostTask(TaskItem task)
    {
        if (string.IsNullOrEmpty(task.Title))
        {
            return BadRequest("Title is required.");
        }
        var created = await _repository.CreateAsync(task);
        return CreatedAtAction(nameof(GetTask), new { id = created.Id }, created);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
        return Ok(await _repository.GetAllAsync());
    }

    [HttpGet("{id}", Name = nameof(GetTask))]
    public async Task<ActionResult<TaskItem>> GetTask(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskItem>> PutTask(int id, TaskItem task)
    {
        var updated = await _repository.UpdateAsync(id, task);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}