namespace TaskManager.Dtos;

public class ProjectWithTaskDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<TaskDto> Tasks { get; set; } = new();
}