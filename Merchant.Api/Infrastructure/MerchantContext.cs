using Merchants.Api.Infrastructure.EntityConfigurations;
using Merchants.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Merchants.Api.Infrastructure
{
    public class MerchantContext : DbContext
    {
        public MerchantContext(DbContextOptions<MerchantContext> options)
            : base(options)
        {

        }

        public DbSet<Partner> Partners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PartnerEntityTypeConfiguration());
        }
    }
}
