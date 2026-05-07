namespace BracketService.Domain.Exceptions;

public class MatchInvalidWinnerException(Guid winnerId, Guid player1Id, Guid player2Id)
    : ArgumentException($"Winner \"{winnerId}\" is not a participant in match between \"{player1Id}\" and \"{player2Id}\"", nameof(winnerId));
