using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using taskmanager_api.Models;
using taskmanager_api.Services;

namespace taskmanager_api.Controllers;

[Authorize]
public class TasksController : Controller
{
    private readonly ITaskService _taskService;
    private readonly IProjectService _projectService;
    private readonly UserManager<ApplicationUser> _userManager;

    public TasksController(ITaskService taskService, IProjectService projectService, UserManager<ApplicationUser> userManager)
    {
        _taskService = taskService;
        _projectService = projectService;
        _userManager = userManager;
    }

    // GET: Tasks
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var tasks = await _taskService.GetAllAsync(userId!);
        return View(tasks);
    }

    // GET: Tasks/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var task = await _taskService.GetByIdAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

    // GET: Tasks/Pending
    public async Task<IActionResult> Pending()
    {
        var userId = _userManager.GetUserId(User)!;
        var tasks = await _taskService.GetPendingAsync(userId);
        return View(tasks);
    }

    // GET: Tasks/Create
    public async Task<IActionResult> Create()
    {
        await PopulateProjectsDropdown();
        return View();
    }

    // POST: Tasks/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TaskItem task)
    {
        if (!ModelState.IsValid)
        {
            await PopulateProjectsDropdown();
            return View(task);
        }
        await _taskService.AddAsync(task);
        return RedirectToAction(nameof(Index));
    }

    // GET: Tasks/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var task = await _taskService.GetByIdAsync(id);
        if (task == null) return NotFound();
        await PopulateProjectsDropdown(task.ProjectId);
        return View(task);
    }

    // POST: Tasks/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TaskItem task)
    {
        if (!ModelState.IsValid)
        {
            await PopulateProjectsDropdown(task.ProjectId);
            return View(task);
        }
        await _taskService.UpdateAsync(task);
        return RedirectToAction(nameof(Index));
    }

    // GET: Tasks/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _taskService.GetByIdAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

    // POST: Tasks/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _taskService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateProjectsDropdown(int? selectedId = null)
    {
        var userId = _userManager.GetUserId(User)!;
        var projects = await _projectService.GetAllAsync(userId);
        ViewBag.ProjectId = new SelectList(projects, "Id", "Name", selectedId);
    }
}
