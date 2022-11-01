using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shippment.Domain.AggregateModels.ItineraryAggregate
{
    public interface ItineraryRepository : IRepository<Itinerary>
    {
        Task<bool> CreateNewItineraryAsync(Itinerary itinerary);
    }
}
