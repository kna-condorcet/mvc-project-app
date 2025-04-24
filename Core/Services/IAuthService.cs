using Condorcet.B2.AspnetCore.MVC.Application.Models;

namespace Condorcet.B2.AspnetCore.MVC.Application.Core.Services;

public interface IAuthService
{
    Task<bool> RegisterUserAsync(RegisterUserViewModel model);
}