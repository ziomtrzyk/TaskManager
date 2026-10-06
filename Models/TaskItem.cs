using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public TaskState Status { get; set; } = TaskState.ToDo;
    public DateTime? DueDate { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }


}

public enum TaskState
{
    ToDo,
    InProgress,
    Done
}