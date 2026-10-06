using TaskManager.Models;

namespace TaskManager.Dtos;

public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public TaskState Status { get; set; }
    public DateTime? DueDate { get; set; }
    public int ProjectId { get; set; }
}