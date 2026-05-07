namespace BracketService.Domain.Exceptions;

public class BracketNullException()
    : ArgumentNullException(nameof(Bracket), $"Bracket cannot be null");
