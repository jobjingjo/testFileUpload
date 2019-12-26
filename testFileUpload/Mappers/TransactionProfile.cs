using AutoMapper;
using testFileUpload.Core.Models;
using testFileUpload.Models;

namespace testFileUpload.Mappers
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionViewModel>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                        dest.Payment,
                    opt => opt.MapFrom(src => $"{src.Amount} {src.CurrencyCode}"))
                .ForMember(dest =>
                        dest.Status,
                    opt => opt.MapFrom(src => $"{src.Status}"));
        }
    }
}