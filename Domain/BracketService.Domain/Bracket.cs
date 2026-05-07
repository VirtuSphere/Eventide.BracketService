using BracketService.Domain.Exceptions;
using BracketService.ValueObjects;
using BracketService.Domain.Enums;
using BracketService.Domain.Base;

namespace BracketService.Domain;

public class Bracket : Entity<Guid>
{
    public Guid TournamentId { get; private set; }
    public BracketType Type { get; private set; }
    public TotalRounds TotalRounds { get; private set; }
    public CurrentRound CurrentRound { get; private set; }
    public BracketStatus Status { get; private set; }
    private readonly List<BracketRound> _rounds = new();
    public IReadOnlyCollection<BracketRound> Rounds => _rounds.AsReadOnly();
    public DateTime CreatedAt { get; private set; }

    private Bracket() { }

    public Bracket(Guid id, Guid tournamentId, BracketType type, TotalRounds totalRounds, CurrentRound currentRound, BracketStatus status, DateTime createdAt) : base(id)
    {
        TournamentId = tournamentId != Guid.Empty ? tournamentId : throw new ArgumentNullValueException(nameof(tournamentId));
        Type = type;
        TotalRounds = totalRounds ?? throw new ArgumentNullValueException(nameof(totalRounds));
        CurrentRound = currentRound ?? throw new ArgumentNullValueException(nameof(currentRound));
        Status = status;
        CreatedAt = createdAt;
    }

    public static Bracket Generate(Guid tournamentId, BracketType type, List<Guid> participantIds)
    {
        if (participantIds.Count < 2) 
            throw new BracketParticipantsInsufficientException(participantIds.Count, 2);

        var shuffled = participantIds.OrderBy(_ => Guid.NewGuid()).ToList();
        var matchCount = shuffled.Count / 2;
        var totalRoundsValue = (int)Math.Ceiling(Math.Log2(shuffled.Count));

        var bracket = new Bracket(
            Guid.NewGuid(),
            tournamentId,
            type,
            new TotalRounds(totalRoundsValue),
            new CurrentRound(1),
            BracketStatus.Generated,
            DateTime.UtcNow
        );

        var firstRound = new BracketRound(Guid.NewGuid(), 1);

        for (int i = 0; i < matchCount; i++)
        {
            var match = new BracketMatch(Guid.NewGuid(), shuffled[i * 2], shuffled[i * 2 + 1], 1);
            firstRound.AddMatch(match);
        }

        bracket._rounds.Add(firstRound);
        return bracket;
    }

    public void AdvanceWinner(Guid matchId, Guid winnerId)
    {
        if (Status == BracketStatus.Completed)
            throw new BracketCompletedException(Id);

        // Найти матч в текущем раунде
        var currentRound = _rounds.FirstOrDefault(r => r.RoundNumber == CurrentRound.Value);
        if (currentRound == null)
            throw new DomainException($"Round {CurrentRound.Value} not found in bracket");

        var match = currentRound.Matches.FirstOrDefault(m => m.Id == matchId);
        if (match == null)
            throw new DomainException($"Match {matchId} not found in round {CurrentRound.Value}");

        if (match.Status == MatchStatus.Completed)
            throw new MatchAlreadyCompletedException(matchId);

        // Установить победителя и обновить статус матча
        match.SetWinner(winnerId);

        // Проверить, завершились ли все матчи текущего раунда
        if (currentRound.IsCompleted)
        {
            // Если это не последний раунд, создать следующий
            if (CurrentRound.Value < TotalRounds.Value)
            {
                AdvanceToNextRound(currentRound);
            }
            else
            {
                // Это был финальный раунд
                Status = BracketStatus.Completed;
            }
        }
        else
        {
            Status = BracketStatus.InProgress;
        }
    }

    private void AdvanceToNextRound(BracketRound completedRound)
    {
        // Получить всех победителей из текущего раунда
        var winners = completedRound.Matches
            .Where(m => m.Status == MatchStatus.Completed && m.WinnerId.HasValue)
            .Select(m => m.WinnerId.Value)
            .ToList();

        if (winners.Count % 2 != 0)
            throw new InvalidRoundAdvancementException(winners.Count);

        // Создать новый раунд
        var nextRoundNumber = CurrentRound.Value + 1;
        var nextRound = new BracketRound(Guid.NewGuid(), nextRoundNumber);

        // Создать матчи для следующего раунда
        for (int i = 0; i < winners.Count; i += 2)
        {
            var nextMatch = new BracketMatch(Guid.NewGuid(), winners[i], winners[i + 1], nextRoundNumber);
            nextRound.AddMatch(nextMatch);
        }

        _rounds.Add(nextRound);
        CurrentRound = new CurrentRound(nextRoundNumber);
    }

    public void Complete() => Status = BracketStatus.Completed;

    public void AddRound(BracketRound round)
    {
        if (round == null) throw new ArgumentNullValueException(nameof(round));
        _rounds.Add(round);
    }
}