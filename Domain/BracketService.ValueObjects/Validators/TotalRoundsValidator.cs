using BracketService.ValueObjects.Base;
using BracketService.ValueObjects.Exceptions;

namespace BracketService.ValueObjects.Validators;

public class TotalRoundsValidator : IValidator<int>
{
    /// <summary>
    /// The  max length
    /// </summary>
    public static byte MAX_LENGTH => 100;

    /// <summary>
    /// The  min length
    /// </summary>
    public static byte MIN_LENGTH => 1;

    /// <summary>
    /// Validates that the specified value is within the allowed range for the number of teams.
    /// </summary>
    /// <param name="value">The number of teams to validate. Must be between the minimum and maximum allowed values, inclusive.</param>
    /// <exception cref="MaxTeamsOverValueException">Thrown if the specified value exceeds the maximum allowed number of teams.</exception>
    /// <exception cref="MaxTeamsNegativeValueException">Thrown if the specified value is less than the minimum allowed number of teams.</exception>
    public void Validate(int value)
    {
        if (value > MAX_LENGTH)
            throw new MaxRoundsOverValueException(value, MAX_LENGTH);

        if (value < MIN_LENGTH)
            throw new MinRoundsUnderValueException(value, MIN_LENGTH);
    }
}
