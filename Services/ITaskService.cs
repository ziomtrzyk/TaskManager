using TaskManager.Dtos;

namespace TaskManager.Services;

public interface ITaskService
{
    Task<List<TaskDto>> GetAllAsync(int userId, TaskQuery taskQuery, CancellationToken cancellationToken);
    Task<TaskDto?> GetByIdAsync(int userId, int id, CancellationToken cancellationToken);
    Task<TaskDto?> CreateAsync(int userId, CreateTaskDto input, CancellationToken cancellationToken);
    Task<UpdateResult> UpdateAsync(int userId, int id, CreateTaskDto input, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int userId, int id, CancellationToken cancellationToken);
    Task<(UpdateResult Result,TaskDto? FollowUp)> CompleteWithFollowUpAsync(int userId, int id, CompleteTaskDto input, CancellationToken cancellationToken);
}

public enum UpdateResult
{
    Success,
    TaskNotFound,
    ProjectNotFound
}