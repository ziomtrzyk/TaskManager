using TaskManager.Dtos;

namespace TaskManager.Services;

public interface ITaskService
{
    Task<List<TaskDto>> GetAllAsync(TaskQuery taskQuery);
    Task<TaskDto?> GetByIdAsync(int id);
    Task<TaskDto?> CreateAsync(CreateTaskDto input);
    Task<UpdateResult> UpdateAsync(int id, CreateTaskDto input);
    Task<bool> DeleteAsync(int id);
}

public enum UpdateResult
{
    Success,
    TaskNotFound,
    ProjectNotFound
}