using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientKpis.Commands.AddEdit
{
	public class AddEditClientKpisCommand : IRequest<Result<int>>
	{
		public int Id { get; set; }
		public int ClientId { get; set; }
		public int? DailyClaimCount { get; set; }
		public decimal? MonthlyCashCollection { get; set; }
		public int? VolumeCredentialDenials { get; set; }
		public decimal? CredentialDenialsCashValue { get; set; }
		public decimal? ClaimDenialPercentage { get; set; }
		public decimal? DemographicDenialPercentage { get; set; }
		public decimal? CodingDenialPercentage { get; set; }
		public int? AverageSubmitDays { get; set; }
		public int? AverageDaysInReceivables { get; set; }
		public decimal? AR90DaysInsurancePercentage { get; set; }
		public decimal? AR90DaysSelfPayPercentage { get; set; }
		public decimal? AR180DaysInsurancePercentage { get; set; }
		public decimal? AR180DaysSelfPayPercentage { get; set; }
		public decimal? CleanClaimRate { get; set; }
		public int? Visits { get; set; }
        public decimal? Charges { get; set; }
        public decimal? DenialRate { get; set; }
		public decimal? CollectionPercentage { get; set; }
		public decimal? CashCollections { get; set; }
		public decimal? Over90Days { get; set; }
		public decimal? DaysInAR { get; set; }
		public decimal? BDRate { get; set; }

		#region Chart KPIs
		public decimal? CodingAccuracy { get; set; }
		public decimal? DocumentationAccuracy { get; set; }
		public decimal? ChartCompletionTiming { get; set; }
		public decimal? OrganizationalPassRate { get; set; }
		public decimal? ComplianceAccuracy { get; set; }
		#endregion

		#region Provider KPIs
		public decimal? ScheduledAppointments { get; set; }
		public decimal? DailyCompletedVisits { get; set; }
		public decimal? NoShow { get; set; }
		public decimal? OpenCharts { get; set; }
		#endregion

	}
	public class AddEditClientKpisCommandHandler : IRequestHandler<AddEditClientKpisCommand, Result<int>>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IStringLocalizer<AddEditClientKpisCommandHandler> _localizer;

		public AddEditClientKpisCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditClientKpisCommandHandler> localizer)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_localizer = localizer;
		}

		public async Task<Result<int>> Handle(AddEditClientKpisCommand command, CancellationToken cancellationToken)
		{
			try
			{
				if (command.Id == 0)
				{

					var clientKpiData = _mapper.Map<ClientKpi>(command);
					clientKpiData = await _unitOfWork.Repository<ClientKpi>().AddAsync(clientKpiData);
					await _unitOfWork.Commit(cancellationToken);

					var client = await _unitOfWork.Repository<Domain.Entities.Client>().GetByIdAsync(command.ClientId);
					client.ClientKpiId = clientKpiData.Id;
					await _unitOfWork.Commit(cancellationToken);

					return await Result<int>.SuccessAsync(clientKpiData.Id, _localizer["Client Kpi Saved"]);
				}
				else
				{
					var clientKpiData = await _unitOfWork.Repository<ClientKpi>().Entities
						.Include("Client")
						.FirstOrDefaultAsync(x => x.Id == command.Id);

					if (clientKpiData != null)
					{
						_mapper.Map(command, clientKpiData);
						clientKpiData.Client.ClientKpiId = command.Id;

						await _unitOfWork.Repository<ClientKpi>().UpdateAsync(clientKpiData);
						await _unitOfWork.Commit(cancellationToken);
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
