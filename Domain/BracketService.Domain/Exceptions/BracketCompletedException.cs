namespace BracketService.Domain.Exceptions;

public class BracketCompletedException(Guid bracketId)
    : InvalidOperationException($"Bracket \"{bracketId}\" is already completed and cannot be modified");
