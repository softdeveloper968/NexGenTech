using AutoMapper;
using MedHelpAuthorizations.Application.Features.DocumentTypes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.DocumentTypes.Queries.GetAll;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class DocumentTypeProfile : Profile
    {
        public DocumentTypeProfile()
        {
            CreateMap<GetAllDocumentTypeResponse, DocumentType>().ReverseMap();
            CreateMap<AddEditDocumentTypeCommand, DocumentType>().ReverseMap();
        }
    }
}
