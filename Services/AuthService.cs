using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Dtos;
using TaskManager.Models;

namespace TaskManager.Services;

public class AuthService(AppDbContext db, IPasswordHasher<User> passwordHasher, ITokenService tokenService) : IAuthService
{
    public async Task<bool> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        if (await db.Users.AnyAsync(u => u.Username == registerDto.Username, cancellationToken))
            return false;
        var user = new User
        {
            Username = registerDto.Username,
        };
        user.PasswordHash = passwordHasher.HashPassword(user, registerDto.Password);
        db.Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username, cancellationToken);
        if (user == null)
            return null;

        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
        if (result == PasswordVerificationResult.Failed)
            return null;
        return new AuthResponseDto { Token = tokenService.Generate(user)};
    }
}