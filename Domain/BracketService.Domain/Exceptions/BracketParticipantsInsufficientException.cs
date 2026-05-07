namespace BracketService.Domain.Exceptions;

public class BracketParticipantsInsufficientException(int participantCount, int minRequired)
    : ArgumentException($"Tournament has {participantCount} participants but minimum {minRequired} required for bracket generation", nameof(participantCount));
