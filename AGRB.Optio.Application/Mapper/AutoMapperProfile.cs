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
            CreateMap<Channels,ChanellModel>().ReverseMap();
            CreateMap<Currency,CurrencyModel>().ReverseMap();
            CreateMap<Location,locationModel>().ReverseMap();
            CreateMap<Merchant,MerchantModel>().ReverseMap();
            CreateMap<Transaction,TransactionModel>().ReverseMap();
            CreateMap<TypeOfTransaction,TransactionTypeModel>().ReverseMap();
            CreateMap<User,UserModel>().ReverseMap();   
            CreateMap<ValuteCourse,ValuteModel>().ReverseMap();
        }
    }
}
