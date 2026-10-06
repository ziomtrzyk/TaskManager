using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Mappings;
using TaskManager.Dtos;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TaskDto>>> GetAll()
    {
        return await db.TaskItems.Select(t => t.ToDto()).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetById(int id)
    {
        var taskItem = await db.TaskItems.FindAsync(id);

        if (taskItem is null)
            return NotFound();

        return taskItem.ToDto();
    }
    [HttpPost]
    public async Task<ActionResult<TaskDto>> Create(CreateTaskDto input)
    {
        if (!await db.Projects.AnyAsync(p => p.Id == input.ProjectId))
            return BadRequest("The project does not exist.");

        var taskItem = input.ToTaskItem();
        db.TaskItems.Add(taskItem);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = taskItem.Id }, taskItem.ToDto());
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CreateTaskDto input)
    {
        var taskItem = await db.TaskItems.FindAsync(id);
        if (taskItem is null)
            return NotFound();

        if (!await db.Projects.AnyAsync(p => p.Id == input.ProjectId))
            return BadRequest("The project does not exist.");
        
        taskItem.UpdateItem(input);

        await db.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var taskItem = await db.TaskItems.FindAsync(id);
        if (taskItem is null)
            return NotFound();

        db.TaskItems.Remove(taskItem);
        await db.SaveChangesAsync();
        return NoContent();
    }
}