namespace BracketService.ValueObjects.Exceptions;

public class MaxRoundsOverValueException(int value, int maxValue)
    : ArgumentOutOfRangeException(nameof(value), value, $"Rounds value \"{value}\" exceeds the maximum allowed value \"{maxValue}\"");
