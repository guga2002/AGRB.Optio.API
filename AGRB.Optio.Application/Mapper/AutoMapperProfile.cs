using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Models.RequestModels;
using AGRB.Optio.Domain.Entities;
using AutoMapper;

namespace AGRB.Optio.Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<Channels, ChannelModel>().ReverseMap();
            CreateMap<Currency, CurrencyModel>().ReverseMap();
            CreateMap<Location, LocationModel>().ReverseMap();
            CreateMap<Merchant, MerchantModel>().ReverseMap();
            CreateMap<Transaction, TransactionModel>().ReverseMap();
            CreateMap<TypeOfTransaction, TransactionTypeModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<ExchangeRate, ExchangeRateModel>().ReverseMap();
            CreateMap<Feadback, FeadbackModel>().ReverseMap();
        }
    }
}
