using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Dtos;
using TaskManager.Mappings;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAll()
    => await db.Projects.Select(p => p.ToDto()).ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetById(int id)
    {
        var project = await db.Projects.FindAsync(id);
        return project is null ? NotFound() : project.ToDto();
    }
    [HttpGet("{id}/tasks")]
    public async Task<ActionResult<List<TaskDto>>> GetAllFromProject(int id)
    {
        if (!await db.Projects.AnyAsync(p => p.Id == id))
            return NotFound();

        return await db.TaskItems
        .Where(t => t.ProjectId == id)
        .Select(t => t.ToDto())
        .ToListAsync();
    }


    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto input)
    {
        var project = input.ToProject();
        db.Projects.Add(project);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CreateProjectDto input)
    {
        var project = await db.Projects.FindAsync(id);
        if (project is null) return NotFound();

        project.UpdateProject(input);

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