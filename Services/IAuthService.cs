using TaskManager.Dtos;

namespace TaskManager.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
}