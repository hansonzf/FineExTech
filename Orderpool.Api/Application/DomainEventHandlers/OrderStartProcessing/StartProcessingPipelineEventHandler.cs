using MediatR;
using Orderpool.Api.Events;
using Orderpool.Api.OrderProcessPipeline;

namespace Orderpool.Api.Application.DomainEventHandlers.OrderStartProcessing
{
    public class StartProcessingPipelineEventHandler 
        : INotificationHandler<OrderStartProcessingDomainEvent>
    {
        private readonly IMediator _mediator;

        public StartProcessingPipelineEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(OrderStartProcessingDomainEvent notification, CancellationToken cancellationToken)
        {
            var watcher = notification.OrderWatcherId;
            var processParam = new ProcessParameter(watcher);
            _mediator.Send(processParam);

            return Task.CompletedTask;
        }
    }
}
