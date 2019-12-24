using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using testFileUpload.Core.Models;
using testFileUpload.Core.Types;

namespace testFileUpload.Core.Mappers
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<XmlTransaction, Transaction>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                        dest.Amount,
                    opt => opt.MapFrom(src => src.PaymentDetails.Amount))
                .ForMember(dest =>
                        dest.CurrencyCode,
                    opt => opt.MapFrom(src => src.PaymentDetails.CurrencyCode))
                .ForMember(dest =>
                        dest.Status,
                    opt => opt.MapFrom(src => src.Status))
                .ForMember(dest =>
                        dest.TransactionDate,
                    opt => opt.MapFrom(src => src.TransactionDate));

            CreateMap<CsvTransaction, Transaction>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                        dest.Amount,
                    opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest =>
                        dest.CurrencyCode,
                    opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest =>
                        dest.Status,
                    opt => opt.MapFrom(src => src.Status))
                .ForMember(dest =>
                        dest.TransactionDate,
                    opt => opt.MapFrom(src => src.TransactionDate));

            CreateMap<XmlStatus, TransactionStatus>().ConvertUsing<EnumConverter>();

        }

        public class EnumConverter : ITypeConverter<XmlStatus, TransactionStatus>
        {
            public TransactionStatus Convert(XmlStatus source, TransactionStatus destination, ResolutionContext context)
            {
                // Conversion logic here
                XmlStatus value = (XmlStatus)source;
                switch (value)
                {
                    case XmlStatus.Approved:
                        return TransactionStatus.Approved;
                    case XmlStatus.Rejected:
                        return TransactionStatus.Rejected;
                    case XmlStatus.Done:
                        return TransactionStatus.Done;
                    default:
                        return TransactionStatus.Unknow;
                }
            }
        }

        public class EnumConverter2 : ITypeConverter<CsvStatus, TransactionStatus>
        {
            public TransactionStatus Convert(CsvStatus source, TransactionStatus destination, ResolutionContext context)
            {
                // Conversion logic here
                CsvStatus value = (CsvStatus)source;
                switch (value)
                {
                    case CsvStatus.Approved:
                        return TransactionStatus.Approved;
                    case CsvStatus.Failed:
                        return TransactionStatus.Rejected;
                    case CsvStatus.Finished:
                        return TransactionStatus.Done;
                    default:
                        return TransactionStatus.Unknow;
                }
            }
        }
    }
}
