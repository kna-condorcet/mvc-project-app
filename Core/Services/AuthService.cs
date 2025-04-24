using Condorcet.B2.AspnetCore.MVC.Application.Core.Domain;
using Condorcet.B2.AspnetCore.MVC.Application.Core.Repository;
using Condorcet.B2.AspnetCore.MVC.Application.Models;
using Condorcet.B2.ProjectManagement.WebApplication.Core.Domain;

namespace Condorcet.B2.AspnetCore.MVC.Application.Core.Services;

public class AuthService: IAuthService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    public AuthService(IPasswordHasher passwordHasher, IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    public async Task<bool> RegisterUserAsync(RegisterUserViewModel registerDto)
    {
        var existingUser = await _userRepository.UsernameExist(registerDto.Username);
        
        if (existingUser)
            return false;
        
        var (hash, salt) = _passwordHasher.Hash(registerDto.Password);
        
        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = hash,
            Salt = salt,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            IsActive = true,
            Role = UserRoles.User
        };

        await _userRepository.CreateUserAsync(user);

        return true;
    }
}