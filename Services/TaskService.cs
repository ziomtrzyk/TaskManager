
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Dtos;
using TaskManager.Mappings;


namespace TaskManager.Services;

public class TaskService(AppDbContext db) : ITaskService

{
    public async Task<List<TaskDto>> GetAllAsync()
    {
        return await db.TaskItems.Select(t => t.ToDto()).ToListAsync();
    }

    public async Task<TaskDto?> GetByIdAsync(int id)
    {
        var taskItem = await db.TaskItems.FindAsync(id);

        if (taskItem is null)
            return null;

        return taskItem.ToDto();
    }

    public async Task<TaskDto?> CreateAsync(CreateTaskDto input)
    {
        if (!await db.Projects.AnyAsync(p => p.Id == input.ProjectId))
            return null;

        var taskItem = input.ToTaskItem();
        db.TaskItems.Add(taskItem);
        await db.SaveChangesAsync();
        return taskItem.ToDto();
    }

    public async Task<UpdateResult> UpdateAsync(int id, CreateTaskDto input)
    {
        var taskItem = await db.TaskItems.FindAsync(id);
        if (taskItem is null)
            return UpdateResult.TaskNotFound;

        if (!await db.Projects.AnyAsync(p => p.Id == input.ProjectId))
            return UpdateResult.ProjectNotFound;

        taskItem.UpdateItem(input);

        await db.SaveChangesAsync();
        return UpdateResult.Success;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var taskItem = await db.TaskItems.FindAsync(id);
        if (taskItem is null)
            return false;

        db.TaskItems.Remove(taskItem);
        await db.SaveChangesAsync();
        return true;
    }
}