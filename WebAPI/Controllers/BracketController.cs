using Eventide.BracketService.Application.Commands.GenerateBracket;
using Eventide.BracketService.Application.Queries.GetBracket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Eventide.BracketService.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BracketController : ControllerBase
{
    private readonly IMediator _mediator;

    public BracketController(IMediator mediator) => _mediator = mediator;

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GenerateBracketCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }

    [HttpGet("tournament/{tournamentId}")]
    public async Task<IActionResult> GetByTournament(Guid tournamentId)
    {
        var result = await _mediator.Send(new GetBracketQuery { TournamentId = tournamentId });
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.ErrorMessage);
    }
}