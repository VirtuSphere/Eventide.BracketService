using BracketService.ValueObjects.Base;
using BracketService.ValueObjects.Exceptions;

namespace BracketService.ValueObjects.Validators;

public class CurrentRoundValidator : IValidator<int>
{
    /// <summary>
    /// The max rounds
    /// </summary>
    public static byte MAX_LENGTH => 100;

    /// <summary>
    /// The min rounds
    /// </summary>
    public static byte MIN_LENGTH => 1;

    /// <summary>
    /// Validates that the specified value is within the allowed range for the current round.
    /// </summary>
    /// <param name="value">The current round to validate. Must be between the minimum and maximum allowed values, inclusive.</param>
    /// <exception cref="MaxRoundsOverValueException">Thrown if the specified value exceeds the maximum allowed rounds.</exception>
    /// <exception cref="MinRoundsUnderValueException">Thrown if the specified value is less than the minimum allowed rounds.</exception>
    public void Validate(int value)
    {
        if (value > MAX_LENGTH)
            throw new MaxRoundsOverValueException(value, MAX_LENGTH);

        if (value < MIN_LENGTH)
            throw new MinRoundsUnderValueException(value, MIN_LENGTH);
    }
}
