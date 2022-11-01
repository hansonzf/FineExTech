using DomainBase;

namespace Warehouse.Domain.AggregatesModel.CountingCargoPaperAggregate
{
    public class CountingCargoResult : ValueObject
    {
        public CountingCargoResult(string countingPaperNum, int index, string sKU, int count, string proof)
        {
            CountingPaperNumber = countingPaperNum;
            Index = index;
            SKU = sKU;
            Count = count;
            Proof = new Evidence(proof);
        }

        public string CountingPaperNumber { get; private set; }
        public int Index { get; private set; }
        public string SKU { get; private set; }
        public int Count { get; private set; }
        public Evidence Proof { get; protected set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CountingPaperNumber;
            yield return Index;
            yield return SKU;
            yield return Count;
            yield return Proof;
        }
    }
}
