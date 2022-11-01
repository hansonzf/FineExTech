using MediatR;
using Orderpool.Api.Models.OrderWatcherAggregate;

namespace Orderpool.Api.OrderProcessPipeline
{
    public class ProcessParameter : IRequest<OrderWatcher>
    {
        public ProcessParameter(long orderWatcherId)
        {
            OrderWatcherId = orderWatcherId;
        }

        public long OrderWatcherId { get; private set; }

    }
}
