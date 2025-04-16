using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ImportDocumentMessage.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ImportDocumentMessage.Queries.GetInputDocumentMessageById;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ImportDocumentMessageProfile : Profile
	{
		public ImportDocumentMessageProfile()
		{
			CreateMap<ImportDocumentMessage, ImportDocumentMessageResponseModel>().ReverseMap();
			CreateMap<ImportDocumentMessage, ImportDocumentMessageResponse>().ReverseMap();
		}
	}
}
