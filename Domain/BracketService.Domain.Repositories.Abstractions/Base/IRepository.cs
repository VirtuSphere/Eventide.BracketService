using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BracketService.Domain.Base;

namespace BracketService.Domain.Repositories.Abstractions.Base;

/// <summary>
/// Generic repository interface for domain entities.
/// Provides standard CRUD operations for aggregate roots and entities.
/// </summary>
/// <typeparam name="TEntity">The entity type, must inherit from Entity&lt;TId&gt;.</typeparam>
/// <typeparam name="TId">The identifier type, must be a value type.</typeparam>
public interface IRepository<TEntity, TId> where TEntity : Entity<TId> where TId : struct, IEquatable<TId>
{
    /// <summary>
    /// Retrieves all entities of the specified type.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>A read-only collection of all entities.</returns>
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>The entity if found, otherwise null.</returns>
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an entity with the specified identifier exists.
    /// </summary>
    /// <param name="id">The unique identifier to check.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>True if the entity exists, otherwise false.</returns>
    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new entity to the repository.
    /// The entity is marked for addition but not persisted until SaveAsync is called.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// Changes are tracked and persisted when SaveAsync is called.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity from the repository.
    /// The entity is marked for deletion but not removed until SaveAsync is called.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}