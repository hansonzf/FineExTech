using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.AggregatesModel.StockInAggregate;

namespace Warehouse.Infrastructure.EntityConfigurations
{
    public class StockInPaperEntityTypeConfiguration : IEntityTypeConfiguration<StockInPaper>
    {
        public void Configure(EntityTypeBuilder<StockInPaper> builder)
        {
            builder.HasKey(s => s.Id)
                .IsClustered(false);

            builder.Property(s => s.TenantId)
                .IsRequired();

            builder.Property(s => s.PaperSerialNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(s => s.StockhouseId)
                .IsRequired();

            builder.Property(s => s.CargoOwnerId)
                .IsRequired();

            builder.Property(s => s.Status).HasConversion<string>();

            builder.HasIndex(i => new 
                { 
                    i.TenantId, 
                    i.StockhouseId 
                }).IsClustered(true);
            builder.HasIndex(i => i.PaperSerialNumber);
        }
    }

    public class StockInDetailEntityTypeConfiguration : IEntityTypeConfiguration<StockInDetail>
    {
        public void Configure(EntityTypeBuilder<StockInDetail> builder)
        {
            builder.HasKey(d => d.Id).IsClustered(false);


            builder.HasIndex(d => d.PaperSerialNumber).IsClustered(true);
            builder.HasIndex(d => d.CargoOwner);
            builder.HasIndex(d => d.SKU);
        }
    }
}
