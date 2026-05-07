using BracketService.Domain.Exceptions;
using BracketService.ValueObjects;
using BracketService.Domain.Enums;
using BracketService.Domain.Base;

namespace BracketService.Domain;

public class BracketMatch : Entity<Guid>
{
    public Guid Player1Id { get; private set; }
    public Guid Player2Id { get; private set; }
    public Guid? WinnerId { get; private set; }
    public int RoundNumber { get; private set; }
    public MatchStatus Status { get; private set; }

    private BracketMatch() { }

    public BracketMatch(Guid id, Guid player1Id, Guid player2Id, int roundNumber) : base(id)
    {
        Player1Id = player1Id != Guid.Empty ? player1Id : throw new ArgumentNullValueException(nameof(player1Id));
        Player2Id = player2Id != Guid.Empty ? player2Id : throw new ArgumentNullValueException(nameof(player2Id));
        RoundNumber = roundNumber > 0 ? roundNumber : throw new ArgumentOutOfRangeException(nameof(roundNumber));
        Status = MatchStatus.Pending;
        WinnerId = null;
    }

    public static BracketMatch Create(Guid player1Id, Guid player2Id, int roundNumber)
    {
        return new BracketMatch(Guid.NewGuid(), player1Id, player2Id, roundNumber);
    }

    public void SetWinner(Guid winnerId)
    {
        if (winnerId != Player1Id && winnerId != Player2Id)
            throw new MatchInvalidWinnerException(winnerId, Player1Id, Player2Id);
        
        if (Status == MatchStatus.Completed)
            throw new MatchAlreadyCompletedException(Id);
        
        WinnerId = winnerId;
        Status = MatchStatus.Completed;
    }
}
