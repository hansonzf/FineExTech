namespace Merchant.Api.Dtos
{
    public class PartnerDto
    {
        public long Id { get; set; }
        public int TenantId { get; set; }
        public string MerchantCode { get; set; }
        public string CompanyName { get; set; }
        public string ContactBook { get; set; }
        public DateTime CreatedTime { get; set; }
    }

}
