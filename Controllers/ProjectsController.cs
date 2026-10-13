using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Dtos;
using TaskManager.Services;
using TaskManager.Extensions;

namespace TaskManager.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ProjectsController(IProjectService service, ITaskService taskService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAll()
    => await service.GetAllAsync(User.GetUserId());

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetById(int id)
    {
        var found = await service.GetByIdAsync(User.GetUserId(), id);
        if (found is null)
            return NotFound();
        return found;
    }
    [HttpGet("{id}/tasks")]
    public async Task<ActionResult<List<TaskDto>>> GetAllFromProject(int id)
    {
        if (!await service.ExistsAsync(User.GetUserId(), id))
            return NotFound();
        return await taskService.GetAllAsync(User.GetUserId(), new TaskQuery { ProjectId = id });
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto input)
    {
        var created = await service.CreateAsync(User.GetUserId(), input);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CreateProjectDto input)
    {
        var updated = await service.UpdateAsync(User.GetUserId(), id, input);
        if (!updated)
            return NotFound();

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await service.DeleteAsync(User.GetUserId(), id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}