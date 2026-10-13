using TaskManager.Dtos;

namespace TaskManager.Services;

public interface ITaskService
{
    Task<List<TaskDto>> GetAllAsync(int userId, TaskQuery taskQuery);
    Task<TaskDto?> GetByIdAsync(int userId, int id);
    Task<TaskDto?> CreateAsync(int userId, CreateTaskDto input);
    Task<UpdateResult> UpdateAsync(int userId, int id, CreateTaskDto input);
    Task<bool> DeleteAsync(int userId, int id);
}

public enum UpdateResult
{
    Success,
    TaskNotFound,
    ProjectNotFound
}