namespace BracketService.Domain.Exceptions;

public class InvalidRoundAdvancementException(int winnerCount)
    : InvalidOperationException($"Cannot advance {winnerCount} winners to next round - even number required");
