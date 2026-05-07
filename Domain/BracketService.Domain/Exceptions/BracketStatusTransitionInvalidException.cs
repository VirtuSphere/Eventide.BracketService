namespace BracketService.Domain.Exceptions;

public class BracketStatusTransitionInvalidException(string fromStatus, string toStatus)
    : InvalidOperationException($"Cannot transition bracket status from \"{fromStatus}\" to \"{toStatus}\"");
