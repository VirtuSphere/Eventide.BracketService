using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketService.Domain.Base;

public abstract class Entity<TId>(TId id) where TId : struct, IEquatable<TId>
{
    /// <summary>
    /// Gets the ID of the entity.
    /// </summary>
    public TId Id { get; } = id;
    /// <summary>
    /// Protected constructor for entity framework if needed.
    /// </summary>
    protected Entity() : this(default!)
    {

    }
}
