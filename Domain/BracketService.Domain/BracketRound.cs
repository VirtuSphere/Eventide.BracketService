using System;
using System.Collections.Generic;
using BracketService.Domain.Exceptions;
using BracketService.ValueObjects;
using BracketService.Domain.Enums;
using BracketService.Domain.Base;

namespace BracketService.Domain;

public class BracketRound : Entity<Guid>
{
    public int RoundNumber { get; private set; }
    private readonly List<BracketMatch> _matches = new();
    public IReadOnlyCollection<BracketMatch> Matches => _matches.AsReadOnly();
    public bool IsCompleted => _matches.All(m => m.Status == MatchStatus.Completed);

    private BracketRound() { }

    public BracketRound(Guid id, int roundNumber) : base(id)
    {
        RoundNumber = roundNumber > 0 ? roundNumber : throw new ArgumentOutOfRangeException(nameof(roundNumber));
    }

    public void AddMatch(BracketMatch match)
    {
        if (match == null) throw new ArgumentNullValueException(nameof(match));
        _matches.Add(match);
    }

    public bool RemoveMatch(BracketMatch match)
    {
        if (match == null) throw new ArgumentNullValueException(nameof(match));
        return _matches.Remove(match);
    }
}
