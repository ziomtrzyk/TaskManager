using Microsoft.AspNetCore.Mvc;
using TaskManager.Dtos;
using TaskManager.Services;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService service) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Register(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        var iscreated = await service.RegisterAsync(registerDto, cancellationToken);
        if(!iscreated)
            return Conflict("Username is already taken.");
        return Created();
    }
    [HttpPost("login")]
    [ProducesResponseType<AuthResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var login = await service.LoginAsync(loginDto, cancellationToken);
        if(login is null)
            return Unauthorized("Invalid username or password.");
        return login;
    }
}