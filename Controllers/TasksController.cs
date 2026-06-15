using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taskmanager_api.DTOs;
using taskmanager_api.Models;
using taskmanager_api.Services;

namespace taskmanager_api.Controllers;

[Authorize]
[Route("api/tasks")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly IProjectService _projectService;

    public TasksController(ITaskService taskService, IProjectService projectService)
    {
        _taskService = taskService;
        _projectService = projectService;
    }

    // GET: api/tasks
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var tasks = await _taskService.GetAllAsync(userId);
        return Ok(tasks.Select(t => t.ToResponse()));
    }

    // GET: api/tasks/pending
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var tasks = await _taskService.GetPendingAsync(userId);
        return Ok(tasks.Select(t => t.ToResponse()));
    }

    // GET: api/tasks/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var task = await _taskService.GetByIdAsync(id);
        if (task == null || task.Project!.UserId != userId) return NotFound();
        return Ok(task.ToResponse());
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<IActionResult> Create(TaskRequest dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var project = await _projectService.GetByIdAsync(dto.ProjectId);
        if (project == null || project.UserId != userId) return BadRequest("Projekt ne postoji ili nemate pristup.");

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Priority = dto.Priority,
            IsCompleted = dto.IsCompleted,
            Notes = dto.Notes,
            ProjectId = dto.ProjectId
        };

        await _taskService.AddAsync(task);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task.ToResponse());
    }

    // PUT: api/tasks/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TaskRequest dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var existing = await _taskService.GetByIdAsync(id);
        if (existing == null || existing.Project!.UserId != userId) return NotFound();

        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.DueDate = dto.DueDate;
        existing.Priority = dto.Priority;
        existing.IsCompleted = dto.IsCompleted;
        existing.Notes = dto.Notes;

        await _taskService.UpdateAsync(existing);
        return Ok(existing.ToResponse());
    }

    // DELETE: api/tasks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var task = await _taskService.GetByIdAsync(id);
        if (task == null || task.Project!.UserId != userId) return NotFound();
        await _taskService.DeleteAsync(id);
        return NoContent();
    }
}