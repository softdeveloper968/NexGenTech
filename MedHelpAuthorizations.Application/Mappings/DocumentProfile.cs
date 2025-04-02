using AutoMapper;
using MedHelpAuthorizations.Application.Features.Documents.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Documents.Commands.AddEditByPatient;
using MedHelpAuthorizations.Application.Features.Documents.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.Documents.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Documents.Queries.GetByCriteria;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<AddEditDocumentCommand, Document>()                
                .ReverseMap();
            CreateMap<AddEditDocumentByPatientCommand, AddEditDocumentCommand>().ReverseMap();
            CreateMap<Document, GetAllDocumentsResponse>()
                .ForMember(x => x.DocumentTypeName, map => map.MapFrom(y => y.DocumentType.Name))
                .ForMember(x => x.DocumentTypeId, map => map.MapFrom(y => y.DocumentType.Id))
                .ReverseMap();
            CreateMap<Document, GetByCriteriaDocumentsResponse>()
                .ForMember(x => x.DocumentTypeName, map => map.MapFrom(y => y.DocumentType.Name))
                .ForMember(x => x.DocumentTypeId, map => map.MapFrom(y => y.DocumentType.Id))
                .ForMember(x => x.AuthorizationCreatedOn, map => map.MapFrom(y => y.Authorization.CreatedOn))
                .ReverseMap();
            CreateMap<Document, GetByIdDocumentsResponse>()
                .ForMember(x => x.DocumentTypeName, map => map.MapFrom(y => y.DocumentType.Name))
                .ForMember(x => x.DocumentTypeId, map => map.MapFrom(y => y.DocumentType.Id))
                .ReverseMap();
        }
    }
}