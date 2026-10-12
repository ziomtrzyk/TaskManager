using Microsoft.AspNetCore.Mvc;
using TaskManager.Dtos;
using TaskManager.Services;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var iscreated = await service.RegisterAsync(registerDto);
        if(!iscreated)
            return Conflict("Nazwa użytkownika jest zajęta");
        return Created();
    }
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        var login = await service.LoginAsync(loginDto);
        if(login is null)
            return Unauthorized("Nieprawidłowa nazwa użytkownika lub hasło");
        return login;
    }
}