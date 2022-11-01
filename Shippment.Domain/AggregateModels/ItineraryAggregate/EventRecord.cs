using DomainBase;
using Shippment.Domain.AggregateModels.LocationAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shippment.Domain.AggregateModels.ItineraryAggregate
{
    public class EventRecord : Entity
    {
        public long ItineraryId { get; protected set; }
        public DateTime EventTime { get; protected set; }
        public LocationDescription Location { get; protected set; }
        public string OperationName { get; protected set;  }

        protected EventRecord()
        { }
    }
}
