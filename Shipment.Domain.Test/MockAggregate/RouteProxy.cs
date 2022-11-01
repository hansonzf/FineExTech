using Shippment.Domain.AggregateModels.LocationAggregate;
using Shippment.Domain.AggregateModels.RouterAggregate;

namespace Shipment.Domain.Test.MockAggregate
{
    public class RouteProxy : Route
    {
        public RouteProxy(string nameOfRoute, LocationDescription origin, LocationDescription destination, Segment[] segments = default)
            : base(nameOfRoute, origin, destination, segments)
        { }

        public static List<Route> SeedTestData()
        {
            List<Route> store = new List<Route>();

            LocationDescription wh = new LocationDescription(1, "武汉");
            LocationDescription hf = new LocationDescription(2, "合肥");
            LocationDescription nj = new LocationDescription(3, "南京");
            LocationDescription sh = new LocationDescription(4, "上海");

            var route1 = new RouteProxy("武汉 -> 上海 干线直达", wh, sh);
            route1.Id = 1;
            store.Add(route1);

            var route2 = new RouteProxy("武汉 -> 上海 干线中转（1）", wh, sh, 
                new Segment[2] { 
                    new Segment(wh, nj, 680),
                    new Segment(nj, sh, 240)
                });
            route2.Id = 2;
            store.Add(route2);

            var route3 = new RouteProxy("武汉 -> 上海 干线中转（2）", wh, sh,
                new Segment[3] {
                    new Segment(wh, hf, 350),
                    new Segment(hf, nj, 300),
                    new Segment(nj, sh, 240)
                });
            route3.Id = 3;
            store.Add(route3);

            return store;
        }
    }
}
