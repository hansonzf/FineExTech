using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;

namespace Warehouse.Infrastructure.EntityConfigurations
{
    public class StorehouseEntityTypeConfiguration : IEntityTypeConfiguration<Storehouse>
    {
        public void Configure(EntityTypeBuilder<Storehouse> builder)
        {
            builder.HasKey(s => s.Id).IsClustered(false);

            builder.Property(s => s.TenantId).IsRequired();
            builder.Property(s => s.MerchantId).IsRequired();

            builder.Property(s => s.WarehouseName)
                .HasMaxLength(100)
                .IsRequired();

            builder.OwnsOne<Address>("WarehouseAddress");

            builder.Ignore(s => s.Initialized);
        }
    }

    public class LocationEntityTypeConfiguration : IEntityTypeConfiguration<StockArea>
    {
        public void Configure(EntityTypeBuilder<StockArea> builder)
        {
            builder.HasKey(l => l.Id).IsClustered(false);

            builder.Property(l => l.Code)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(l => l.LocationName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(l => l.MaxLoad)
                .IsRequired()
                .HasDefaultValue<float>(0);

            builder.Property(builder => builder.UsedLoad)
                .IsRequired()
                .HasDefaultValue<float>(0);

            builder.Property(builder => builder.Useable)
                .IsRequired()
                .HasDefaultValue<bool>(true);

            builder.Property(builder => builder.AllowOverload)
                .IsRequired()
                .HasDefaultValue<bool>(true);


            builder.HasIndex(l => l.StorehouseId).IsClustered(true);
            builder.HasIndex(l => new { l.Code, l.StorehouseId }).IsUnique();
        }
    }
}
