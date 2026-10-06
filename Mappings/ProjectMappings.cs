using TaskManager.Dtos;
using TaskManager.Models;

namespace TaskManager.Mappings;

public static class ProjectMappings
{
    public static ProjectDto ToDto(this Project input) => new()
    {
        Id = input.Id,
        Name = input.Name,
        Description = input.Description
    };

    public static Project ToProject(this CreateProjectDto input) => new()
    {
        Name = input.Name,
        Description = input.Description
    };

    public static void UpdateProject(this Project project, CreateProjectDto input)
    {
        project.Name = input.Name;
        project.Description = input.Description;
    }
}