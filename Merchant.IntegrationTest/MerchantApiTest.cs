using Merchant.Api.Dtos;
using Merchant.IntegrationTest.Fixture;
using Merchants.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Merchant.IntegrationTest
{
    public class MerchantApiTest : IClassFixture<FineExWebApplicationFactory<Program>>
    {
        private readonly FineExWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public MerchantApiTest(FineExWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Create_an_merchant_should_success()
        {
            PartnerDto p = new PartnerDto 
            { 
                CompanyName = "²âÊÔ¹«Ë¾1",
                ContactBook = "[]",
                MerchantCode = DateTime.Now.ToString("yyyyMMddhhmmsshhhhhh"),
                TenantId = 1,
                CreatedTime = DateTime.Now
            };
            string postBody = JsonConvert.SerializeObject(p);
            var content = new StringContent(postBody);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _client.PostAsync("/partners", content);
            string message = await response.Content.ReadAsStringAsync();

            Assert.Equal(201, (int)response.StatusCode);
        }

        [Fact]
        public async Task Create_empty_merchant_should_return_badrequest()
        {
            PartnerDto p = new PartnerDto();
            string postBody = JsonConvert.SerializeObject(p);
            var content = new StringContent(postBody);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _client.PostAsync("/partners", content);
            string message = await response.Content.ReadAsStringAsync();

            Assert.Equal(400, (int)response.StatusCode);
        }

        [Theory]
        [InlineData(1, "20220929161354965872")]
        [InlineData(2, "20220930161354949342")]
        public async Task CheckMerchantCodeExist_should_return_false_when_doesnot_exist_tenant_and_code(int tenantId, string code)
        {
            int notExistTenantId = tenantId;
            string notExistCodeUnderTenant = code;
            string url = $"/exist/tenant/{notExistTenantId}/merchant/{notExistCodeUnderTenant}";

            var response = await _client.GetAsync(url);
            string message = await response.Content.ReadAsStringAsync();

            Assert.Equal(200, (int)response.StatusCode);
            Assert.False(bool.Parse(message));
        }
    }
}