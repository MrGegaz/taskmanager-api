using taskmanager_api.Models;

namespace taskmanager_api.Services;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetAllAsync(string userId);
    Task<Project?> GetByIdAsync(int id);
    Task<IEnumerable<Project>> GetActiveAsync(string userId);
    Task<Project?> CreateAsync(Project project);
    Task<Project?> UpdateAsync(Project project);
    Task<Project> DeleteAsync(int id);
}