using Warehouse.Domain.AggregatesModel.StockInAggregate;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;
using Warehouse.Infrastructure.Repositories;

namespace Warehouse.Domain.UnitTest
{
    public class StockhouseTest
    {
        [Fact]
        public void Create_storehouse_should_sucess()
        {
            var storehouse = Storehouse.CreateInstance(1, 1, "≤‚ ‘≤÷ø‚");
            
            Assert.NotNull(storehouse);
            Assert.Equal(1, storehouse.TenantId);
            Assert.Equal(1, storehouse.MerchantId);
        }

        [Fact]
        public void New_created_storehouse_will_not_initialized()
        {
            var storehouse = Storehouse.CreateInstance(1, 1, "≤‚ ‘≤÷ø‚");

            Assert.False(storehouse.Initialized);
        }

        [Fact]
        public void Not_initialized_storehouse_cannot_process_work()
        {
            var storehouse = Storehouse.CreateInstance(1, 1, "≤‚ ‘≤÷ø‚");
            string paperNumber = "123456";

            Assert.Throws<InvalidOperationException>(
                () => storehouse.StockInProcess(paperNumber));
        }

        [Fact]
        public void Add_storehouse_location_should_success()
        {
            var storehouse = Storehouse.CreateInstance(1, 1, "≤‚ ‘≤÷ø‚");
            string expectLocationCode = "test-1-1";
            string expectLocationName = "test-location-name";
            float expectMaxVolume = 1000;

            bool result = storehouse.AddLocation(expectLocationCode, expectLocationName, expectMaxVolume);
            var stockLocation = storehouse.GetStockArea(expectLocationCode);

            Assert.True(result);
            Assert.NotNull(stockLocation);
            Assert.Equal(expectLocationName, stockLocation.LocationName);
            Assert.Equal(expectMaxVolume, stockLocation.MaxLoad);
        }
    }
}