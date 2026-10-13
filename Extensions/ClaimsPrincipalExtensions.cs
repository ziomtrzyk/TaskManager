using System.Security.Claims;

namespace TaskManager.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Brak claima NameIdentifier");
        return int.Parse(id);
    }
}