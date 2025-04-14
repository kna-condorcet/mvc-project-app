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
        public async Task<IActionResult> Create(CreateProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _projectRepository.Insert(new Project
            {
                Name = model.Name
            });
            return RedirectToAction(nameof(Index));
        }

    }
}
