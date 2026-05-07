namespace BracketService.Domain.Exceptions;

public class BracketRoundNullException()
    : ArgumentNullException(nameof(BracketRound), $"BracketRound cannot be null");
