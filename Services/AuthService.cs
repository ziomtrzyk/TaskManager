using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Dtos;
using TaskManager.Models;

namespace TaskManager.Services;

public class AuthService(AppDbContext db, IPasswordHasher<User> passwordHasher) : IAuthService
{
    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        if (await db.Users.AnyAsync(u => u.Username == registerDto.Username))
            return false;
        var user = new User
        {
            Username = registerDto.Username,
        };
        user.PasswordHash = passwordHasher.HashPassword(user, registerDto.Password);
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
        if (user == null)
            return null;

        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
        if (result == PasswordVerificationResult.Failed)
            return null;
        return new AuthResponseDto { Token = "bajojajo" };
    }
}