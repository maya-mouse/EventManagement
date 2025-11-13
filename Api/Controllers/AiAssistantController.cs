using Application.AiAssistant.DTOs;
using Application.AiAssistant.Queries;
using Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
   
[Authorize]
public class AiAssistantController(IMediator mediator, IAiAssistantService aiAssistantService) : ControllerBaseApi
{
    private readonly IMediator _mediator = mediator;
    private readonly IAiAssistantService _aiAssistantService = aiAssistantService;


   [HttpPost]
    public async Task<ActionResult<string>> Ask([FromBody] AskQueryDto dto, CancellationToken cancellationToken)
    {
            var userId = GetUserId(); 
            if (userId == 0) return Unauthorized("User ID claim is missing or invalid.");
            
            var contextQuery = new GetUserEventsContextQuery(userId);
            var contextJson = await _mediator.Send(contextQuery, cancellationToken);
            
            var answer = await _aiAssistantService.GetAnswerAsync(dto.Question, contextJson, cancellationToken);
            
            return Ok(answer);
    }
}