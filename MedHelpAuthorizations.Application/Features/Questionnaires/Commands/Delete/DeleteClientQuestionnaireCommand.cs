using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Features.Questionnaires.Commands.Delete
{
    public class DeleteClientQuestionnaireCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteClientQuestionnaireCommandHandler : IRequestHandler<DeleteClientQuestionnaireCommand, Result<int>>
    {        
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteClientQuestionnaireCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }

        public async Task<Result<int>> Handle(DeleteClientQuestionnaireCommand command, CancellationToken cancellationToken)
        {   
            var questionnaire = await _unitOfWork.Repository<ClientQuestionnaire>().GetByIdAsync(command.Id);
            await _unitOfWork.Repository<ClientQuestionnaire>().DeleteAsync(questionnaire);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(questionnaire.Id, "ClientQuestionnaire Deleted");        }
    }
}