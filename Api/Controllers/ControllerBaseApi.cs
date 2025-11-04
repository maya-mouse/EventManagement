using System.Security.Claims;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ControllerBaseApi : ControllerBase
{
    protected int GetUserId()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idClaim == null || !int.TryParse(idClaim, out int userId))
        {
            throw new UnauthorizedAccessException("Invalid user identifier in token");
        }
        return userId;
    }
}
