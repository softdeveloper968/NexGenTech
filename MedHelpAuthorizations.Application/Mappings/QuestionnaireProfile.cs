using AutoMapper;
using MedHelpAuthorizations.Application.Features.Questionnaires.Commands.AddEdit;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class QuestionnaireProfile : Profile
    {
        public QuestionnaireProfile()
        {           
            CreateMap<PatientQuestionnaireAnswer, PatientQuestionnaireAnswerRequest>()
                .ReverseMap();
            CreateMap<PatientQuestionnaire, AddEditPatientQuestionnaireCommand>()
                .ReverseMap();
        }
    }
}
