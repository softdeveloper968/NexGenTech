using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using MedHelpAuthorizations.Domain.Entities;
using System;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.Enums;


namespace MedHelpAuthorizations.Application.Features.Questionnaires.Commands.AddEdit
{
    public partial class AddEditClientQuestionnaireCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public StateEnum RelatedState { get; set; } = StateEnum.UNK; // Unknown
    }

    public class AddEditClientQuestionnaireCommandHandler : IRequestHandler<AddEditClientQuestionnaireCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditClientQuestionnaireCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public AddEditClientQuestionnaireCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditClientQuestionnaireCommandHandler> localizer, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(AddEditClientQuestionnaireCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var questionnaire = _mapper.Map<ClientQuestionnaire>(command);
                    questionnaire.Name = command.Name;
                    questionnaire.Description = command.Description;
                    questionnaire.RelatedState = command.RelatedState;                    
                await _unitOfWork.Repository<ClientQuestionnaire>().AddAsync(questionnaire);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(questionnaire.Id, _localizer["ClientQuestionnaire Saved"]);
            }
            else
            {
                var questionnaire = await _unitOfWork.Repository<ClientQuestionnaire>().GetByIdAsync(command.Id);
                if (questionnaire != null)
                {
                    _mapper.Map(command, questionnaire);
                    await _unitOfWork.Repository<ClientQuestionnaire>().UpdateAsync(questionnaire);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(questionnaire.Id, _localizer["ClientQuestionnaire Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["ClientQuestionnaire Not Found!"]);
                }
            }
        }
    }
}