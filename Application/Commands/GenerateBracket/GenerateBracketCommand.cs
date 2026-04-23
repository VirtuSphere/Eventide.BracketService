using Eventide.BracketService.Application.Common;
using Eventide.BracketService.Domain.Enums;
using MediatR;

namespace Eventide.BracketService.Application.Commands.GenerateBracket;

public class GenerateBracketCommand : IRequest<Result<Guid>>
{
    public Guid TournamentId { get; init; }
    public BracketType Type { get; init; }
    public List<Guid> ParticipantIds { get; init; } = new();
}