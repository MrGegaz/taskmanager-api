using System.ComponentModel.DataAnnotations;

namespace taskmanager_api.Models;

public class TaskItem
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = String.Empty;
    
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime DueDate { get; set; }

    public Priority Priority { get; set; } = Priority.Normalan;
    
    public bool IsCompleted { get; set; }
    
    [MaxLength(300)]
    public string? Notes { get; set; }
    
    // Foreign Key
    public int ProjectId { get; set; }
    
    public Project? Project { get; set; }
}