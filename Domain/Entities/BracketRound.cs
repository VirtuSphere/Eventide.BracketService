using Eventide.BracketService.Domain.Enums;

namespace Eventide.BracketService.Domain.Entities;

public class BracketRound
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int RoundNumber { get; set; }
    public List<BracketMatch> Matches { get; set; } = new();
    public bool IsCompleted => Matches.All(m => m.Status == MatchStatus.Completed);
}