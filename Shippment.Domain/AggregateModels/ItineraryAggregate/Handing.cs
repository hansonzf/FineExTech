using DomainBase;
using Shippment.Domain.AggregateModels.LocationAggregate;
using Shippment.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shippment.Domain.AggregateModels.ItineraryAggregate
{
    public abstract class Handing : Entity
    {
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

        public abstract void Process(string trackingNumber);
    }
}
