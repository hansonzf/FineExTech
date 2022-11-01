using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shippment.Domain.AggregateModels.ItineraryAggregate
{
    public class RedirectCargoHanding : Handing
    {
        public override void Process(string trackingNumber)
        {
            throw new NotImplementedException();
        }
    }
}
