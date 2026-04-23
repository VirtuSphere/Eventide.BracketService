using Eventide.BracketService.Domain.Entities;

namespace Eventide.BracketService.Domain.Interfaces;

public interface IBracketRepository
{
    Task<Bracket?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Bracket?> GetByTournamentIdAsync(Guid tournamentId, CancellationToken ct = default);
    Task AddAsync(Bracket bracket, CancellationToken ct = default);
    Task UpdateAsync(Bracket bracket, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}