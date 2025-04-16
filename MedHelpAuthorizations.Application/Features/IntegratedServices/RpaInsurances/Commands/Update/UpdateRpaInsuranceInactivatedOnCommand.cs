using AutoMapper;
using MedHelpAuthorizations.Application.Features.Documents.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Base;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.RpaInsurances.Commands.Update
{
    public class UpdateRpaInsuranceInactivatedOnCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
    }
    public class UpdateRpaInsuranceInactivatedOnCommandHandler : IRequestHandler<UpdateRpaInsuranceInactivatedOnCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<UpdateRpaInsuranceInactivatedOnCommandHandler> _localizer;

        public UpdateRpaInsuranceInactivatedOnCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<UpdateRpaInsuranceInactivatedOnCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(UpdateRpaInsuranceInactivatedOnCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var rpaInsurance = await _unitOfWork.Repository<RpaInsurance>().Entities
                   .Where(x => x.Id == command.Id).FirstAsync();

                
                rpaInsurance.InactivatedOn = command.IsActive ? null : DateTime.UtcNow;

                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(rpaInsurance.Id, _localizer["RpaInsurance.InactivivatedOn Property Updated"]);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
