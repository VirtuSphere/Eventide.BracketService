using Eventide.BracketService.Application.Commands.GenerateBracket;
using Eventide.BracketService.Domain.Enums;
using Eventide.TournamentService.Contracts.Events;
using MassTransit;
using MediatR;

namespace Eventide.BracketService.Application.EventHandlers;

public class RegistrationClosedConsumer : IConsumer<RegistrationClosedEvent>
{
    private readonly IMediator _mediator;

    public RegistrationClosedConsumer(IMediator mediator) => _mediator = mediator;

    public async Task Consume(ConsumeContext<RegistrationClosedEvent> context)
    {
        var msg = context.Message;
        
        await _mediator.Send(new GenerateBracketCommand
        {
            TournamentId = msg.TournamentId,
            Type = BracketType.SingleElimination,
            ParticipantIds = msg.ParticipantIds
        });
    }
}