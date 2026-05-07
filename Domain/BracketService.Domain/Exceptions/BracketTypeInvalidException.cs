namespace BracketService.Domain.Exceptions;

public class BracketTypeInvalidException(string bracketType)
    : ArgumentException($"Bracket type \"{bracketType}\" is invalid", nameof(bracketType));
