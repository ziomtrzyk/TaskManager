using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Dtos;
using TaskManager.Mappings;
using TaskManager.Models;

namespace TaskManager.Services;

public class ProjectService(AppDbContext db) : IProjectService
{
    public async Task<List<ProjectDto>> GetAllAsync(int userId)
    => await db.Projects.Where(p => p.UserId == userId).Select(p => p.ToDto()).ToListAsync();

    public async Task<ProjectDto?> GetByIdAsync(int userId, int id)
    {
        var project = await db.Projects.Where(p => p.UserId == userId).FirstOrDefaultAsync(p => p.Id == id);
        return project?.ToDto();
    }
    public async Task<ProjectDto> CreateAsync(int userId, CreateProjectDto input)
    {
        var project = input.ToProject();
        project.UserId = userId;
        db.Projects.Add(project);
        await db.SaveChangesAsync();
        return project.ToDto();
    }

    public async Task<bool> UpdateAsync(int userId, int id, CreateProjectDto input)
    {
        var project = await db.Projects.Where(p => p.UserId == userId).FirstOrDefaultAsync(p => p.Id == id);
        if (project is null) return false;

        project.UpdateProject(input);

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int userId, int id)
    {
        var project = await db.Projects.Where(p => p.UserId == userId).FirstOrDefaultAsync(p => p.Id == id);
        if (project is null) return false;

        db.Projects.Remove(project);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int userId, int id)
    {
        return await db.Projects.AnyAsync(p => p.Id == id && p.UserId == userId);
    }
    public async Task<List<ProjectWithTaskDto>> GetAllWithTasksAsync(int userId)
    {
        var projects = await db.Projects
            .Where(p => p.UserId == userId)
            .Include(p => p.TaskItems)
            .ToListAsync();

        return projects.Select(p => p.ToDtoWithTasks()).ToList();
    }
}