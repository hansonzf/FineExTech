using Shippment.Domain.AggregateModels.LocationAggregate;
using Shippment.Domain.AggregateModels.RouterAggregate;

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

        [Fact]
        public void Get_outer_boundary_leg_of_route()
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
            Leg actual = route.GetRouteLeg(0);

            Assert.NotNull(actual);
            Assert.Equal("武汉 -> 上海 专线中转（2）", actual.RouteName);
            Assert.Equal(1, actual.From.LocationId);
            Assert.Equal(2, actual.To.LocationId);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Get_specific_index_leg_of_route(int index)
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
            Leg actual = route.GetRouteLeg(index);

            Assert.NotNull(actual);
            Assert.Equal("武汉 -> 上海 专线中转（2）", actual.RouteName);
            switch (index)
            {
                case 1:
                    Assert.Equal(1, actual.From.LocationId);
                    Assert.Equal(3, actual.To.LocationId);
                    break;
                case 2:
                    Assert.Equal(3, actual.From.LocationId);
                    Assert.Equal(4, actual.To.LocationId);
                    break;
                case 3:
                    Assert.Equal(4, actual.From.LocationId);
                    Assert.Equal(2, actual.To.LocationId);
                    break;
            }
        }
    }
}
