using DomainBase;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;

namespace Warehouse.Domain.AggregatesModel.CountingCargoPaperAggregate
{
    public enum CargoHandleState
    {
        None = 0,
        Counted,
        Received,
        Returnback
    }
    public class CargoDetail : Entity
    {
        protected CargoDetail()
        { }

        internal CargoDetail(string paperNumber, int index, string sKU, float claimCount, StockAreaType requirement)
        {
            PaperNumber = paperNumber;
            Index = index;
            SKU = sKU;
            ClaimCount = claimCount;
            RequiredEnvironment = requirement;
            Status = CargoHandleState.None;
        }

        public string PaperNumber { get; protected set; }
        public int Index { get; protected set; }
        public int TenantId { get; protected set; }
        public string SKU { get; protected set; }
        public float ClaimCount { get; protected set; }
        public float Count { get; protected set; }
        public StockAreaType RequiredEnvironment { get; protected set; }
        public CargoHandleState Status { get; protected set; }

        public bool IsCountMatch
        {
            get
            {
                if (Status == CargoHandleState.Counted)
                    return ClaimCount == Count;
                else
                    return true;
            }
        }

        public int Verify(CountingCargoResult factCountResult)
        {
            Count = factCountResult.Count;
            Status = CargoHandleState.Counted;

            return Index;
        }
    }
}
