using TaskManager.Dtos;

namespace TaskManager.Services;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllAsync(int userId);
    Task<ProjectDto?> GetByIdAsync(int userId, int id);
    Task<ProjectDto> CreateAsync(int userId, CreateProjectDto input);
    Task<bool> UpdateAsync(int userId, int id, CreateProjectDto input);
    Task<bool> DeleteAsync(int userId, int id);
    Task<bool> ExistsAsync(int userId, int id);
}