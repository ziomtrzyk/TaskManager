using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Project>>> GetAll()
    => await db.Projects.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetById(int id)
    {
        var project = await db.Projects.FindAsync(id);
        return project is null ? NotFound() : project;
    }
    [HttpGet("{id}/tasks")]
    public async Task<ActionResult<List<TaskItem>>> GetAllFromProject(int id)
    {
        if (!await db.Projects.AnyAsync(p => p.Id == id))
            return NotFound();

        return await db.TaskItems
        .Where(t => t.ProjectId == id)
        .ToListAsync();
    }


    [HttpPost]
    public async Task<ActionResult<Project>> Create(Project project)
    {
        db.Projects.Add(project);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, Project input)
    {
        var project = await db.Projects.FindAsync(id);
        if (project is null) return NotFound();

        project.Name = input.Name;
        project.Description = input.Description;

        await db.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var project = await db.Projects.FindAsync(id);
        if (project is null) return NotFound();

        db.Projects.Remove(project);
        await db.SaveChangesAsync();
        return NoContent();
    }
}