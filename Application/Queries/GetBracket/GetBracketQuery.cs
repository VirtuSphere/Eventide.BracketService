using Eventide.BracketService.Application.Common;
using Eventide.BracketService.Application.DTOs;
using MediatR;

namespace Eventide.BracketService.Application.Queries.GetBracket;

public class GetBracketQuery : IRequest<Result<BracketDto>>
{
    public Guid TournamentId { get; init; }
}