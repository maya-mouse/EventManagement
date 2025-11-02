using System.Security.Claims;
using Application.Events.Commands;
using Application.Events.DTOs;
using Application.Events.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class EventsController(IMediator mediator) : ControllerBaseApi
{
    private readonly IMediator _mediator = mediator;
    [HttpGet]
    public async Task<ActionResult<List<EventDto>>> GetPublicEvents()
    {
        var result = await _mediator.Send(new GetPublicEventsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<List<EventDto>>> GetSingleEvent(int id)
    {
        var result = await _mediator.Send(new GetSingleEventQuery(id));
        return Ok(result);
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent(CreateEventDto dto)
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaim is null || !int.TryParse(userClaim, out int hostId))
        {
            return Unauthorized("User not found");
        }
        var command = new CreateEventCommand(dto, hostId);
       
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch]
    public async Task<ActionResult<EventDto>> UpdateEvent(CreateEventCommand updCommand)
    {
        var result = await _mediator.Send(updCommand);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteEvent(DeleteEventCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/join")]
    public async Task<ActionResult> JoinEvent(int eventId, int userId)
    {
        var result = await _mediator.Send(new JoinEventCommand(eventId, userId));
        return Ok(result);
    }

    [HttpPost("{id}/leave")]
    public async Task<ActionResult> LeaveEvent(int eventId, int userId)
    {
        var result = await _mediator.Send(new LeaveEventCommand(eventId, userId));
        return Ok(result);
    }
}