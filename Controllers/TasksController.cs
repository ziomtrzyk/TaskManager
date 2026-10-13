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
    public async Task<ActionResult<List<TaskDto>>> GetAll([FromQuery] TaskQuery taskQuery)
    {
        return await service.GetAllAsync(User.GetUserId(), taskQuery);
    }

    [HttpGet("{id}")]
    [ProducesResponseType<TaskDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> GetById(int id)
    {
        var found = await service.GetByIdAsync(User.GetUserId(), id);
        if (found is null)
            return NotFound();
        return found;
    }
    [HttpPost]
    [ProducesResponseType<TaskDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskDto>> Create(CreateTaskDto input)
    {
        var created = await service.CreateAsync(User.GetUserId(), input);
        if (created is null)
            return BadRequest("The project does not exist.");

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(int id, CreateTaskDto input)
    {
        return await service.UpdateAsync(User.GetUserId(), id, input) switch
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
    public async Task<ActionResult> Delete(int id)
    {
        return await service.DeleteAsync(User.GetUserId(), id) ? NoContent() : NotFound();
    }
}