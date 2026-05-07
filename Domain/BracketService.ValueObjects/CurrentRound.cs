using BracketService.ValueObjects.Base;
using BracketService.ValueObjects.Validators;
namespace BracketService.ValueObjects;

public class CurrentRound : ValueObject<int>
{
    public CurrentRound(int value) : base(new CurrentRoundValidator(), value) { }
}
