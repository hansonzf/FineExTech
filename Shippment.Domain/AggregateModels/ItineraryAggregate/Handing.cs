using DomainBase;
using Shippment.Domain.AggregateModels.LocationAggregate;

namespace Shippment.Domain.AggregateModels.ItineraryAggregate
{
    public abstract class Handing : Entity
    {
        protected const int Loading = 1;
        protected const int Unloading = 2;
        protected const int Departing = 3;
        protected const int Arraiving = 4;
        protected const int Receiving = 5;
        protected const int Sending = 6;
            
        protected Handing()
        { }

        protected Handing(LocationDescription location)
        {
            Location = location;
        }

        public string TrackingNumber { get; protected set; }
        public LocationDescription Location { get; protected set; }
        public DateTime OperationTime { get; protected set; }
        public string HandingEventName { get; protected set; }
        public int HandingType { get; protected set; }

        public abstract void Process(string trackingNumber);
    }
}
