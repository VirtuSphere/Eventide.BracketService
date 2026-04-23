using Eventide.BracketService.Domain.Enums;
using Eventide.BracketService.Domain.Exceptions;

namespace Eventide.BracketService.Domain.Entities;

public class Bracket
{
    public Guid Id { get; private set; }
    public Guid TournamentId { get; private set; }
    public BracketType Type { get; private set; }
    public int TotalRounds { get; private set; }
    public int CurrentRound { get; private set; }
    public BracketStatus Status { get; private set; }
    public List<BracketRound> Rounds { get; private set; } = new();
    public DateTime CreatedAt { get; private set; }

    private Bracket() { }

    public static Bracket Generate(Guid tournamentId, BracketType type, List<Guid> participantIds)
    {
        if (participantIds.Count < 2) throw new DomainException("Need at least 2 participants");

        var shuffled = participantIds.OrderBy(_ => Guid.NewGuid()).ToList();
        var matchCount = shuffled.Count / 2;

        var bracket = new Bracket
        {
            Id = Guid.NewGuid(),
            TournamentId = tournamentId,
            Type = type,
            TotalRounds = (int)Math.Ceiling(Math.Log2(shuffled.Count)),
            CurrentRound = 1,
            Status = BracketStatus.Generated,
            CreatedAt = DateTime.UtcNow
        };

        var firstRound = new BracketRound { RoundNumber = 1, Matches = new List<BracketMatch>() };

        for (int i = 0; i < matchCount; i++)
        {
            firstRound.Matches.Add(BracketMatch.Create(shuffled[i * 2], shuffled[i * 2 + 1], 1));
        }

        bracket.Rounds.Add(firstRound);
        return bracket;
    }

    public void AdvanceWinner(Guid matchId, Guid winnerId)
    {
        Status = BracketStatus.InProgress;
        // Логика продвижения в следующий раунд
    }

    public void Complete() => Status = BracketStatus.Completed;
}