using Condorcet.B2.AspnetCore.MVC.Application.Core.Domain;
using Condorcet.B2.AspnetCore.MVC.Application.Core.Repository;
using Condorcet.B2.AspnetCore.MVC.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Condorcet.B2.AspnetCore.MVC.Application.Controllers
{
    public class ProjectsController : Controller
    {

        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        // GET: ProjectsController
        public async Task<ActionResult> Index()
        {
            var projects = await _projectRepository.GetAll();
            return View(new ProjectIndexViewModel()
            {
                Projects = projects
            });
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(ProjectFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _projectRepository.Insert(new Project
            {
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                ExpectedEndDate = model.ExpectedEndDate,
                Priority = (int)model.Priority,
                Budget = model.Budget,
                ProjectCode = model.ProjectCode
            });
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectRepository.GetById(id);
            if (project == null)
                return NotFound();
            return View(new ProjectFormViewModel()
            {
                Id = project.Id,
                ProjectCode = project.ProjectCode,
                Title = project.Title,
                Description = project.Description,
                StartDate = project.StartDate,
                ExpectedEndDate = project.ExpectedEndDate,
                Priority = (ProjectPriority)project.Priority,
                Budget = project.Budget
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(ProjectFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _projectRepository.Update(model.Id.Value, new Project
            {
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                ExpectedEndDate = model.ExpectedEndDate,
                Priority = (int)model.Priority,
                Budget = model.Budget,
                ProjectCode = model.ProjectCode
            });
            return RedirectToAction(nameof(Index));
        }

    }
}
