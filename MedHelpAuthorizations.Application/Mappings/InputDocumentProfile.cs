using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.InputDocuments.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.IntegratedServices.InputDocuments.Queries.GetAll;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class InputDocumentProfile : Profile
    {
        public InputDocumentProfile()
        {
            CreateMap<InputDocument, AddEditInputDocumentCommand>()              
                .ReverseMap();
            CreateMap<InputDocument, GetAllInputDocumentsResponse>()
                .ReverseMap();
            CreateMap<InputDocument, GetAllInputDocumentsResponse>()
            //.ForMember(dest => dest.ClaimStatusBatch, opt => opt.MapFrom(src => src.ClaimStatusBatch))
            .ForMember(dest => dest.ClaimStatusBatches, opt => opt.MapFrom(src => src.ClaimStatusBatches));

            CreateMap<InputDocument, GetAllInputDocumentsResponse>()
                .ReverseMap();
        }
    }
}