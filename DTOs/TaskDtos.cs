using System.ComponentModel.DataAnnotations;
using taskmanager_api.Models;

namespace taskmanager_api.DTOs;

public class TaskResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public Priority Priority { get; set; }
    public bool IsCompleted { get; set; }
    public string? Notes { get; set; }
    public int ProjectId { get; set; }
}

public class TaskRequest
{
    [Required] [MaxLength(100)] public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public Priority Priority { get; set; } = Priority.Normalan;
    public bool IsCompleted { get; set; }
    [MaxLength(300)] public string? Notes { get; set; }
    [Required] public int ProjectId { get; set; }
}

public static class TaskMappingExtensions
{
    public static TaskResponse ToResponse(this TaskItem t) => new()
    {
        Id = t.Id,
        Title = t.Title,
        Description = t.Description,
        CreatedAt = t.CreatedAt,
        DueDate = t.DueDate,
        Priority = t.Priority,
        IsCompleted = t.IsCompleted,
        Notes = t.Notes,
        ProjectId = t.ProjectId
    };
}