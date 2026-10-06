using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models;

public class Project
{
    public int Id {get; set;}
    public string Name {get; set;} = string.Empty;
    public string? Description {get; set;}
    public List<TaskItem> TaskItems {get;set;} = new();
}