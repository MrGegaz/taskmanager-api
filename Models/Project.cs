using System.ComponentModel.DataAnnotations;

namespace taskmanager_api.Models;

public class Project
{
    public int Id { get; set; }
    
    public string? UserId { get; set; }
    
    public ApplicationUser? User { get; set; }

    [Required] [MaxLength(100)] public string Name { get; set; } = String.Empty;
    
    public string? Description { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    [Required]
    public Boolean IsActive { get; set; }

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

}