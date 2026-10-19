using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Dtos;
using TaskManager.Mappings;
using TaskManager.Models;

namespace TaskManager.Services;

public class ProjectService(AppDbContext db) : IProjectService
{
    public async Task<List<ProjectDto>> GetAllAsync(int userId, CancellationToken cancellationToken)
    => await db.Projects.Where(p => p.UserId == userId).Select(p => p.ToDto()).ToListAsync(cancellationToken);

    public async Task<ProjectDto?> GetByIdAsync(int userId, int id, CancellationToken cancellationToken)
    {
        var project = await db.Projects
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        return project?.ToDto();
    }
    public async Task<ProjectDto> CreateAsync(int userId, CreateProjectDto input, CancellationToken cancellationToken)
    {
        var project = input.ToProject();
        project.UserId = userId;
        db.Projects.Add(project);
        await db.SaveChangesAsync(cancellationToken);
        return project.ToDto();
    }

    public async Task<bool> UpdateAsync(int userId, int id, CreateProjectDto input, CancellationToken cancellationToken)
    {
        var project = await db.Projects.Where(p => p.UserId == userId).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (project is null) return false;

        project.UpdateProject(input);

        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int userId, int id, CancellationToken cancellationToken)
    {
        var project = await db.Projects.Where(p => p.UserId == userId).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (project is null) return false;

        db.Projects.Remove(project);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(int userId, int id, CancellationToken cancellationToken)
    {
        return await db.Projects.AnyAsync(p => p.Id == id && p.UserId == userId, cancellationToken);
    }
    public async Task<List<ProjectWithTaskDto>> GetAllWithTasksAsync(int userId, CancellationToken cancellationToken)
    {
        var projects = await db.Projects
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Include(p => p.TaskItems)
            .ToListAsync(cancellationToken);

        return projects.Select(p => p.ToDtoWithTasks()).ToList();
    }
}