using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TaskItem>>> GetAll()
    {
        return await db.TaskItems.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetById(int id)
    {
        var taskItem = await db.TaskItems.FindAsync(id);

        if (taskItem is null)
            return NotFound();

        return taskItem;
    }
    [HttpPost]
    public async Task<ActionResult> Create(TaskItem taskItem)
    {
        if (!await db.Projects.AnyAsync(t => t.Id == taskItem.ProjectId))
            return BadRequest("The project does not exist.");

        db.TaskItems.Add(taskItem);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = taskItem.Id }, taskItem);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, TaskItem input)
    {
        var taskItem = await db.TaskItems.FindAsync(id);
        if (taskItem is null)
            return NotFound();

        if (!await db.Projects.AnyAsync(p => p.Id == input.ProjectId))
            return BadRequest("The project does not exist.");
        taskItem.Title = input.Title;
        taskItem.Status = input.Status;
        taskItem.DueDate = input.DueDate;
        taskItem.ProjectId = input.ProjectId;

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