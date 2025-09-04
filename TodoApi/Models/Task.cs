namespace TodoApi.Models;

public enum TaskStatus
{
    CREATED,
    IN_PROGRESS,
    COMPLETED
}

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.CREATED;

    public TaskItem() { }

    public TaskItem(int id, string title, string description, TaskStatus status)
    {
        Id = id;
        Title = title;
        Description = description;
        Status = status;
    }
}