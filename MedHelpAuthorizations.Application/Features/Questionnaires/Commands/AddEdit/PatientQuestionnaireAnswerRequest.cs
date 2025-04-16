using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Questionnaires.Commands.AddEdit
{
    public class PatientQuestionnaireAnswerRequest
    {
        public int Id { get; set; }
        public int PatientQuestionnaireId { get; set; }
        public int ClientQuestionnaireCategoryQuestionId { get; set; }
        //[StringLength(50)]

        private bool _otherOptions = false;

        public bool CustomAnswer
        {
            get { return _otherOptions; }
            set { _otherOptions = value; }
        }


        public string Answer { get; set; }
    }
}
