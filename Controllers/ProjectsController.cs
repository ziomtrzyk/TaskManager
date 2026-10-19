using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Dtos;
using TaskManager.Services;
using TaskManager.Extensions;

namespace TaskManager.Controllers;

[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ApiController]
[Route("[controller]")]
public class ProjectsController(IProjectService service, ITaskService taskService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<List<ProjectDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProjectDto>>> GetAll(CancellationToken cancellationToken)
    => await service.GetAllAsync(User.GetUserId(), cancellationToken);

    [HttpGet("{id}")]
    [ProducesResponseType<ProjectDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var found = await service.GetByIdAsync(User.GetUserId(), id, cancellationToken);
        if (found is null)
            return NotFound();
        return found;
    }
    [HttpGet("{id}/tasks")]
    [ProducesResponseType<List<TaskDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<TaskDto>>> GetAllFromProject(int id, CancellationToken cancellationToken)
    {
        if (!await service.ExistsAsync(User.GetUserId(), id, cancellationToken))
            return NotFound();
        return await taskService.GetAllAsync(User.GetUserId(), new TaskQuery { ProjectId = id }, cancellationToken);
    }

    [HttpPost]
    [ProducesResponseType<ProjectDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto input, CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(User.GetUserId(), input, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, CreateProjectDto input, CancellationToken cancellationToken)
    {
        var updated = await service.UpdateAsync(User.GetUserId(), id, input, cancellationToken);
        if (!updated)
            return NotFound();

        return NoContent();
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(User.GetUserId(), id, cancellationToken);
        if (!deleted) return NotFound();

        return NoContent();
    }
    [HttpGet("with-tasks")]
    [ProducesResponseType<List<ProjectWithTaskDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProjectWithTaskDto>>> GetAllWithTasks(CancellationToken cancellationToken)
        => await service.GetAllWithTasksAsync(User.GetUserId(), cancellationToken);
}