using Microsoft.AspNetCore.Mvc;
using TaskManager.Dtos;
using TaskManager.Services;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectsController(IProjectService service, ITaskService taskService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAll()
    => await service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetById(int id)
    {
        var found = await service.GetByIdAsync(id);
        if (found is null)
            return NotFound();
        return found;
    }
    [HttpGet("{id}/tasks")]
    public async Task<ActionResult<List<TaskDto>>> GetAllFromProject(int id)
    {
        if (!await service.ExistsAsync(id))
            return NotFound();
        return await taskService.GetAllAsync(new TaskQuery { ProjectId = id });
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto input)
    {
        var created = await service.CreateAsync(input);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CreateProjectDto input)
    {
        var updated = await service.UpdateAsync(id, input);
        if (!updated)
            return NotFound();

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await service.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}