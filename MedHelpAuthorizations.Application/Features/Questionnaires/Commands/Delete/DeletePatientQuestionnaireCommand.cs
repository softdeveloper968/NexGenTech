using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Application.Features.Questionnaires.Commands.Delete
{
    public class DeletePatientQuestionnaireCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeletePatientQuestionnaireCommandHandler : IRequestHandler<DeletePatientQuestionnaireCommand, Result<int>>
    {        
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeletePatientQuestionnaireCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }

        public async Task<Result<int>> Handle(DeletePatientQuestionnaireCommand command, CancellationToken cancellationToken)
        {   
            var questionnaire = await _unitOfWork.Repository<PatientQuestionnaire>()
                .Entities.Include(x => x.PatientQuestionnairesAnswers)
                .FirstOrDefaultAsync(x => x.Id == command.Id);
            await _unitOfWork.Repository<PatientQuestionnaire>().DeleteAsync(questionnaire);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(questionnaire.Id, "PatientQuestionnaire Deleted");        }
    }
}