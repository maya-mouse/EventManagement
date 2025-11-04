using Application.Events.Commands;
using Application.Events.DTOs;
using Application.Events.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class EventsController(IMediator mediator) : ControllerBaseApi
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<EventDto>>> GetPublicEvents()
    {
    
    var userId = GetUserId();
    var query = new GetPublicEventsQuery(UserId: userId);
    
    var result = await _mediator.Send(query);
    return Ok(result);
    }

    [HttpGet("{id}")]
     [Authorize]
    public async Task<ActionResult<List<EventDetailDto>>> GetSingleEvent(int id)
    {
        var userId = GetUserId();

        var result = await _mediator.Send(new GetSingleEventQuery(id, userId));
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent(CreateEventDto dto)
    {
        var hostId = GetUserId();
        var command = new CreateEventCommand(dto, hostId);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<ActionResult<EventDto>> UpdateEvent([FromRoute] int id, [FromBody] CreateEventDto updEventDto)
    {
        
        var userId = GetUserId();

        var command = new UpdateEventCommand(
            EventId: id,
            OrganizerId: userId,
            updateEventDto: updEventDto
        );

        await _mediator.Send(command);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEvent(int id)
    {
        var result = await _mediator.Send(new DeleteEventCommand(id));
        return Ok(result);
    }
    
    [Authorize]
    [HttpPost("{id}/join")]
    public async Task<ActionResult> JoinEvent([FromRoute] int id)
    {
       var userId = GetUserId(); 
       var result = await _mediator.Send(new JoinEventCommand(EventId: id, UserId: userId));
        
        return Ok(result);
    }

    [Authorize]
    [HttpPost("{id}/leave")]
    public async Task<ActionResult> LeaveEvent([FromRoute] int id)
    {
        var userId = GetUserId();
        var result = await _mediator.Send(new LeaveEventCommand(eventId: id, userId: userId));

        return Ok(result);
    }

    [Authorize] 
    [HttpGet("me/events")]
    public async Task<ActionResult<List<CalendarEventDto>>> GetUserCalendarEvents()
    {
        var userId = GetUserId();

        var query = new GetUserEventsQuery(UserId: userId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}