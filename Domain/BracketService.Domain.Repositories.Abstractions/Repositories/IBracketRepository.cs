using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BracketService.Domain;
using BracketService.Domain.Base;
using BracketService.Domain.Repositories.Abstractions.Base;

namespace BracketService.Domain.Repositories.Abstractions.Repositories;

/// <summary>
/// Repository interface for Bracket aggregate root.
/// Provides methods to manage bracket persistence and retrieval.
/// </summary>
public interface IBracketRepository : IRepository<Bracket, Guid>
{
    /// <summary>
    /// Retrieves a bracket by tournament identifier.
    /// </summary>
    /// <param name="tournamentId">The unique identifier of the tournament.</param>
    /// <param name="ct">Cancellation token for async operation.</param>
    /// <returns>The bracket associated with the tournament, or null if not found.</returns>
    Task<Bracket?> GetByTournamentIdAsync(Guid tournamentId, CancellationToken ct = default);

    /// <summary>
    /// Retrieves all brackets for a specific tournament status.
    /// </summary>
    /// <param name="status">The bracket status to filter by.</param>
    /// <param name="ct">Cancellation token for async operation.</param>
    /// <returns>A read-only collection of brackets with the specified status.</returns>
    Task<IReadOnlyCollection<Bracket>> GetByStatusAsync(Enums.BracketStatus status, CancellationToken ct = default);

    /// <summary>
    /// Persists all pending changes to the data store.
    /// Used as part of the Unit of Work pattern.
    /// </summary>
    /// <param name="ct">Cancellation token for async operation.</param>
    Task SaveChangesAsync(CancellationToken ct = default);
}