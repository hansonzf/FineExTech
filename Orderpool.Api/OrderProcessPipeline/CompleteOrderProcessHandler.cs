using MediatR;
using Orderpool.Api.Models.OrderWatcherAggregate;
using System.Net.NetworkInformation;

namespace Orderpool.Api.OrderProcessPipeline
{
    public class CompleteOrderProcessHandler : IRequestHandler<ProcessParameter, OrderWatcher>
    {
        private readonly IOrderWatcherRepository _repository;

        public CompleteOrderProcessHandler(IOrderWatcherRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderWatcher> Handle(ProcessParameter request, CancellationToken cancellationToken)
        {
            var watcher = await _repository.GetByIdAsync(request.OrderWatcherId);
            if (watcher is null)
                return null;

            watcher.AddProcessResult("Complete all process steps");
            await _repository.SaveProcessAsync(watcher);
            watcher.EndProcess();

            return watcher;
        }
    }
}
