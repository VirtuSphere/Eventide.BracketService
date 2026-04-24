using Eventide.BracketService.Application.Common;
using Eventide.BracketService.Contracts.Events;
using Eventide.BracketService.Domain.Entities;
using Eventide.BracketService.Domain.Interfaces;
using MassTransit;
using MediatR;

namespace Eventide.BracketService.Application.Commands.GenerateBracket;

public class GenerateBracketHandler : IRequestHandler<GenerateBracketCommand, Result<Guid>>
{
    private readonly IBracketRepository _repo;
    private readonly IPublishEndpoint _publishEndpoint;

    public GenerateBracketHandler(IBracketRepository repo, IPublishEndpoint publishEndpoint)
    {
        _repo = repo;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Result<Guid>> Handle(GenerateBracketCommand req, CancellationToken ct)
    {
        var bracket = Bracket.Generate(req.TournamentId, req.Type, req.ParticipantIds);
        await _repo.AddAsync(bracket, ct);
        await _repo.SaveChangesAsync(ct);

        var matches = bracket.Rounds
            .SelectMany(r => r.Matches)
            .Select(m => new MatchInfo
            {
                Player1Id = m.Player1Id,
                Player2Id = m.Player2Id,
                RoundNumber = m.RoundNumber
            }).ToList();

        await _publishEndpoint.Publish(new BracketGeneratedEvent
        {
            BracketId = bracket.Id,
            TournamentId = bracket.TournamentId,
            Matches = matches
        }, ct);

        return Result<Guid>.Success(bracket.Id);
    }
}