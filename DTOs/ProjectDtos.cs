using System.ComponentModel.DataAnnotations;
using taskmanager_api.Models;

namespace taskmanager_api.DTOs;

public class ProjectResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public List<TaskResponse> Tasks { get; set; } = [];
}

public class ProjectRequest
{
    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required] public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }

    public Project ToEntity() => new()
    {
        Name = Name,
        Description = Description,
        StartDate = StartDate,
        EndDate = EndDate,
        IsActive = IsActive
    };
}

public static class ProjectMappingExtensions
{
    public static ProjectResponse ToResponse(this Project p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        StartDate = p.StartDate,
        EndDate = p.EndDate,
        IsActive = p.IsActive,
        Tasks = p.Tasks.Select(t => t.ToResponse()).ToList()
    };
}