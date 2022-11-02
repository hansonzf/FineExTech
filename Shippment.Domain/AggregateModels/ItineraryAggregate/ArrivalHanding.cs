using Shippment.Domain.AggregateModels.LocationAggregate;

namespace Shippment.Domain.AggregateModels.ItineraryAggregate
{
    public class ArrivalHanding : Handing
    {
        public ArrivalHanding(LocationDescription location)
            : base(location)
        {
            HandingType = Handing.Arraiving;
        }

        public override void Process(string trackingNumber)
        {
            TrackingNumber = trackingNumber;
            OperationTime = DateTime.Now;
            HandingEventName = @$"
{OperationTime}
 【{Location.City}】您的货物从已到达 【{Location.LocationName}】

";
        }
    }
}
