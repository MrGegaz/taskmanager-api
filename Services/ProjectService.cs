using Microsoft.EntityFrameworkCore;
using taskmanager_api.Data;
using taskmanager_api.Models;

namespace taskmanager_api.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;

    public ProjectService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Project>> GetAllAsync(string userId)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Project>> GetActiveAsync(string userId)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .Where(p => p.IsActive && p.UserId == userId)
            .ToListAsync();
    }

    public async Task<Project?> CreateAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project?> UpdateAsync(Project project)
    {
        var existingProject = await _context.Projects.FindAsync(project.Id);
        if (existingProject == null) return null;

        existingProject.Name = project.Name;
        existingProject.Description = project.Description;
        existingProject.EndDate = project.EndDate;
        existingProject.IsActive = project.IsActive;

        await _context.SaveChangesAsync();
        return existingProject;
    }

    public async Task<Project> DeleteAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) throw new Exception("Projekt nije pronađen");

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return project;
    }
}