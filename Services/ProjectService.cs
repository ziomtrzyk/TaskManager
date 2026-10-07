using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Dtos;
using TaskManager.Mappings;

namespace TaskManager.Services;

public class ProjectService(AppDbContext db) : IProjectService
{
    public async Task<List<ProjectDto>> GetAllAsync()
    => await db.Projects.Select(p => p.ToDto()).ToListAsync();

    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var project = await db.Projects.FindAsync(id);
        return project?.ToDto();
    }
    public async Task<ProjectDto> CreateAsync(CreateProjectDto input)
    {
        var project = input.ToProject();
        db.Projects.Add(project);
        await db.SaveChangesAsync();
        return project.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, CreateProjectDto input)
    {
        var project = await db.Projects.FindAsync(id);
        if (project is null) return false;

        project.UpdateProject(input);

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await db.Projects.FindAsync(id);
        if (project is null) return false;

        db.Projects.Remove(project);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await db.Projects.AnyAsync(p => p.Id == id);
    }
}