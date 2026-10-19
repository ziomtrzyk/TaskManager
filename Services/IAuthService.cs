using TaskManager.Dtos;

namespace TaskManager.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
}