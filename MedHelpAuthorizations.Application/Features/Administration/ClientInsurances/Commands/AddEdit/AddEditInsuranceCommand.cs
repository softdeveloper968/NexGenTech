using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Commands.AddEdit
{
    public partial class AddEditInsuranceCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(125)]
        public string LookupName { get; set; }

        [Required]
        [StringLength(125)]
        public string Name { get; set; }
        public long? PhoneNumber { get; set; }
        public long? FaxNumber { get; set; }
        public string ExternalId { get; set; }
        public string PayerIdentifier { get; set; }
        public int? RpaInsuranceId { get; set; }
        public int ClientId { get; set; }
        public ICollection<Domain.Entities.ClientInsuranceFeeSchedule> ClientInsuranceFeeSchedules { get; set; }
        public bool RequireLocationInput { get; set; } = false;
        public bool RequirePayerIdentifier { get; set; } = false;//597
        public bool AutoCalcPenalty { get; set; } = false;

    }

    public class AddEditInsuranceCommandHandler : IRequestHandler<AddEditInsuranceCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditInsuranceCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public AddEditInsuranceCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditInsuranceCommandHandler> localizer, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(AddEditInsuranceCommand command, CancellationToken cancellationToken)
        {
            command.ClientId = _clientId;
            if (command.Id == 0)
            {
                var clientInsuranceExist = await _unitOfWork.Repository<ClientInsurance>().Entities
                                                .FirstOrDefaultAsync(cl => cl.ClientId == _clientId && (cl.LookupName.Trim() == command.LookupName.Trim() || cl.Name.Trim() == command.Name.Trim()));
                if (clientInsuranceExist != null)
                    return await Result<int>.FailAsync(_localizer["ClientInsurance already exists with same name or lookup name"]);

                var clientInsurance = _mapper.Map<ClientInsurance>(command);
                await _unitOfWork.Repository<ClientInsurance>().AddAsync(clientInsurance);
                await _unitOfWork.Commit(cancellationToken);

                return await Result<int>.SuccessAsync(clientInsurance.Id, _localizer["ClientInsurance Saved"]);
            }
            else
            {
                var clientInsurance = await _unitOfWork.Repository<ClientInsurance>().GetByIdAsync(command.Id);
                if (clientInsurance != null)
                {
                    clientInsurance.LookupName = command.LookupName;
                    clientInsurance.Name = command.Name;
                    clientInsurance.PhoneNumber = command.PhoneNumber;
                    clientInsurance.FaxNumber = command.FaxNumber;
                    clientInsurance.ExternalId = command.ExternalId;
                    clientInsurance.PayerIdentifier = command.PayerIdentifier;
                    clientInsurance.RpaInsuranceId = command.RpaInsuranceId;
                    clientInsurance.ClientId = command.ClientId;
                    clientInsurance.RequireLocationInput = command.RequireLocationInput;
                    clientInsurance.AutoCalcPenalty = command.AutoCalcPenalty;
                    await _unitOfWork.Repository<ClientInsurance>().UpdateAsync(clientInsurance);
                    await _unitOfWork.Commit(cancellationToken);
                    
					return await Result<int>.SuccessAsync(clientInsurance.Id, _localizer["ClientInsurance Updated"]);
				}
				else
				{
					return await Result<int>.FailAsync(_localizer["ClientInsurance Not Found!"]);
				}
			}
        }
    }
}