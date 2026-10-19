using TaskManager.Dtos;

namespace TaskManager.Services;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllAsync(int userId, CancellationToken cancellationToken);
    Task<ProjectDto?> GetByIdAsync(int userId, int id, CancellationToken cancellationToken);
    Task<ProjectDto> CreateAsync(int userId, CreateProjectDto input, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(int userId, int id, CreateProjectDto input, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int userId, int id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(int userId, int id, CancellationToken cancellationToken);
    Task<List<ProjectWithTaskDto>> GetAllWithTasksAsync(int userId, CancellationToken cancellationToken);
}