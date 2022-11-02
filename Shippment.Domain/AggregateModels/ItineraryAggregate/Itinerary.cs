using DomainBase;
using Shippment.Domain.AggregateModels.LocationAggregate;
using Shippment.Domain.AggregateModels.RouterAggregate;
using Shippment.Domain.AggregateModels.TransportOrderAggregate;
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
        private List<LocationDescription> _tracker;

        protected Itinerary()
        {
            _handings = new List<Handing>();
            _tracker = new List<LocationDescription>();
            CurrentLegIndex = 0;
        }

        public Itinerary(string trackingNumber)
            : this()
        {
            TrackingNumber = trackingNumber;
        }

        public string TrackingNumber { get; protected set; }
        public int CurrentLegIndex { get; protected set; }

        public ReadOnlyCollection<Handing> Handings
        {
            get => _handings.AsReadOnly();
            protected set => _handings = value.ToList();
        }

        public void TrackRoute(List<Leg> legs)
        {
            if (legs is null)
                return;

            if (legs.Count > 1)
            {
                var orderedLegs = legs.OrderBy(l => l.LegIndex);
                foreach (var item in orderedLegs)
                {
                    if (_tracker.Last() == item.From)
                        continue;

                    _tracker.Append(item.From);
                }
                _tracker.Add(orderedLegs.Last().To);
            }
        }

        public bool IsMatchDeliveryGoal(DeliverySpecification goal)
        {
            if (!_tracker.Any())
                return false;

            var from = _tracker.First();
            var to = _tracker.Last();

            if (from == goal.Origin && to == goal.Destination)
                return true;
            else
                return false;
        }

        public void Log(Handing handingEvent)
        {
            if (handingEvent.HandingType > 30 && !_tracker.Any())
                return;

            if (handingEvent.HandingType == Handing.Arraiving)
                CurrentLegIndex = _tracker.IndexOf(handingEvent.Location);

            _handings.Add(handingEvent);
        }

        public string FlushLog()
        {
            if (!_handings.Any())
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            var orderedHandings = _handings.OrderBy(h => h.OperationTime);
            foreach (var evt in orderedHandings)
            {
                sb.AppendLine(evt.HandingDescription);
            }

            return sb.ToString();
        }
    }
}
