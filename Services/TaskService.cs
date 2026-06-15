using Microsoft.EntityFrameworkCore;
using taskmanager_api.Data;
using taskmanager_api.Models;

namespace taskmanager_api.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    // Zadaci nemaju vlastiti UserId jer se određuje preko Project.UserId
    public async Task<IEnumerable<TaskItem>> GetAllAsync(string userId)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .Where(t => t.Project!.UserId == userId)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<TaskItem>> GetPendingAsync(string userId)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .Where(t => t.Project!.UserId == userId && !t.IsCompleted)
            .ToListAsync();
    }

    public async Task AddAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return;
        
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
}