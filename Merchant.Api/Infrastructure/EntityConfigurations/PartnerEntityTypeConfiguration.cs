using Merchants.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Merchants.Api.Infrastructure.EntityConfigurations
{
    public class PartnerEntityTypeConfiguration : IEntityTypeConfiguration<Partner>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Partner> builder)
        {
            builder.HasKey(m => m.Id).IsClustered(false);

            builder.Property(m => m.MerchantCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(m => m.CompanyName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.ContactBook)
                .IsRequired()
                .HasDefaultValue<string>("[]");

            builder.Property(m => m.CreatedTime)
                .ValueGeneratedOnAdd();

            builder.Property(m => m.InCooperating)
                .IsRequired()
                .HasDefaultValue<bool>(true);

            builder.HasIndex(m => new { m.TenantId, m.MerchantCode }).IsUnique();
            builder.HasIndex(m => m.TenantId).IsClustered();
        }
    }
}
