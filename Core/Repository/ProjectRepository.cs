using Condorcet.B2.AspnetCore.MVC.Application.Core.Domain;
using Condorcet.B2.AspnetCore.MVC.Application.Core.Extensions;
using Condorcet.B2.AspnetCore.MVC.Application.Core.Infrastructure;

namespace Condorcet.B2.AspnetCore.MVC.Application.Core.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public ProjectRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public async Task<List<Project>> GetAll()
        {
            using var connection = await _dbConnectionProvider.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT id, name, deadline FROM projects ORDER BY id";
            using var result = command.ExecuteReader();

            var projectList = new List<Project>();

            while (result.Read())
            {
                projectList.Add(result.MapToProject());
            }

            return projectList;
        }

        public async Task<Project?> GetById(int id)
        {
            using var connection = await _dbConnectionProvider.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT id, name, deadline FROM projects WHERE id = @id";
            var param = command.CreateParameter();
            param.ParameterName = "id";
            param.Value = id;
            command.Parameters.Add(param);

            using var result = command.ExecuteReader();

            if (result.Read())
            {
                return result.MapToProject();
            }

            return null;
        }

        public async Task<int> Insert(Project project)
        {
            using var connection = await _dbConnectionProvider.CreateConnection();
            using var commandInsertProject = connection.CreateCommand();

            commandInsertProject.CommandText = """
                                               INSERT INTO projects(name, deadline) 
                                               VALUES(@name, @deadline)
                                               """;
            var nameParam = commandInsertProject.CreateParameter();
            nameParam.ParameterName = "name";
            nameParam.Value = project.Name;

            var deadlineParam = commandInsertProject.CreateParameter();
            deadlineParam.ParameterName = "deadline";
            deadlineParam.Value = project.Deadline;

            commandInsertProject.Parameters.Add(nameParam);
            commandInsertProject.Parameters.Add(deadlineParam);

            var resultInsert = commandInsertProject.ExecuteNonQuery();

            return resultInsert;
        }

        public async Task<int> Update(int id, Project project)
        {
            using var connection = await _dbConnectionProvider.CreateConnection();
            using var commandUpdateProject = connection.CreateCommand();
            commandUpdateProject.CommandText = """
                                               UPDATE projects SET name = @name, deadline = @deadline
                                               WHERE id = @id;
                                               """;

            var nameUpdateParam = commandUpdateProject.CreateParameter();
            nameUpdateParam.ParameterName = "name";
            nameUpdateParam.Value = project.Name;

            var idParam = commandUpdateProject.CreateParameter();
            idParam.ParameterName = "id";
            idParam.Value = id;
            
            var deadlineParam = commandUpdateProject.CreateParameter();
            deadlineParam.ParameterName = "deadline";
            deadlineParam.Value = project.Deadline;

            commandUpdateProject.Parameters.Add(nameUpdateParam);
            commandUpdateProject.Parameters.Add(deadlineParam);
            commandUpdateProject.Parameters.Add(idParam);

            var resultupdate = commandUpdateProject.ExecuteNonQuery();

            return resultupdate;
        }
    }
}