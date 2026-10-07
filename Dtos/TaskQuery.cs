using Microsoft.VisualBasic;
using TaskManager.Models;

namespace TaskManager.Dtos;

public class TaskQuery
{
    public int? ProjectId { get; set; }
    public TaskState? Status { get; set; }
    public DateTime? DueDate { get; set; }
    public string? OrderBy { get; set; }
}