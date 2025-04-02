using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.ClientKpi.Commands
{
    public class AddEditAdminClientKpisCommand : IRequest<Result<int>>
    {
        public int TenantId { get; set; }
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int? DailyClaimCount { get; set; }
        public decimal? MonthlyCashCollection { get; set; }
        public int? VolumeCredentialDenials { get; set; }
        public decimal? CredentialDenialsCashValue { get; set; }
        public decimal? ClaimDenialPercentage { get; set; }
        public int? DemographicDenialPercentage { get; set; }
        public int? CodingDenialPercentage { get; set; }
        public int? AverageSubmitDays { get; set; }
        public int? AverageDaysInReceivables { get; set; }
        public decimal? AR90DaysInsurancePercentage { get; set; }
        public decimal? AR90DaysSelfPayPercentage { get; set; }
        public decimal? AR180DaysInsurancePercentage { get; set; }
        public decimal? AR180DaysSelfPayPercentage { get; set; }
        public decimal? CleanClaimRate { get; set; }
    }
    public class AddEditAdminClientKpisCommandHandler : IRequestHandler<AddEditAdminClientKpisCommand, Result<int>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditAdminClientKpisCommandHandler> _localizer;

        public AddEditAdminClientKpisCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory, IMapper mapper, IStringLocalizer<AddEditAdminClientKpisCommandHandler> localizer)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditAdminClientKpisCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(command.TenantId);

                if (command.Id == 0)
                {
                    var clientKpiData = _mapper.Map<MedHelpAuthorizations.Domain.Entities.ClientKpi>(command);
                    clientKpiData = await unitOfWork.Repository<MedHelpAuthorizations.Domain.Entities.ClientKpi>().AddAsync(clientKpiData);
                    await unitOfWork.Commit(cancellationToken);

                    var client = await unitOfWork.Repository<Domain.Entities.Client>().GetByIdAsync(command.ClientId);
                    client.ClientKpiId = clientKpiData.Id;
                    await unitOfWork.Commit(cancellationToken);

                    return await Result<int>.SuccessAsync(clientKpiData.Id, _localizer["Client Kpi Saved"]);
                }
                else
                {
                    var clientKpiData = await unitOfWork.Repository<MedHelpAuthorizations.Domain.Entities.ClientKpi>().Entities
                        .Include("Client")
                        .FirstOrDefaultAsync(x => x.Id == command.Id);

                    if (clientKpiData != null)
                    {
                        _mapper.Map(command, clientKpiData);
                        clientKpiData.Client.ClientKpiId = command.Id;

                        await unitOfWork.Repository<MedHelpAuthorizations.Domain.Entities.ClientKpi>().UpdateAsync(clientKpiData);
                        await unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(clientKpiData.Id, _localizer["Client Kpi Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["Client Kpi Found!"]);
                    }
                }
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(_localizer[$"Error Saving ClientKpi! {ex.Message}"]);
            }
        }
    }
}