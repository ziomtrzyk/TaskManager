
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Dtos;
using TaskManager.Mappings;
using TaskManager.Models;


namespace TaskManager.Services;

public class TaskService(AppDbContext db) : ITaskService

{
    public async Task<List<TaskDto>> GetAllAsync(int userId, TaskQuery taskQuery, CancellationToken cancellationToken)
    {
        var query = db.TaskItems.Where(t => t.Project!.UserId == userId);
        if (taskQuery.ProjectId is not null)
            query = query.Where(t => t.ProjectId == taskQuery.ProjectId);
        if (taskQuery.Status is not null)
            query = query.Where(t => t.Status == taskQuery.Status);
        query = taskQuery.OrderBy?.ToLower() switch
        {
            "status" => query.OrderBy(t => t.Status),
            "title" => query.OrderBy(t => t.Title),
            "duedate" => query.OrderBy(t => t.DueDate == null).ThenBy(t => t.DueDate),
            _ => query.OrderBy(t => t.Id)
        };
        return await query.Select(t => t.ToDto()).ToListAsync(cancellationToken);
    }

    public async Task<TaskDto?> GetByIdAsync(int userId, int id, CancellationToken cancellationToken)
    {
        var taskItem = await db.TaskItems
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id && t.Project!.UserId == userId, cancellationToken);

        if (taskItem is null)
            return null;

        return taskItem.ToDto();
    }

    public async Task<TaskDto?> CreateAsync(int userId, CreateTaskDto input, CancellationToken cancellationToken)
    {
        if (!await db.Projects.AnyAsync(p => p.Id == input.ProjectId && p.UserId == userId, cancellationToken))
            return null;

        var taskItem = input.ToTaskItem();
        db.TaskItems.Add(taskItem);
        await db.SaveChangesAsync(cancellationToken);
        return taskItem.ToDto();
    }

    public async Task<UpdateResult> UpdateAsync(int userId, int id, CreateTaskDto input, CancellationToken cancellationToken)
    {
        var taskItem = await db.TaskItems.FirstOrDefaultAsync(t => t.Id == id && t.Project!.UserId == userId, cancellationToken);
        if (taskItem is null)
            return UpdateResult.TaskNotFound;

        if (!await db.Projects.AnyAsync(p => p.Id == input.ProjectId && p.UserId == userId, cancellationToken))
            return UpdateResult.ProjectNotFound;

        taskItem.UpdateItem(input);

        await db.SaveChangesAsync(cancellationToken);
        return UpdateResult.Success;
    }

    public async Task<bool> DeleteAsync(int userId, int id, CancellationToken cancellationToken)
    {
        var taskItem = await db.TaskItems.FirstOrDefaultAsync(t => t.Id == id && t.Project!.UserId == userId, cancellationToken);
        if (taskItem is null)
            return false;

        db.TaskItems.Remove(taskItem);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
    public async Task<(UpdateResult Result,TaskDto? FollowUp)> CompleteWithFollowUpAsync(int userId, int id, CompleteTaskDto input, CancellationToken cancellationToken)
    {
        var taskItem = await db.TaskItems.FirstOrDefaultAsync(t => t.Id == id && t.Project!.UserId == userId, cancellationToken);
        if(taskItem is null)
            return (UpdateResult.TaskNotFound, null);
        
        if(!await db.Projects.AnyAsync(p => p.Id == input.FollowUpProjectId && p.UserId == userId, cancellationToken))
            return (UpdateResult.ProjectNotFound, null);

        taskItem.Status = TaskState.Done;

        var followUp = new TaskItem
        {
            Title = input.FollowUpTitle,
            ProjectId = input.FollowUpProjectId
        };
        db.TaskItems.Add(followUp);

        await db.SaveChangesAsync(cancellationToken);
        return (UpdateResult.Success, followUp.ToDto());
    }
}