using MediatR;
using Newtonsoft.Json;
using Orderpool.Api.Models.OrderWatcherAggregate;

namespace Orderpool.Api.OrderProcessPipeline
{
    public class CheckInventoryHandler : IPipelineBehavior<ProcessParameter, OrderWatcher>
    {
        private readonly IOrderWatcherRepository _repository;

        public CheckInventoryHandler(IOrderWatcherRepository repository)
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
            List<CheckInventoryResultStruture> checkInventoryResultStrutureList = new List<CheckInventoryResultStruture>
            {
                new CheckInventoryResultStruture { Subject = "CheckInventory", SKU = "xxx-xxx-xx", WarehouseId = 10001, StockCount = 67 },
                new CheckInventoryResultStruture { Subject = "CheckInventory", SKU = "xxx-llx-ss", WarehouseId = 10002, StockCount = 23 },
                new CheckInventoryResultStruture { Subject = "CheckInventory", SKU = "xmx-xmm-xm", WarehouseId = 10001, StockCount = 90 },
            };
            string json = JsonConvert.SerializeObject(checkInventoryResultStrutureList);
            watcher.AddProcessResult(json);
            await _repository.SaveProcessAsync(watcher);

            return await next();
        }
    }

    public record CheckInventoryResultStruture
    {
        public string Subject { get; init; }
        public int WarehouseId { get; init; }
        public string SKU { get; init; }
        public int StockCount { get; init; }
    }
}
