using AutoMapper;
using MedHelpAuthorizations.Application.Features.Notes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetNotesByAuthorizationId;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class NotesProfile : Profile
    {
        public NotesProfile()
        {
            CreateMap<GetNotesByAuthorizationIdResponse, Note>().ReverseMap();
            CreateMap<AddEditNotesCommand, Note>().ReverseMap();
            CreateMap<GetNotesByIdResponse, Note>().ReverseMap();
        }
    }
}
