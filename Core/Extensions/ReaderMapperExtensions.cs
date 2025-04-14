using System.Data;
using Condorcet.B2.AspnetCore.MVC.Application.Core.Domain;

namespace Condorcet.B2.AspnetCore.MVC.Application.Core.Extensions
{
    internal static class ReaderMapperExtensions
    {
        public static Project MapToProject(this IDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("id"));
            var name = reader.GetString(reader.GetOrdinal("name"));
            var isDeadlineNull = reader.IsDBNull(reader.GetOrdinal("deadline"));
            DateTime? deadline = isDeadlineNull ? null : reader.GetDateTime(reader.GetOrdinal("deadline"));

            return new Project
            {
                Id = id,
                Name = name,
                Deadline = deadline
            };
        }
    }
}
