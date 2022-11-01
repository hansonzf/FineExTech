using Shippment.Domain.AggregateModels;
using Shippment.Domain.AggregateModels.LocationAggregate;
using Shippment.Domain.AggregateModels.RouterAggregate;
using Shippment.Domain.AggregateModels.TransportOrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipment.Domain.Test
{
    public class OperationCenterManageRouteTest
    {
        [Fact]
        public void Create_direct_route_without_segment_should_success()
        {
            LocationDescription origin = new LocationDescription(1, "武汉");
            LocationDescription destination = new LocationDescription(2, "上海");

            var route = new Route("武汉 -> 上海 专线直达", origin, destination);

            Assert.NotNull(route);
            Assert.Equal(1, route.Origin.LocationId);
            Assert.Equal(2, route.Destination.LocationId);
        }

        [Fact]
        public void Create_transit_route_with_1_segment_should_success()
        {
            LocationDescription origin = new LocationDescription(1, "武汉");
            LocationDescription destination = new LocationDescription(2, "上海");
            LocationDescription step1 = new LocationDescription(3, "南京");

            Segment[] segments = new Segment[2]
            {
                new Segment(origin, step1, 500),
                new Segment(step1, destination, 400)
            };

            var route = new Route("武汉 -> 上海 专线中转（1）", origin, destination, segments);

            Assert.NotNull(route);
            Assert.Equal(1, route.Origin.LocationId);
            Assert.Equal(2, route.Destination.LocationId);
            Assert.Equal(900, route.Distance);
        }

        [Fact]
        public void Create_transit_route_with_2_segment_should_success()
        {
            LocationDescription origin = new LocationDescription(1, "武汉");
            LocationDescription destination = new LocationDescription(2, "上海");
            LocationDescription step1 = new LocationDescription(3, "合肥");
            LocationDescription step2 = new LocationDescription(4, "南京");

            Segment[] segments = new Segment[3]
            {
                new Segment(origin, step1, 350),
                new Segment(step1, step2, 350),
                new Segment(step2, destination, 300)
            };

            var route = new Route("武汉 -> 上海 专线中转（2）", origin, destination, segments);

            Assert.NotNull(route);
            Assert.Equal(1, route.Origin.LocationId);
            Assert.Equal(2, route.Destination.LocationId);
            Assert.Equal(1000, route.Distance);
        }

        [Fact]
        public void Add_segments_after_created_route_should_success()
        {
            LocationDescription origin = new LocationDescription(1, "武汉");
            LocationDescription destination = new LocationDescription(2, "上海");
            LocationDescription step1 = new LocationDescription(3, "合肥");
            LocationDescription step2 = new LocationDescription(4, "南京");

            Segment[] segments = new Segment[3]
            {
                new Segment(origin, step1, 350),
                new Segment(step1, step2, 350),
                new Segment(step2, destination, 300)
            };

            var route = new Route("武汉 -> 上海 专线中转（1）", origin, destination);
            route.AddSegment(segments);

            Assert.NotNull(route);
            Assert.Equal(1, route.Origin.LocationId);
            Assert.Equal(2, route.Destination.LocationId);
            Assert.Equal(1000, route.Distance);
        }

        [Fact]
        public void Replace_segment_for_exist_transit_route_should_success()
        {
            LocationDescription origin = new LocationDescription(1, "武汉");
            LocationDescription destination = new LocationDescription(2, "上海");
            LocationDescription step1 = new LocationDescription(3, "合肥");
            LocationDescription step2 = new LocationDescription(4, "南京");

            Segment[] segments = new Segment[2]
            {
                new Segment(origin, step1, 350),
                new Segment(step1, destination, 650)
            };

            var route = new Route("武汉 -> 上海 专线中转（2）", origin, destination, segments);
            Segment[] repalceSeg = new Segment[2]
            {
                new Segment(step1, step2, 350),
                new Segment(step2, destination, 300)
            };
            route.ReplaceSegment(1, repalceSeg);

            Assert.NotNull(route);
            Assert.Equal(1, route.Origin.LocationId);
            Assert.Equal(2, route.Destination.LocationId);
            Assert.Equal(1000, route.Distance);
        }
    }
}
