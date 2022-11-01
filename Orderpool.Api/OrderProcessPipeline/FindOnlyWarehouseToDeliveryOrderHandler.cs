using MediatR;
using Newtonsoft.Json;
using Orderpool.Api.Models.OrderWatcherAggregate;

namespace Orderpool.Api.OrderProcessPipeline
{
    public class FindOnlyWarehouseToDeliveryOrderHandler : IPipelineBehavior<ProcessParameter, OrderWatcher>
    {
        private readonly IOrderWatcherRepository _repository;

        public FindOnlyWarehouseToDeliveryOrderHandler(IOrderWatcherRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderWatcher> Handle(ProcessParameter request, RequestHandlerDelegate<OrderWatcher> next, CancellationToken cancellationToken)
        {
            var watcher = await _repository.GetByIdAsync(request.OrderWatcherId);
            if (watcher is null)
                return null;

            // Here will to call remote inventory service to get order items inventory
            // for demo purpose, just create result
            var result = new ProcessingResult { Subject = "FindOnlyWarehouse", WarehosueId = 10002 };
            var json = JsonConvert.SerializeObject(result);

            watcher.AddProcessResult(json);

            await _repository.SaveProcessAsync(watcher);

            return await next();
        }
    }

    public record ProcessingResult
    {
        public string Subject { get; init; }
        public int WarehosueId { get; init; }
    }
}
