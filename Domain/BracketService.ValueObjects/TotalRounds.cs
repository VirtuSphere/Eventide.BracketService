using BracketService.ValueObjects.Base;
using BracketService.ValueObjects.Validators;
namespace BracketService.ValueObjects;

public class TotalRounds : ValueObject<int>
{
    public TotalRounds(int value) : base(new TotalRoundsValidator(), value) { }
}