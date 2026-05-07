namespace BracketService.Domain.Exceptions;

public class MatchAlreadyCompletedException(Guid matchId)
    : InvalidOperationException($"Match \"{matchId}\" is already completed and cannot be modified");
