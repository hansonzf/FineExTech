using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Domain.AggregatesModel.CountingCargoPaperAggregate;
using Warehouse.Domain.AggregatesModel.StockInPaperAggregate;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;
using Warehouse.Domain.Services;

namespace Warehouse.Infrastructure.Services
{
    public class CountingCargoService : ICountingCargoService
    {
        private readonly ICountingRepository _repository;
        private readonly ILogger<CountingCargoService> _logger;

        public CountingCargoService(ICountingRepository repository, ILogger<CountingCargoService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<StockArea>> CountingCargo(int tenantId, string stockinPaperNum, string countingPaperNum, List<CountingCargoResult> countingResult)
        {
            var stockinPaper = await _repository.GetStockInPaper(tenantId, stockinPaperNum);
            var countingPaper = await _repository.GetCountingCargoPaper(tenantId, countingPaperNum);

            if (stockinPaper is null || countingPaper is null)
            {
                // 
                _logger.LogError("");
            }

            if (stockinPaper.CargoOwnerId != countingPaper.CargoOwnerId)
            {
                //
            }

            if (stockinPaper.StockhouseId != countingPaper.StorehouseId)
            {
                //
            }

            countingResult.ForEach(res => {
                countingPaper.CheckFact(res);
            });
        }
    }
}
