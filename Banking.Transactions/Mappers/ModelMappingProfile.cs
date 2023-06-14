using AutoMapper;
using Banking.Framework.Domain;
using Banking.Transactions.Models;

namespace Banking.Transactions.Mappers
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            CreateMap<TransactionModel, AccountTransaction>()
                 .AfterMap<SetIdentityAction>()
                 .ForAllMembers(opts => opts.Ignore());

            CreateMap<TransactionResult, TransactionResultModel>()
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(o => o.Balance.Amount.ToString("N")))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(o => o.Balance.Currency.ToString()));
        }
    }
}
