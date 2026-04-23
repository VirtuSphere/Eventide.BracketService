namespace Eventide.BracketService.Application.DTOs;

public class BracketDto
{
    public Guid Id { get; init; }
    public Guid TournamentId { get; init; }
    public string Type { get; init; } = string.Empty;
    public int CurrentRound { get; init; }
    public int TotalRounds { get; init; }
    public string Status { get; init; } = string.Empty;
    public List<RoundDto> Rounds { get; init; } = new();
}

public class RoundDto
{
    public int RoundNumber { get; init; }
    public List<MatchDto> Matches { get; init; } = new();
}

public class MatchDto
{
    public Guid Id { get; init; }
    public Guid Player1Id { get; init; }
    public Guid Player2Id { get; init; }
    public Guid? WinnerId { get; init; }
    public string Status { get; init; } = string.Empty;
}