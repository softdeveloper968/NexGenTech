using MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetById;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetAllPaged
{
    public class GetAllPagedPatientQuestionnairesResponse 
    {

        public string Name { get; set; }

        public string AuthNumber { get; set; }
        public int Id { get; set; }

        public int PatientId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public int? AuthorizationId { get; set; }
        public DateTime? ProcessedOn { get; set; }

        public int ClientQuestionnaireId { get; set; }
        //public int ClientCategoryId { get; set; }

        public IList<ClientQuestionnaireCategoryQuestion> ClientQuestionnaireQuestion { get; set; }
        public IList<PatientQuestionnaireAnswer> PatientQuestionnaireAnswers { get; set;}

    }
}