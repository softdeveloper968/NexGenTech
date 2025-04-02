using System;
using AutoMapper;

namespace MedHelpAuthorizations.Application.Mappings.Converters
{
    public class DateTimeTypeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {

            return System.Convert.ToDateTime(source);
        }
    }
    public class ConverterProfile : Profile
    {
        public ConverterProfile()
        {
            CreateMap<string, DateTime>().ConvertUsing(new DateTimeTypeConverter());
        }
    }
}
