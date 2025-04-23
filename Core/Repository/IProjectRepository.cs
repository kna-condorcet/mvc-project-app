using Condorcet.B2.AspnetCore.MVC.Application.Core.Domain;

namespace Condorcet.B2.AspnetCore.MVC.Application.Core.Repository
{
    public interface IProjectRepository
    {

        public Task<List<Project>> GetAll();
        public Task<Project?> GetById(int id);
        public Task<int> Insert(Project project);
        public Task<int> Update(int id, Project project);

        Task<bool> Exists(string? name);
        Task<bool> ProjectCodeExists(string code);
        Task<bool> ProjectCodeExists(string code, int excludedId);
    }
}
