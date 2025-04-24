namespace Condorcet.B2.AspnetCore.MVC.Application.Core.Services;

public interface IPasswordHasher
{
    public (string hash, string salt) Hash(string password);
    public bool VerifyPassword(string password, string hash, string salt);
}