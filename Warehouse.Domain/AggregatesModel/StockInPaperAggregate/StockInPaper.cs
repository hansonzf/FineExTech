using DomainBase;
using Warehouse.Domain.AggregatesModel.CountingCargoPaperAggregate;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;
using Warehouse.Domain.Events;

namespace Warehouse.Domain.AggregatesModel.StockInPaperAggregate
{
    public enum StockInStatus
    {
        Standby,
        Processing,
        Received,
        PartiallyReceived
    }

    public class StockInPaper : Entity, IAggregateRoot
    {
        protected StockInPaper()
        {
            StockInDetails = new List<StockInDetail>();
        }

        public int TenantId { get; private set; }
        public string SerialNumber { get; private set; }
        public long StockhouseId { get; private set; }
        public long CargoOwnerId { get; private set; }
        public StockInStatus Status { get; private set; }
        public IList<StockInDetail> StockInDetails { get; private set; }

        public static StockInPaper CreateInstance(int tenantId, long stockhouseId, long cargoOwnerId, List<KeyValuePair<string, int>> cargoInfo)
        {
            var paper = new StockInPaper();
            paper.SerialNumber = $"STIN{DateTime.Now.ToString("yyyyMMddhhmmssf")}";
            paper.TenantId = tenantId;
            paper.StockhouseId = stockhouseId;
            paper.CargoOwnerId = cargoOwnerId;
            paper.Status = StockInStatus.Standby;

            paper.AddDomainEvent(new StockInPaperArrivedDomainEvent(paper.TenantId, paper.StockhouseId, paper.SerialNumber));
            return paper;
        }

        
    }

}
