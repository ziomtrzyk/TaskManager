using Microsoft.AspNetCore.Mvc;
using TaskManager.Dtos;
using TaskManager.Services;


namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController(ITaskService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TaskDto>>> GetAll()
    {
        return await service.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetById(int id)
    {
        var found = await service.GetByIdAsync(id);
        if (found is null)
            return NotFound();
        return found;
    }
    [HttpPost]
    public async Task<ActionResult<TaskDto>> Create(CreateTaskDto input)
    {
        var created = await service.CreateAsync(input);
        if (created is null)
            return BadRequest("The project does not exist.");

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CreateTaskDto input)
    {
        return await service.UpdateAsync(id, input) switch
        {
            UpdateResult.Success => NoContent(),
            UpdateResult.TaskNotFound => NotFound(),
            UpdateResult.ProjectNotFound => BadRequest("The project does not exist."),
            _ => StatusCode(500)
        };
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        return await service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}