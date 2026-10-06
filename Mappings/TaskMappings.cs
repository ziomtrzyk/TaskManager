using TaskManager.Dtos;
using TaskManager.Models;

namespace TaskManager.Mappings;

public static class TaskMappings
{
    public static TaskDto ToDto(this TaskItem input) => new()
    {
        Id = input.Id,
        Title = input.Title,
        Status = input.Status,
        DueDate = input.DueDate,
        ProjectId = input.ProjectId
    };

    public static TaskItem ToTaskItem(this CreateTaskDto input) => new()
    {
        Title = input.Title,
        Status = input.Status,
        DueDate = input.DueDate,
        ProjectId = input.ProjectId
    };

    public static void UpdateItem(this TaskItem taskItem, CreateTaskDto input)
    {
        taskItem.Title = input.Title;
        taskItem.Status = input.Status;
        taskItem.DueDate = input.DueDate;
        taskItem.ProjectId = input.ProjectId;
    }
}