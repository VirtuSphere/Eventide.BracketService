namespace Eventide.BracketService.Contracts.Events;

public class BracketGeneratedEvent
{
    public Guid BracketId { get; init; }
    public Guid TournamentId { get; init; }
    public List<MatchInfo> Matches { get; init; } = new();
}

public class MatchInfo
{
    public Guid Player1Id { get; init; }
    public Guid Player2Id { get; init; }
    public int RoundNumber { get; init; }
}