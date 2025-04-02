using AutoMapper;
using MedHelpAuthorizations.Application.Features.Patients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Mappings
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<AddEditPatientCommand, GetPatientByIdResponse>().ReverseMap();
        }
    }
}
