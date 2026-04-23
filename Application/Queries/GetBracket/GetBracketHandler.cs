using Eventide.BracketService.Application.Common;
using Eventide.BracketService.Application.DTOs;
using Eventide.BracketService.Domain.Interfaces;
using MediatR;

namespace Eventide.BracketService.Application.Queries.GetBracket;

public class GetBracketHandler : IRequestHandler<GetBracketQuery, Result<BracketDto>>
{
    private readonly IBracketRepository _repo;

    public GetBracketHandler(IBracketRepository repo) => _repo = repo;

    public async Task<Result<BracketDto>> Handle(GetBracketQuery req, CancellationToken ct)
    {
        var bracket = await _repo.GetByTournamentIdAsync(req.TournamentId, ct);
        if (bracket is null) return Result<BracketDto>.Failure("Bracket not found");

        var dto = new BracketDto
        {
            Id = bracket.Id,
            TournamentId = bracket.TournamentId,
            Type = bracket.Type.ToString(),
            CurrentRound = bracket.CurrentRound,
            TotalRounds = bracket.TotalRounds,
            Status = bracket.Status.ToString(),
            Rounds = bracket.Rounds.Select(r => new RoundDto
            {
                RoundNumber = r.RoundNumber,
                Matches = r.Matches.Select(m => new MatchDto
                {
                    Id = m.Id,
                    Player1Id = m.Player1Id,
                    Player2Id = m.Player2Id,
                    WinnerId = m.WinnerId,
                    Status = m.Status.ToString()
                }).ToList()
            }).ToList()
        };

        return Result<BracketDto>.Success(dto);
    }
}