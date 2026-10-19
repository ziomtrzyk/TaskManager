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
public class TasksController(ITaskService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<List<TaskDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<TaskDto>>> GetAll([FromQuery] TaskQuery taskQuery, CancellationToken cancellationToken)
    {
        return await service.GetAllAsync(User.GetUserId(), taskQuery, cancellationToken);
    }

    [HttpGet("{id}")]
    [ProducesResponseType<TaskDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var found = await service.GetByIdAsync(User.GetUserId(), id, cancellationToken);
        if (found is null)
            return NotFound();
        return found;
    }
    [HttpPost]
    [ProducesResponseType<TaskDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskDto>> Create(CreateTaskDto input, CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(User.GetUserId(), input, cancellationToken);
        if (created is null)
            return BadRequest("The project does not exist.");

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(int id, CreateTaskDto input, CancellationToken cancellationToken)
    {
        return await service.UpdateAsync(User.GetUserId(), id, input, cancellationToken) switch
        {
            UpdateResult.Success => NoContent(),
            UpdateResult.TaskNotFound => NotFound(),
            UpdateResult.ProjectNotFound => BadRequest("The project does not exist."),
            _ => StatusCode(500)
        };
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await service.DeleteAsync(User.GetUserId(), id, cancellationToken) ? NoContent() : NotFound();
    }
    [HttpPost("{id}/complete")]
    [ProducesResponseType<TaskDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskDto>> Complete(int id, CompleteTaskDto input, CancellationToken cancellationToken)
    {
        var (result, followUp) = await service.CompleteWithFollowUpAsync(User.GetUserId(), id, input, cancellationToken);

        return result switch
        {
            UpdateResult.Success => Ok(followUp),
            UpdateResult.TaskNotFound => NotFound(),
            UpdateResult.ProjectNotFound => BadRequest("The project does not exist."),
            _=> StatusCode(500)
        };
    }
}