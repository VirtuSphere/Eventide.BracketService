using Eventide.BracketService.Domain.Entities;
using Eventide.BracketService.Domain.Interfaces;
using Eventide.BracketService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Eventide.BracketService.Infrastructure.Repositories;

public class BracketRepository : IBracketRepository
{
    private readonly BracketDbContext _context;

    public BracketRepository(BracketDbContext context) => _context = context;

    public async Task<Bracket?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _context.Brackets.Include(b => b.Rounds).ThenInclude(r => r.Matches)
            .FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<Bracket?> GetByTournamentIdAsync(Guid tournamentId, CancellationToken ct)
        => await _context.Brackets.Include(b => b.Rounds).ThenInclude(r => r.Matches)
            .FirstOrDefaultAsync(b => b.TournamentId == tournamentId, ct);

    public async Task AddAsync(Bracket bracket, CancellationToken ct)
        => await _context.Brackets.AddAsync(bracket, ct);

    public Task UpdateAsync(Bracket bracket, CancellationToken ct)
    { _context.Brackets.Update(bracket); return Task.CompletedTask; }

    public async Task SaveChangesAsync(CancellationToken ct) => await _context.SaveChangesAsync(ct);
}