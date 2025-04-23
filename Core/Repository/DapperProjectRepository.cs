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
            var result =
                await connection.QueryAsync<Project>(
                    "SELECT id, title, project_code as projectCode, description, start_date as startDate, expected_end_date as expectedEndDate, priority, budget FROM projects ORDER BY id");
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
            const string sql = """
                               INSERT INTO projects 
                               (
                                   title, 
                                   project_code, 
                                   description, 
                                   start_date, 
                                   expected_end_date, 
                                   priority, 
                                   budget
                               )
                               VALUES 
                               (
                                   @Title, 
                                   @ProjectCode, 
                                   @Description, 
                                   @StartDate, 
                                   @ExpectedEndDate, 
                                   @Priority, 
                                   @Budget
                               )
                               RETURNING id
                               """;
            var id = await connection.ExecuteScalarAsync<int>(sql, project);

            return id;
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

        public async Task<bool> ProjectCodeExists(string code)
        {
            using var connection = await _dbConnectionProvider.CreateConnection();
            const string sql = """
                               SELECT COUNT(1) FROM projects 
                               WHERE project_code = @code
                               """;

            var count = await connection.ExecuteScalarAsync<int>(
                sql, new { code });

            return count > 0;
        }
    }
}