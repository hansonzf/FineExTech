using DomainBase;

namespace Shippment.Domain.AggregateModels.ScheduleAggregate
{
    public class TimeManagement : ValueObject
    {
        public DateTime EstimateSetoutTime { get; protected set; }
        public DateTime? FactSetoutTime { get; protected set; }
        public DateTime? FactArrivedTime { get; protected set; }
        public double EstimateTransportInterval { get; protected set; }
        public double FactTransportInterval
        {
            get
            {
                if (!FactArrivedTime.HasValue || !FactSetoutTime.HasValue)
                    return 0f;

                return (FactArrivedTime.Value - FactSetoutTime.Value).TotalHours;
            }
        }
        public bool IsSetout => FactSetoutTime.HasValue;
        public bool IsArrived => FactArrivedTime.HasValue;

        public TimeManagement(DateTime estimateSetoutTime)
        {
            EstimateSetoutTime = estimateSetoutTime;
        }

        public TimeManagement(DateTime estimateSetoutTime, double estimateInterval, DateTime? factSetoutTime = null, DateTime? factArrivedTime = null)
            : this(estimateSetoutTime)
        {
            FactSetoutTime = factSetoutTime;
            FactArrivedTime = factArrivedTime;
            EstimateTransportInterval = estimateInterval;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EstimateSetoutTime;
            yield return FactSetoutTime;
            yield return FactArrivedTime;
            yield return EstimateTransportInterval;
        }
    }
}
