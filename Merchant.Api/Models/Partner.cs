using DomainBase;
using Newtonsoft.Json;

namespace Merchants.Api.Models
{
    public class Partner : Entity, IAggregateRoot
    {
        protected Partner() 
        { }

        public int TenantId { get; private set; }
        public string MerchantCode { get; private set; }
        public string CompanyName { get; private set; }
        public string ContactBook { get; private set; }
        public DateTime CreatedTime { get; private set; }
        public bool InCooperating { get; private set; }

        public static Partner CreateInstance(long? id = null)
        {
            return id.HasValue ? new Partner { Id = id.Value } : new Partner();
        }

        public Partner OpenAccount(int tenantId, string companyName)
        {
            TenantId = tenantId;
            MerchantCode = DateTime.Now.ToString("yyyyMMddhhmmssffffff");
            CompanyName = companyName;

            return this;
        }

        public Partner Dissolve()
        {
            InCooperating = false;

            return this;
        }

        public List<Contact> GetContacts()
        {
            return JsonConvert.DeserializeObject<List<Contact>>(ContactBook) ?? new List<Contact>();
        }

        public void SetContacts(List<Contact> contactBook)
        {
            if (contactBook.Any())
                ContactBook = JsonConvert.SerializeObject(contactBook);
        }
    }
}
