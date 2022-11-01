using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.AggregatesModel.InventoryAggregate;

namespace Warehouse.Infrastructure.EntityConfigurations
{
    public class InventoryRecordEntityTypeConfiguration : IEntityTypeConfiguration<InventoryRecord>
    {
        public void Configure(EntityTypeBuilder<InventoryRecord> builder)
        {
            builder.HasKey(r => r.Id).IsClustered(false);

            builder.HasIndex(r => new { 
                r.StorehouseId, 
                r.CargoOwner, 
                r.SKU, 
                r.LocationCode 
            }).IsUnique();
            builder.HasIndex(r => r.StorehouseId).IsClustered(true);
        }
    }
}
