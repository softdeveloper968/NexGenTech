using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientDocuments.Command.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientDocuments.Queries;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeRoles;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientDocumentProfile : Profile
    {
        public ClientDocumentProfile()
        {
            CreateMap<ClientDocument, AddEditClientDocumentCommand>().ReverseMap();
            CreateMap<ClientDocument, GetAllPagedClientDocumentsResponse>().ReverseMap();
        }
    }
}
