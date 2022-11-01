using DomainBase;
using System.Collections.ObjectModel;
using Warehouse.Domain.AggregatesModel.StockInPaperAggregate;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;

namespace Warehouse.Domain.AggregatesModel.CountingCargoPaperAggregate
{
    public class CountingCargoPaper : Entity, IAggregateRoot
    {
        protected List<CargoDetail> _cargoList;
        protected CountingCargoPaper()
        { 
            _cargoList = new List<CargoDetail>();
        }

        public string SerialNumber { get; protected set; }
        public int TenantId { get; protected set; }
        public long StorehouseId { get; protected set; }
        public long CargoOwnerId { get; protected set; }
        public DateTime CountingTime { get; protected set; }
        public Evidence Evidence { get; protected set; }
        public ReadOnlyCollection<CargoDetail> CargoList => _cargoList.AsReadOnly();

        public static CountingCargoPaper CopyFromStockinPaper(StockInPaper stockinPaper)
        {
            var countingPaper = new CountingCargoPaper
            {
                SerialNumber = $"CNTC{DateTime.Now.ToString("yyyyMMddhhmmssf")}",
                TenantId = stockinPaper.TenantId,
                StorehouseId = stockinPaper.StockhouseId,
                CargoOwnerId = stockinPaper.CargoOwnerId,
                CountingTime = DateTime.Now,
                _cargoList = new List<CargoDetail>()
            };

            int index = 1;
            foreach (var item in stockinPaper.StockInDetails)
            {
                var cargoDetail = new CargoDetail(countingPaper.SerialNumber, index++, item.SKU, item.Count, item.EnvironmentRequirement);
                countingPaper._cargoList.Add(cargoDetail);
            }

            return countingPaper;
        }

        public int CheckFact(CountingCargoResult result)
        {
            if (result is null)
                return 0;

            var cargo = _cargoList.SingleOrDefault(x => x.Index == result.Index);

            if (cargo is null)
            {
                // while counting the cargo, found that have more goods deliveried, but not record on stock-in paper
                // so system should add a new record for the extra goods into counting paper
                cargo = new CargoDetail(SerialNumber, result.Index, result.SKU, 0, StockAreaType.Unknow);
            }

            var index = cargo.Verify(result);
            if (result.Proof is not null)
                Evidence.Append(result.Proof);

            return index;
        }
    }
}
