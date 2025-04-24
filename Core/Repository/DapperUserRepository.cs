using Condorcet.B2.AspnetCore.MVC.Application.Core.Domain;
using Condorcet.B2.AspnetCore.MVC.Application.Core.Infrastructure;
using Dapper;

namespace Condorcet.B2.AspnetCore.MVC.Application.Core.Repository;

public class DapperUserRepository : IUserRepository
{
    private readonly IDbConnectionProvider _dbConnectionProvider;

    public DapperUserRepository(IDbConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
    }

    public async Task<int> CreateUserAsync(User user)
    {
        using var connection = await _dbConnectionProvider.CreateConnection();
        const string sql = """
                           INSERT INTO users (username, email, password_hash, salt, first_name, last_name, is_active, role)
                           VALUES (@Username, @Email, @PasswordHash, @Salt, @FirstName, @LastName, @IsActive, @Role)
                           RETURNING id
                           """;

        return await connection.ExecuteScalarAsync<int>(sql, user);
    }

    public async Task<bool> UsernameExist(string username)
    {
        using var connection = await _dbConnectionProvider.CreateConnection();

        return await connection.ExecuteScalarAsync<bool>("""
                                                         SELECT EXISTS (SELECT * FROM users WHERE username = @username)
                                                         """, new { username });
    }
}