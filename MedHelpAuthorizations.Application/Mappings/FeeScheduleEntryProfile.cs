using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetAllPaged;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class FeeScheduleEntryProfile : Profile
    {
        public FeeScheduleEntryProfile()
        {
            CreateMap<AddEditFeeScheduleEntryCommand, ClientFeeScheduleEntry>()
                .ForPath(dest => dest.ClientCptCode, src => src.Ignore());
            CreateMap<ClientFeeScheduleEntry, GetAllFeeScheduleEntryResponse>().ReverseMap();
               //   .ForMember(dest => dest.importStatus, opt => opt.MapFrom(src => src.ImportStatus));
        }
    }
}
