using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taskmanager_api.DTOs;
using taskmanager_api.Services;

namespace taskmanager_api.Controllers;

[Authorize]
[Route("api/projects")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // GET: api/projects
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var projects = await _projectService.GetAllAsync(userId);
        return Ok(projects.Select(p => p.ToResponse()));
    }

    // GET: api/projects/active
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var projects = await _projectService.GetActiveAsync(userId);
        return Ok(projects.Select(p => p.ToResponse()));
    }

    // GET: api/projects/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var project = await _projectService.GetByIdAsync(id);
        if (project == null || project.UserId != userId) return NotFound();
        return Ok(project.ToResponse());
    }

    // POST: api/projects
    [HttpPost]
    public async Task<IActionResult> Create(ProjectRequest dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var project = dto.ToEntity();
        project.UserId = userId;
        var created = await _projectService.CreateAsync(project);
        return CreatedAtAction(nameof(GetById), new { id = created!.Id }, created.ToResponse());
    }

    // PUT: api/projects/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProjectRequest dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var existing = await _projectService.GetByIdAsync(id);
        if (existing == null || existing.UserId != userId) return NotFound();

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.EndDate = dto.EndDate;
        existing.IsActive = dto.IsActive;

        var updated = await _projectService.UpdateAsync(existing);
        return Ok(updated!.ToResponse());
    }

    // DELETE: api/projects/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var project = await _projectService.GetByIdAsync(id);
        if (project == null || project.UserId != userId) return NotFound();
        await _projectService.DeleteAsync(id);
        return NoContent();
    }
}