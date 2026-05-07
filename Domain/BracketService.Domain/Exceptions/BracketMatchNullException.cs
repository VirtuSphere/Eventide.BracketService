namespace BracketService.Domain.Exceptions;

public class BracketMatchNullException()
    : ArgumentNullException(nameof(BracketMatch), $"BracketMatch cannot be null");
