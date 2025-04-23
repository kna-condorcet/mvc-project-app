using Condorcet.B2.AspnetCore.MVC.Application.Core.Domain;
using Condorcet.B2.AspnetCore.MVC.Application.Core.Infrastructure;
using Dapper;

namespace Condorcet.B2.AspnetCore.MVC.Application.Core.Repository
{
    public class DapperProjectRepository : IProjectRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public DapperProjectRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public async Task<List<Project>> GetAll()
        {
            using var connection = await _dbConnectionProvider.CreateConnection();
            var result = await connection.QueryAsync<Project>("SELECT id, name, deadline FROM projects ORDER BY id");
            return result.ToList();
        }

        public async Task<Project?> GetById(int id)
        {
            using var connection = await _dbConnectionProvider.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Project>("SELECT id, name, deadline FROM projects WHERE id = @id",
                new { id });
        }

        public async Task<int> Insert(Project project)
        {
            using var connection = await _dbConnectionProvider.CreateConnection();
            var newId = await connection.ExecuteScalarAsync<int>("""
                                                 INSERT INTO projects(name, deadline) 
                                                 VALUES(@name, @deadline)
                                                 RETURNING id
                                                 """, project);
            return newId;
        }

        public async Task<int> Update(int id, Project project)
        {
            project.Id = id;
            using var connection = await _dbConnectionProvider.CreateConnection();
            return await connection.ExecuteAsync("""
                                                 UPDATE projects SET name = @name, deadline = @deadline
                                                 WHERE id = @id;
                                                 """, project);
        }

        public async Task<bool> Exists(string? name)
        {
            using var connection = await _dbConnectionProvider.CreateConnection();
            return await connection.ExecuteScalarAsync<bool>("""
                                                             SELECT EXISTS (SELECT 1 FROM projects WHERE name = @name)
                                                             """, new {name});
        }
    }
}