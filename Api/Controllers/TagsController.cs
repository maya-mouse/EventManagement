using Application.Events.Commands;
using Application.Events.DTOs;
using Application.Events.Queries;
using Application.Tags.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TagsController(IMediator mediator) : ControllerBaseApi
{ 
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<TagDto>>> GetAllTags()
    {
        var result = await _mediator.Send(new GetAllTagsQuery());
        return Ok(result);
    }
}
