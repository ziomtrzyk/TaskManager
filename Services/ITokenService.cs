using TaskManager.Models;

namespace TaskManager.Services;

public interface ITokenService
{
    string Generate(User user);
}