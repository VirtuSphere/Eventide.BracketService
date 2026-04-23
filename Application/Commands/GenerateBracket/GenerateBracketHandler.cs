using Eventide.BracketService.Application.Common;
using Eventide.BracketService.Domain.Entities;
using Eventide.BracketService.Domain.Interfaces;
using MediatR;

namespace Eventide.BracketService.Application.Commands.GenerateBracket;

public class GenerateBracketHandler : IRequestHandler<GenerateBracketCommand, Result<Guid>>
{
    private readonly IBracketRepository _repo;

    public GenerateBracketHandler(IBracketRepository repo) => _repo = repo;

    public async Task<Result<Guid>> Handle(GenerateBracketCommand req, CancellationToken ct)
    {
        var bracket = Bracket.Generate(req.TournamentId, req.Type, req.ParticipantIds);
        await _repo.AddAsync(bracket, ct);
        await _repo.SaveChangesAsync(ct);

        return Result<Guid>.Success(bracket.Id);
    }
}