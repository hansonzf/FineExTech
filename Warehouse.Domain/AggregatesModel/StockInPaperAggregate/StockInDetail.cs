using DomainBase;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;

namespace Warehouse.Domain.AggregatesModel.StockInPaperAggregate
{
    public class StockInDetail : Entity
    {
        protected StockInDetail()
        { }

        internal StockInDetail(string serialNumber, string sKU, int count, StockAreaType environmentRequirement, string remark = "")
        {
            PaperSerialNumber = serialNumber;
            SKU = sKU;
            Count = count;
            EnvironmentRequirement = environmentRequirement;
            Remark = remark;
        }

        public long StockInPaperId { get; protected set; }
        public string PaperSerialNumber { get; protected set; }
        public string SKU { get; protected set; }
        public int Count { get; protected set; }
        public StockAreaType EnvironmentRequirement { get; protected set; }
        public string Remark { get; protected set; }
    }
}
