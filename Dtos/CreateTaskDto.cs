using System.ComponentModel.DataAnnotations;
using TaskManager.Models;
using TaskManager.Validation;

namespace TaskManager.Dtos;

public class CreateTaskDto
{
    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    public TaskState Status { get; set; } = TaskState.ToDo;
    [NotInPast]
    public DateTime? DueDate { get; set; }
    [Range(1, int.MaxValue)]
    public int ProjectId { get; set; }
}