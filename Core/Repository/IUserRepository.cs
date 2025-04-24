using Condorcet.B2.AspnetCore.MVC.Application.Core.Domain;

namespace Condorcet.B2.AspnetCore.MVC.Application.Core.Repository;

public interface IUserRepository
{
    Task<int> CreateUserAsync(User user);
    Task<bool> UsernameExist(string username);
    Task<User?> GetByUsernameAsync(string username);
}