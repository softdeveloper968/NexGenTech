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
using System.Collections.Generic;


namespace MedHelpAuthorizations.Application.Features.Questionnaires.Commands.AddEdit
{
    public partial class AddEditPatientQuestionnaireCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        public int ClientQuestionnaireId { get; set; }


        public int AuthorizationId { get; set; }
        public int PatientId { get; set; }
        public List<PatientQuestionnaireAnswerRequest> PatientQuestionnairesAnswers { get; set; }
    }

    public class AddEditPatientQuestionnaireCommandHandler : IRequestHandler<AddEditPatientQuestionnaireCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditPatientQuestionnaireCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AddEditPatientQuestionnaireCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditPatientQuestionnaireCommandHandler> localizer, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(AddEditPatientQuestionnaireCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var questionnaire = _mapper.Map<PatientQuestionnaire>(command);                   
                await _unitOfWork.Repository<PatientQuestionnaire>().AddAsync(questionnaire);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(questionnaire.Id, _localizer["PatientQuestionnaire Saved"]);
            }
            else
            {
                var questionnaire = await _unitOfWork.Repository<PatientQuestionnaire>().GetByIdAsync(command.Id);
                if (questionnaire != null)
                {
                    _mapper.Map(command, questionnaire);
                    await _unitOfWork.Repository<PatientQuestionnaire>().UpdateAsync(questionnaire);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(questionnaire.Id, _localizer["PatientQuestionnaire Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["PatientQuestionnaire Not Found!"]);
                }
            }
        }
    }
}