using TaskManager.Dtos;

namespace TaskManager.Services;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllAsync();
    Task<ProjectDto?> GetByIdAsync(int id);
    Task<ProjectDto> CreateAsync(CreateProjectDto input);
    Task<bool> UpdateAsync(int id, CreateProjectDto input);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}