using MediatR;
using Warehouse.Domain.AggregatesModel.CountingCargoPaperAggregate;
using Warehouse.Domain.AggregatesModel.StockInPaperAggregate;
using Warehouse.Domain.Events;

namespace Warehouse.Api.Application.DomainEventHandlers.ReadyToReceiveCargo
{
    public class GenerateCountingCargoPaperHandler
        : INotificationHandler<ReadyToReceiveCargoDomainEvent>
    {
        private readonly IStockInPaperRepository _stockinPaperRepository;
        private readonly ICountingCargoPaperRepository _countingCargoPaperRepository;
        private readonly ILogger<GenerateCountingCargoPaperHandler> _logger;

        public GenerateCountingCargoPaperHandler(
            IStockInPaperRepository repository, 
            ICountingCargoPaperRepository countingCargoPaperRepository, 
            ILogger<GenerateCountingCargoPaperHandler> logger)
        {
            _stockinPaperRepository = repository;
            _logger = logger;
            _countingCargoPaperRepository = countingCargoPaperRepository;
        }

        public async Task Handle(ReadyToReceiveCargoDomainEvent notification, CancellationToken cancellationToken)
        {
            int tenantId = notification.TenantId;
            string paperNumber = notification.PaperNumber;

            if (string.IsNullOrEmpty(paperNumber))
                _logger.LogError("Warehouse receive cargo missing stock-in paper number");

            var paper = await _stockinPaperRepository.GetStockInPaperAsync(paperNumber, tenantId);

            if (paper is null)
            {
                _logger.LogError($"Warehouse receive cargo did not find document which number {paperNumber}");
            }

            var countingPaper = CountingCargoPaper.CopyFromStockinPaper(paper);
            _ = await _countingCargoPaperRepository.SaveCountingCargoPaper(countingPaper);
        }
    }
}
