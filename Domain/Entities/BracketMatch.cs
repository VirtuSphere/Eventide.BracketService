using Eventide.BracketService.Domain.Enums;
using Eventide.BracketService.Domain.Exceptions;

namespace Eventide.BracketService.Domain.Entities;

public class BracketMatch
{
    public Guid Id { get; private set; }
    public Guid Player1Id { get; private set; }
    public Guid Player2Id { get; private set; }
    public Guid? WinnerId { get; private set; }
    public int RoundNumber { get; private set; }
    public MatchStatus Status { get; private set; }

    private BracketMatch() { }

    public static BracketMatch Create(Guid player1Id, Guid player2Id, int roundNumber)
    {
        return new BracketMatch
        {
            Id = Guid.NewGuid(),
            Player1Id = player1Id,
            Player2Id = player2Id,
            RoundNumber = roundNumber,
            Status = MatchStatus.Pending
        };
    }

    public void SetWinner(Guid winnerId)
    {
        if (winnerId != Player1Id && winnerId != Player2Id)
            throw new DomainException("Winner must be a match participant");
        WinnerId = winnerId;
        Status = MatchStatus.Completed;
    }
}