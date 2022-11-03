﻿using Moq;
using Shipment.Domain.Test.MockAggregate;
using Shippment.Domain.AggregateModels.LocationAggregate;
using Shippment.Domain.AggregateModels.TransportOrderAggregate;
using Shippment.Domain.AggregateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shippment.Domain.AggregateModels.RouterAggregate;
using Shippment.Domain.AggregateModels.ItineraryAggregate;

namespace Shipment.Domain.Test.TestFixture
{
    public class CustomerTestFixture
    {
        private readonly List<TransportOrder> _orderTestStore;
        private readonly List<Route> _routeTestStore;
        private Mock<ITransportOrderRepository> _mockRepository;

        public CustomerTestFixture()
        {
            _orderTestStore = TransportOrderProxy.SeedTestData();
            _routeTestStore = RouteProxy.SeedTestData();
            Initialize();
        }

        private void Initialize()
        {
            #region initialize mock TransportOrderRepository
            _mockRepository = new Mock<ITransportOrderRepository>();

            _mockRepository.Setup(rp => rp.GetAsync(It.Is<long>(id => id > 0)))
                .ReturnsAsync((long id) => _orderTestStore.First(o => o.Id == id));

            _mockRepository.Setup(rp => rp.GetWaitforAuditOrdersAsync())
                .ReturnsAsync(() => _orderTestStore.Where(o => o.Status == OrderStatus.Ordered).AsEnumerable());

            _mockRepository.Setup(rp => rp.GetWaitforPickupOrdersAsync())
                .ReturnsAsync(
                    () => _orderTestStore.Where(
                        o => o.Status == OrderStatus.Accepted &&
                            o.PickupCargoInfo.NeedPickupService &&
                            o.PickupCargoInfo.DispatchingId > 0));
            #endregion
        }

        public ITransportOrderRepository TransportOrderRepository => _mockRepository.Object;
        public LocationDescription WUHAN => new LocationDescription(1, "武汉");
        public LocationDescription SHANGHAI => new LocationDescription(2, "上海");
        public PickupDescription PickupInfo => new PickupDescription(
                true,
                EquipmentType.Vehicle,
                "武汉市洪山区东港科技园2栋5层",
                "张峰",
                "18571855277",
                new DateTime(2022, 10, 28, 20, 0, 0),
                "");
        public IEnumerable<Cargo> Cargos = new List<Cargo>
        {
            new Cargo("20个iphone 14 pro", new Dimension(0.5, 0.4, 0.8), new Weight(12, UnitOfWeight.KiloGram), 4),
            new Cargo("20个iphone 14 pro max", new Dimension(0.6, 0.5, 1), new Weight(15, UnitOfWeight.KiloGram), 4),
        };
        public List<Route> RouteTestStore => _routeTestStore;
    }
}
