using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Features.Dashboards.Authorizations.Queries.GetData
{
    public class GetDashboardDataQuery : IRequest<Result<DashboardDataResponse>>
    {
    }

    public class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, Result<DashboardDataResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUserService _userService;
        private readonly IPatientQueryService _patientQueryService;
        private readonly IAuthorizationQueryService _authorizationQueryService;
        private readonly IStringLocalizer<GetDashboardDataQueryHandler> _localizer;

        public GetDashboardDataQueryHandler(IUnitOfWork<int> unitOfWork, 
            IUserService userService, 
            IStringLocalizer<GetDashboardDataQueryHandler> localizer,
            IPatientQueryService patientQueryService, 
            IAuthorizationQueryService authorizationQueryService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _localizer = localizer;
            _patientQueryService = patientQueryService;
            _authorizationQueryService = authorizationQueryService;
        }

        public async Task<Result<DashboardDataResponse>> Handle(GetDashboardDataQuery query, CancellationToken cancellationToken)
        {
            var response = new DashboardDataResponse() { DashboardDataQueryResults = new List<DashboardDataQueryResult>(), DataEnterBarChart = new List<ChartSeries>()};

            #region Patient Count Queries
            var activePatientsCount = await _patientQueryService.GetActivePatientsCountAsync();
            var patientsAddedMtdCount = await _patientQueryService.GetPatientsAddedMtdCountAsync();
            var patientsAddedMtdNoBenefitCheckCount= await _patientQueryService.GetPatientsAddedMtdNoBenefitCheckCountAsync();
            #endregion

            #region Active authorizations
            var activeAuthorizationsCount = await _authorizationQueryService.GetAllActiveAuthorizationsCountAsync();
            var activeSudAuthorizationsCount = await _authorizationQueryService.GetActiveSudAuthorizationsCountAsync();
            var activeOmhcAuthorizationsCount = await _authorizationQueryService.GetActiveOmhcAuthorizationsCountAsync();
            var activePrpAuthorizationsCount = await _authorizationQueryService.GetActivePrpAuthorizationsCountAsync();
            var activeMhAuthorizationsCount = await _authorizationQueryService.GetActiveMhAuthorizationsCountAsync();

            //var activeOtherAuthorizationsCount = await _authorizationQueryService.GetActiveOtherAuthorizationsCountAsync();

            #endregion

            #region Authorizations Discharged This Month
            var dischargedAuthorizationsCount = await _authorizationQueryService.GetAllAuthorizationsDischargedMtdCountAsync();
            var dischargedSudAuthorizationsCount = await _authorizationQueryService.GetSudAuthorizationsDischargedMtdCountAsync();
            var dischargedOmhcAuthorizationsCount = await _authorizationQueryService.GetOmhcAuthorizationsDischargedMtdCountAsync();
            var dischargedPrpAuthorizationsCount = await _authorizationQueryService.GetPrpAuthorizationsDischargedMtdCountAsync();
            var dischargedMhAuthorizationsCount = await _authorizationQueryService.GetMhAuthorizationsDischargedMtdCountAsync();

            //var dischargedOtherAuthorizationsCount = await _authorizationQueryService.GetOtherAuthorizationsDischargedMtdCountAsync();

            #endregion

            #region Authorizations Not Completed
            var notCompletedAuthorizationsCount = await _authorizationQueryService.GetAllAuthorizationsNotCompletedCountAsync();
            var notCompletedSudAuthorizationsCount = await _authorizationQueryService.GetSudAuthorizationsNotCompletedCountAsync();
            var notCompletedOmhcAuthorizationsCount = await _authorizationQueryService.GetOmhcAuthorizationsNotCompletedCountAsync();
            var notCompletedPrpAuthorizationsCount = await _authorizationQueryService.GetPrpAuthorizationsNotCompletedCountAsync();
            var notCompletedMhAuthorizationsCount = await _authorizationQueryService.GetMhAuthorizationsNotCompletedCountAsync();

            //var notCompletedOtherAuthorizationsCount = await _authorizationQueryService.GetOtherAuthorizationsNotCompletedCountAsync();

            #endregion

            #region Patient Query Results
            DashboardDataQueryResult activePatientsQr = new DashboardDataQueryResult() { 
                QueryName = "Active Patients", 
                QueryCode = "AP", 
                QueryDescription = "Patients with an active (non-expired) authorization.",
                QueryRecordCount = activePatientsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Patients,
                QueryStateType = QueryStateTypeEnum.Active
            };
            response.DashboardDataQueryResults.Add(activePatientsQr);

            DashboardDataQueryResult patientsAddedMtdQr = new DashboardDataQueryResult()
            {
                QueryName = "Patients Month to Date",
                QueryCode = "PATM",
                QueryDescription = "Patients that were created this month.",
                QueryRecordCount = patientsAddedMtdCount,
                QuerySubjectType = QuerySubjectTypeEnum.Patients,
                QueryStateType = QueryStateTypeEnum.New
            };
            response.DashboardDataQueryResults.Add(patientsAddedMtdQr);

            DashboardDataQueryResult patientsAddedMtdNoBenefitCheckQr = new DashboardDataQueryResult()
            {
                QueryName = "New Patients - No Verification of Benefits",
                QueryCode = "PATNVB",
                QueryDescription = "Patients that were created this month and did not have benefits verified.",
                QueryRecordCount = patientsAddedMtdNoBenefitCheckCount,
                QuerySubjectType = QuerySubjectTypeEnum.Patients,
                QueryStateType = QueryStateTypeEnum.New
            };
            response.DashboardDataQueryResults.Add(patientsAddedMtdNoBenefitCheckQr);

            #endregion


            #region Authorizations Active Query Results

            DashboardDataQueryResult activeAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Total Active Authorizations",
                QueryCode = "TAA",
                QueryDescription = "All active (non-expired) authorizations.",
                QueryRecordCount = activeAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.Active,
                QueryAuthTypeNames = new string[4] {"SUD","OMHC", "PRP", "MH"}
            };
            response.DashboardDataQueryResults.Add(activeAuthorizationQr);

            DashboardDataQueryResult activeSudAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Active SUD Authorizations",
                QueryCode = "ASUDA",
                QueryDescription = "All active (non-expired) SUD authorizations.",
                QueryRecordCount = activeSudAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.Active,
                QueryAuthTypeNames = new string[1] { "SUD"}
            };
            if(activeSudAuthorizationsCount > 0)
                response.DashboardDataQueryResults.Add(activeSudAuthorizationQr);
            
            DashboardDataQueryResult activeOmhcResultAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Active OMHC Authorizations",
                QueryCode = "AOMHCA",
                QueryDescription = "All active (non-expired) OMHC authorizations.",
                QueryRecordCount = activeOmhcAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.Active,
                QueryAuthTypeNames = new string[1] { "OMHC" }
            };
            if(activeOmhcAuthorizationsCount > 0)
                response.DashboardDataQueryResults.Add(activeOmhcResultAuthorizationQr);

            DashboardDataQueryResult activePrpResultAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Active PRP Authorizations",
                QueryCode = "APRPA",
                QueryDescription = "All active (non-expired) PRP authorizations.",
                QueryRecordCount = activePrpAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.Active,
                QueryAuthTypeNames = new string[1] { "PRP" }
            };
            if(activePrpAuthorizationsCount > 0)
                response.DashboardDataQueryResults.Add(activePrpResultAuthorizationQr);

            DashboardDataQueryResult activeMhResultAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Active MH Authorizations",
                QueryCode = "AMHA",
                QueryDescription = "All active (non-expired) MH authorizations.",
                QueryRecordCount = activeMhAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.Active,
                QueryAuthTypeNames = new string[1] { "MH" }
            };
            if(activeMhAuthorizationsCount > 0)
                response.DashboardDataQueryResults.Add(activeMhResultAuthorizationQr);

            //DashboardDataQueryResult activeOtherAuthorizationQr = new DashboardDataQueryResult()
            //{
            //    QueryName = "Active Other Authorizations",
            //    QueryCode = "AOTHERA",
            //    QueryDescription = "All active (non-expired) Other Insurance authorizations.",
            //    QueryRecordCount = activeOtherAuthorizationsCount,
            //    QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
            //    QueryStateType = QueryStateTypeEnum.Active
            //};
            //response.DashboardDataQueryResults.Add(activeOtherAuthorizationQr);

            #endregion

            #region Authorizations Discharged Query Results

            DashboardDataQueryResult dischargedAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Total discharged Authorizations",
                QueryCode = "TDA",
                QueryDescription = "All discharged authorizations.",
                QueryRecordCount = dischargedAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.Discharged,
                QueryAuthTypeNames = new string[4] { "SUD", "OMHC", "PRP", "MH" }
            };
            response.DashboardDataQueryResults.Add(dischargedAuthorizationQr);

            DashboardDataQueryResult dischargedSudAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Discharged SUD Authorizations",
                QueryCode = "DSUDA",
                QueryDescription = "All discharged SUD authorizations.",
                QueryRecordCount = dischargedSudAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.Discharged,
                QueryAuthTypeNames = new string[1] { "SUD" }
            };

            if(dischargedSudAuthorizationsCount > 0)
                response.DashboardDataQueryResults.Add(dischargedSudAuthorizationQr);

            DashboardDataQueryResult dischargedOmhcResultAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Discharged OMHC Authorizations",
                QueryCode = "DOMHCA",
                QueryDescription = "All discharged OMHC authorizations.",
                QueryRecordCount = dischargedOmhcAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.Discharged,
                QueryAuthTypeNames = new string[1] { "OMHC" }
            };

            if(dischargedOmhcAuthorizationsCount > 0)
                response.DashboardDataQueryResults.Add(dischargedOmhcResultAuthorizationQr);

            DashboardDataQueryResult dischargedPrpAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Discharged PRP Authorizations",
                QueryCode = "DPRPA",
                QueryDescription = "All discharged PRP authorizations.",
                QueryRecordCount = dischargedPrpAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.Discharged,
                QueryAuthTypeNames = new string[1] { "PRP" }
            };

            if(dischargedPrpAuthorizationsCount > 0)
                response.DashboardDataQueryResults.Add(dischargedPrpAuthorizationQr);

            DashboardDataQueryResult dischargedMhAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Discharged MH Authorizations",
                QueryCode = "DMHA",
                QueryDescription = "All discharged MH authorizations.",
                QueryRecordCount = dischargedMhAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.Discharged,
                QueryAuthTypeNames = new string[1] { "MH" }
            };

            if(dischargedMhAuthorizationsCount > 0)
                response.DashboardDataQueryResults.Add(dischargedMhAuthorizationQr);
            #endregion

            //DashboardDataQueryResult dischargedOtherAuthorizationQr = new DashboardDataQueryResult()
            //{
            //    QueryName = "Discharged Other Insurance Authorizations",
            //    QueryCode = "DOIA",
            //    QueryDescription = "All discharged other insurance authorizations.",
            //    QueryRecordCount = dischargedOtherAuthorizationsCount,
            //    QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
            //    QueryStateType = QueryStateTypeEnum.Discharged
            //};
            //response.DashboardDataQueryResults.Add(dischargedOtherAuthorizationQr);

            #region Authorizations Not-Completed Query Results
            DashboardDataQueryResult notCompletedAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Total Not-Completed Authorizations",
                QueryCode = "TNCA",
                QueryDescription = "All Not Completed authorizations.",
                QueryRecordCount = notCompletedAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.NotCompleted,
                QueryAuthTypeNames = new string[4] { "SUD", "OMHC", "PRP", "MH" }
            };
            response.DashboardDataQueryResults.Add(notCompletedAuthorizationQr);

            DashboardDataQueryResult notCompletedSudAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Not Completed SUD Authorizations",
                QueryCode = "NCSUDA",
                QueryDescription = "All Not Completed SUD authorizations.",
                QueryRecordCount = notCompletedSudAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.NotCompleted,
                QueryAuthTypeNames = new string[1] { "SUD" }
            };

            if(notCompletedSudAuthorizationsCount > 0)
                response.DashboardDataQueryResults.Add(notCompletedSudAuthorizationQr);

            DashboardDataQueryResult notCompletedOmhcAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Not Completed OMHC Authorizations",
                QueryCode = "NCOMHCA",
                QueryDescription = "All Not Completed OMHC authorizations.",
                QueryRecordCount = notCompletedOmhcAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.NotCompleted,
                QueryAuthTypeNames = new string[1] { "OMHC" }
            };

            if(notCompletedOmhcAuthorizationsCount > 0)
                response.DashboardDataQueryResults.Add(notCompletedOmhcAuthorizationQr);

            DashboardDataQueryResult notCompletedPrpAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Not Completed PRP Authorizations",
                QueryCode = "NCPRPA",
                QueryDescription = "All Not Completed PRP authorizations.",
                QueryRecordCount = notCompletedPrpAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.NotCompleted,
                QueryAuthTypeNames = new string[1] { "PRP" }
            };
            if (notCompletedPrpAuthorizationsCount > 0)
                response.DashboardDataQueryResults.Add(notCompletedPrpAuthorizationQr);

            DashboardDataQueryResult notCompletedMhAuthorizationQr = new DashboardDataQueryResult()
            {
                QueryName = "Not Completed MH Authorizations",
                QueryCode = "NCMHA",
                QueryDescription = "All Not Completed MH authorizations.",
                QueryRecordCount = notCompletedMhAuthorizationsCount,
                QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
                QueryStateType = QueryStateTypeEnum.NotCompleted,
                QueryAuthTypeNames = new string[1] { "MH" }
            };
            if(notCompletedMhAuthorizationsCount > 0)
                response.DashboardDataQueryResults.Add(notCompletedMhAuthorizationQr);

            //DashboardDataQueryResult notCompletedOtherAuthorizationQr = new DashboardDataQueryResult()
            //{
            //    QueryName = "Not-Completed Other Insurance Authorizations",
            //    QueryCode = "NCOIA",
            //    QueryDescription = "All Not Completed Other insurance authorizations.",
            //    QueryRecordCount = notCompletedOtherAuthorizationsCount,
            //    QuerySubjectType = QuerySubjectTypeEnum.Authorizations,
            //    QueryStateType = QueryStateTypeEnum.NotCompleted, 
            //    QueryAuthTypeNames = new string[0]
            //};
            //response.DashboardDataQueryResults.Add(notCompletedOtherAuthorizationQr);

            #endregion

            return await Result<DashboardDataResponse>.SuccessAsync(response);

        }
    }
}