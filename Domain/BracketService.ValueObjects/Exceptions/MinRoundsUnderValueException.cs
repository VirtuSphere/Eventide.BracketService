namespace BracketService.ValueObjects.Exceptions;

public class MinRoundsUnderValueException(int value, int minValue)
    : ArgumentOutOfRangeException(nameof(value), value, $"Rounds value \"{value}\" is less than the minimum allowed value \"{minValue}\"");
