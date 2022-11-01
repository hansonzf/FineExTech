using DomainBase;
using Shippment.Domain.AggregateModels.LocationAggregate;
using Shippment.Domain.AggregateModels.RouterAggregate;
using Shippment.Domain.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shippment.Domain.AggregateModels.ItineraryAggregate
{
    public class Itinerary : Entity, IAggregateRoot
    {
        private List<Handing> _handings;

        protected Itinerary()
        {
            _handings = new List<Handing>();
        }

        public Itinerary(string trackingNumber)
            : this()
        {
            TrackingNumber = trackingNumber;
        }

        public string TrackingNumber { get; protected set; }


        public ReadOnlyCollection<Handing> Handings
        {
            get => _handings.AsReadOnly();
            protected set => _handings = value.ToList();
        }

        public void Log(Handing handingEvent)
        {
            _handings.Add(handingEvent);
        }
    }
}
