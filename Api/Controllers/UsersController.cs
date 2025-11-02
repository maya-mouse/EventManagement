using Application.Users.Commands.Login;
using Application.Users.Commands.Register;
using Application.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UsersController(IMediator mediator) : ControllerBaseApi
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("login")] 
    public async Task<ActionResult<AuthResponseDto>> Login(LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}