using AutoMapper;
using Optio.Core.Entities;
using RGBA.Optio.Core.Entities;
using RGBA.Optio.Domain.Models;

namespace RGBA.Optio.Domain.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category,CategoryModel>().ReverseMap();
            CreateMap<Channels,ChannelModel>().ReverseMap();
            CreateMap<Currency,CurrencyModel>().ReverseMap();
            CreateMap<Location,LocationModel>().ReverseMap();
            CreateMap<Merchant,MerchantModel>().ReverseMap();
            CreateMap<Transaction,TransactionModel>().ReverseMap();
            CreateMap<TypeOfTransaction,TransactionTypeModel>().ReverseMap();
            CreateMap<User,UserModel>().ReverseMap();   
            CreateMap<ExchangeRate,ExchangeRateModel>().ReverseMap();
        }
    }
}
