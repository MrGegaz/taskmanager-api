using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using taskmanager_api.Models;
using taskmanager_api.Services;

namespace taskmanager_api.Controllers;

[Authorize]
public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProjectsController(IProjectService projectService, UserManager<ApplicationUser> userManager)
    {
        _projectService = projectService;
        _userManager = userManager;
    }

    // GET: Projects
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var projects = await _projectService.GetAllAsync(userId!);
        return View(projects);
    }

    // GET: Projects/Details/x
    public async Task<IActionResult> Details(int id)
    {
        var project = await _projectService.GetByIdAsync(id);
        if (project == null) return NotFound();
        return View(project);
    }
    
    // GET: Projects/Active
    public async Task<IActionResult> Active()
    {
        var userId = _userManager.GetUserId(User);
        var projects = await _projectService.GetActiveAsync(userId!);
        return View(projects);
    }

    // GET: Projects/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Projects/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Project project)
    {
        if (!ModelState.IsValid) return View(project);
        project.UserId = _userManager.GetUserId(User);
        await _projectService.CreateAsync(project);
        return RedirectToAction(nameof(Index));
    }

    // GET: Projects/Edit/x
    public async Task<IActionResult> Edit(int id)
    {
        var project = await _projectService.GetByIdAsync(id);
        if (project == null) return NotFound();
        return View(project);
    }

    // POST: Projects/Edit/x
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Project project)
    {
        if (!ModelState.IsValid) return View(project);
        var result = await _projectService.UpdateAsync(project);
        if (result == null) return NotFound();
        return RedirectToAction(nameof(Index));
    }

    // GET: Projects/Delete/x
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _projectService.GetByIdAsync(id);
        if (project == null) return NotFound();
        return View(project);
    }

    // POST: Projects/Delete/x
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _projectService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}