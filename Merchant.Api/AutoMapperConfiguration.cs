using AutoMapper;
using Merchant.Api.Dtos;
using Merchants.Api.Models;

namespace Merchant.Api
{
    public static class AutoMapperConfiguration
    {
        public static void InitialAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(FineExDataMapperProfile));
        }
    }

    public class FineExDataMapperProfile : Profile
    {
        public FineExDataMapperProfile()
        {
            CreateMap<PartnerDto, Partner>();
            CreateMap<Partner, PartnerDto>();
        }
    }
}
