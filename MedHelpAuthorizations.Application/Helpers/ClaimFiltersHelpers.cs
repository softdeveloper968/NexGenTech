using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllByClientId;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetRpaAssignedInsurances;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetProcedureCodes;
using MedHelpAuthorizations.Application.Features.Providers.Queries.GetAllProviders;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using EnumExtensions = MedHelpAuthorizations.Shared.Extensions.EnumExtensions;

namespace MedHelpAuthorizations.Application.Helpers
{
    public class ClaimFiltersHelpers
    {

        public const string _dateFormat = "MM/dd/yyyy";
        public const string _dateFormat_02 = "MM/dd/yy";
        public const string _timeFormat = "HH:mm:ss";

        public const string other_flattenedLineItemStatus = "Other";
        //public const string zero_pay_flattenedLineItemStatus = "Zero Pay";
        public const string denied_flattenedLineItemStatus = "Denied";
        public const string paid_approved_flattenedLineItemStatus = "Paid/Approved";
        public const string not_adgudicated_flattenedLineItemStatus = "Not-Adjudicated";
        public const string reviewed_flattenedLineItemStatus = "Reviewed";
        public const string export_All_flattenedLineItemStatus = "Export All";
        public const string inProcess_flattenedLineItemStatus = "InProcess";

        public static List<int> ConvertStringToList(string requestedStr, bool excludeFirstIndex = false)
        {
            if (string.IsNullOrEmpty(requestedStr.Trim())) return null;

            List<int> numbersList = requestedStr.Split(',')
                                    .Select(int.Parse)
                                    .ToList();
            if (excludeFirstIndex)
            {
                numbersList = numbersList.Where(n => n != 0)
                              .ToList();
            }
            return numbersList;
        }
        public static List<ClaimStatusExceptionReasonCategoryEnum> ConvertStringToExceptionReasonCategoryEnumList(string requestedStr, bool excludeFirstIndex = false)
        {
            if (string.IsNullOrEmpty(requestedStr.Trim())) return new List<ClaimStatusExceptionReasonCategoryEnum>();

            List<ClaimStatusExceptionReasonCategoryEnum> numbersList = requestedStr.Split(',').ToList().ConvertAll(delegate (string x)
            {
                return (ClaimStatusExceptionReasonCategoryEnum)Enum.Parse(typeof(ClaimStatusExceptionReasonCategoryEnum), x);
            });

            //var numbersList = requestedStr.Split(',')
            //                       .Select(int.Parse)
            //                       .Select(z => (ClaimStatusExceptionReasonCategoryEnum?)z)
            //                       .ToList();
            //var numbersList = requestedStr.Split(',')
            //                  .Select(x => (ClaimStatusExceptionReasonCategoryEnum?)Enum
            //                  .Parse(typeof(ClaimStatusExceptionReasonCategoryEnum?), x.Trim())).ToList();

            if (excludeFirstIndex)
            {
                numbersList = numbersList.Where(n => n != 0)
                              .ToList();
            }
            return numbersList;
        }
        public static string GetProcedureCode(string procedureCode)
        {
            return procedureCode == "All Procedures" ? string.Empty : procedureCode;
        }
        public static List<string> ConvertProcedureCodesToList(string requestedStr)
        {
            if (string.IsNullOrEmpty(requestedStr.Trim())) return null;

            var codes = requestedStr.Split(',').Where(code => code.Trim() != string.Empty && code != "All Procedures").ToList();
            return codes;
        }

        public static string ToRPAInsuranceString(GetRpaAssignedInsurancesResponse insurance)
        {
            return insurance is null ? string.Empty : $"{insurance.Name}";
        }

        public static string ToClaimStatusExceptionReasonCategoryString(ClaimStatusExceptionReasonCategoryEnum? exceptionReason)
        {
            return exceptionReason.HasValue ? $"{exceptionReason.GetDescription()}" : string.Empty;
        }
        public static string ToClaimProcedureString(GetClaimStatusClientProcedureCodeResponse code)
        {
            return $"{code.ProcedureCode}";
        }

        public static string ToInsuranceString(GetAllClientInsurancesByClientIdResponse insurance)
        {
            return insurance is null ? string.Empty : $"{insurance.Name}";
        }

        public static string ToLocationString(GetClientLocationsByClientIdResponse location)
        {
            return location is null ? string.Empty : $"{location.Name}";
        }

        public static string ToProviderString(GetAllProvidersResponse provider)
        {
            return provider is null ? string.Empty : GetProviderName(provider.FirstName, provider.LastName);
        }

        public static string GetProviderName(string firstName, string lastName)
        {
            return $"{firstName ?? string.Empty} {lastName ?? string.Empty}";
        }
        public static string GetMoneyConverter(decimal value)
        {
            return value.ToString("C", CultureInfo.GetCultureInfo("en-US"));
        }
        public static string GetRPAInsuranceOptionsAsString(List<GetRpaAssignedInsurancesResponse> insurances)
        {
            if (insurances is not null && insurances.Any(z => z.Id > 0))
                return string.Join(",", insurances.Select(l => l.Id));
            else return string.Empty;
        }
        public static string GetExceptionReasonOptionsAsString(List<ClaimStatusExceptionReasonCategoryEnum?> exceptionReason)
        {
            if (exceptionReason is not null && exceptionReason.Any())
                return string.Join(",", exceptionReason.Select(z => (int)z.Value));
            else return string.Empty;
        }
        public static string GetProviderOptionsAsString(List<GetAllProvidersResponse> provider)
        {
            if (provider is not null && provider.Any(z => z.Id > 0))
                return string.Join(",", provider.Where(l => l.Id > 0).Select(z => z.Id).ToList());
            else return string.Empty;
        }
        public static string GetProviderOptionsAsString(List<int> provider)
        {
            return string.Join(",", provider.Where(l => l > 0).ToList());
        }
        public static string GetLocationOptionsAsString(List<GetClientLocationsByClientIdResponse> location)
        {
            if (location is not null && location.Any(z => z.Id > 0))
                return string.Join(",", location.Where(l => l.Id > 0).Select(z => z.Id).ToList());
            else return string.Empty;
        }

        public static string GetServiceTypeOptionOptionsAsString(List<int> serviceTypeOption)
        {
            if (serviceTypeOption is not null && serviceTypeOption.Any(z => z > 0))
                return string.Join(",", serviceTypeOption);
            else return string.Empty;
        }
        public static string GetProcedureCodeOptionsAsString(List<GetClaimStatusClientProcedureCodeResponse> code)
        {
            if (code is not null && code.Any(z => z.ProcedureCode != "All Procedures"))
                return string.Join(",", code.Select(x => x.ProcedureCode).ToList());
            else return string.Empty;
        }
        public static string ToServiceTypeAsString(int value, Dictionary<int, string> AvailableAuthTypes)
        {
            return AvailableAuthTypes.Where(z => z.Key == value)?.FirstOrDefault().Value ?? string.Empty;
        }
        public static string ToClientServiceTypeAsString(int value, Dictionary<int, string> clientAvailableAuthTypes)
        {
            return clientAvailableAuthTypes.Where(z => z.Key == value)?.FirstOrDefault().Value ?? string.Empty;
        }
        public static string ToClientInsuranceAsString(int value, Dictionary<int, string> clientInsurances)
        {
            return clientInsurances.Where(z => z.Key == value)?.FirstOrDefault().Value ?? string.Empty;
        }
        public static bool HasColumn(DataRowCollection rows, string ColumnName)
        {
            bool exist = false;
            foreach (DataRow row in rows)
            {
                if (row["ColumnName"].ToString() == ColumnName)
                    return true;
                else continue;
            }
            return exist;
        }
        public static string GetWorkstationLocationAndProviderOptionsAsString(List<int?> location)
        {
            if (location.Any(z => z.Value > 0))
                return string.Join(",", location);
            else return string.Empty;
        }

        public static (decimal Sum, int Quantity) GetAgingReportChartTotalsByDayGroup(List<ARAgingData> _ARAgingData, int startDays = 0, int endDays = 30, string filterReportBy = StoreProcedureTitle.BilledOnDate, bool lastDate = false)
        {
            switch (filterReportBy)
            {
                case StoreProcedureTitle.BilledOnDate:
                    {
                        _ARAgingData = _ARAgingData
                                  .OrderBy(x => x.ClaimBilledOn)
                                  .Where(x =>
                                        lastDate ? ((DateTime.Now - Convert.ToDateTime(x.ClaimBilledOn)).Days >= startDays) :
                                        (DateTime.Now - Convert.ToDateTime(x.ClaimBilledOn)).Days >= startDays
                                        && (DateTime.Now - Convert.ToDateTime(x.ClaimBilledOn)).Days <= endDays).ToList();
                        break;
                    }
                case StoreProcedureTitle.DateOfService:
                    {
                        _ARAgingData = _ARAgingData
                                    .OrderBy(x => x.DateOfServiceFrom)
                                    .Where(x => lastDate ? ((DateTime.Now - Convert.ToDateTime(x.DateOfServiceFrom)).Days >= startDays) : ((DateTime.Now - Convert.ToDateTime(x.DateOfServiceFrom)).Days >= startDays && (DateTime.Now - Convert.ToDateTime(x.DateOfServiceFrom)).Days <= endDays)).ToList();
                        break;
                    }
            }

            decimal sum = _ARAgingData?.Sum(sum => sum.ChargedSum) ?? 0.0m;
            int quantity = _ARAgingData?.Sum(quantity => Convert.ToInt32(quantity.Quantity)) ?? 0;

            return (sum, quantity);
        }
        public static ARAgingDataResponse CreateARAgingChartDataByAgeDayGroup(ARAgingDataResponse _agingReportTotalsByDayGroup, string filterReportBy)
        {
            //0-30
            (decimal sum, int quantity) = ClaimFiltersHelpers.GetAgingReportChartTotalsByDayGroup(_agingReportTotalsByDayGroup.ARAgingData, 0, 30, filterReportBy);
            _agingReportTotalsByDayGroup.AgeGroup_0_30 = sum;
            _agingReportTotalsByDayGroup.AgeGroup_0_30_Qty = quantity;

            //31-60
            (sum, quantity) = ClaimFiltersHelpers.GetAgingReportChartTotalsByDayGroup(_agingReportTotalsByDayGroup.ARAgingData, 31, 60, filterReportBy);
            _agingReportTotalsByDayGroup.AgeGroup_31_60 = sum;
            _agingReportTotalsByDayGroup.AgeGroup_31_60_Qty = quantity;

            //61-90
            (sum, quantity) = ClaimFiltersHelpers.GetAgingReportChartTotalsByDayGroup(_agingReportTotalsByDayGroup.ARAgingData, 61, 90, filterReportBy);
            _agingReportTotalsByDayGroup.AgeGroup_61_90 = sum;
            _agingReportTotalsByDayGroup.AgeGroup_61_90_Qty = quantity;

            //91-120
            (sum, quantity) = ClaimFiltersHelpers.GetAgingReportChartTotalsByDayGroup(_agingReportTotalsByDayGroup.ARAgingData, 91, 120, filterReportBy);
            _agingReportTotalsByDayGroup.AgeGroup_91_120 = sum;
            _agingReportTotalsByDayGroup.AgeGroup_91_120_Qty = quantity;

            //121-150
            (sum, quantity) = ClaimFiltersHelpers.GetAgingReportChartTotalsByDayGroup(_agingReportTotalsByDayGroup.ARAgingData, 121, 150, filterReportBy);
            _agingReportTotalsByDayGroup.AgeGroup_121_150 = sum;
            _agingReportTotalsByDayGroup.AgeGroup_121_150_Qty = quantity;

            //151-180
            (sum, quantity) = ClaimFiltersHelpers.GetAgingReportChartTotalsByDayGroup(_agingReportTotalsByDayGroup.ARAgingData, 151, 180, filterReportBy);
            _agingReportTotalsByDayGroup.AgeGroup_151_180 = sum;
            _agingReportTotalsByDayGroup.AgeGroup_151_180_Qty = quantity;

            //above 181
            (sum, quantity) = ClaimFiltersHelpers.GetAgingReportChartTotalsByDayGroup(_agingReportTotalsByDayGroup.ARAgingData, 181, 0, filterReportBy, lastDate: true);
            _agingReportTotalsByDayGroup.AgeGroup_181_plus = sum;
            _agingReportTotalsByDayGroup.AgeGroup_181_plus_Qty = quantity;

            return _agingReportTotalsByDayGroup;
        }

        public static string GetDenialCategories(IReadOnlyCollection<ClaimLineItemStatusEnum> deniedClaimLineItemStatuses)
        {
            return string.Join(",", deniedClaimLineItemStatuses
                                                .Select(status => (int)status)
                                                .ToList());
        }

        /// <summary>
        /// Parses a string value into a ClaimStatusExceptionReasonCategoryEnum enumeration. //En-81
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <returns>The parsed ClaimStatusExceptionReasonCategoryEnum value.</returns>
        public static ClaimStatusExceptionReasonCategoryEnum ParseClaimStatusExceptionReasonCategoryEnum(string value)
        {
            foreach (ClaimStatusExceptionReasonCategoryEnum enumValue in Enum.GetValues(typeof(ClaimStatusExceptionReasonCategoryEnum)))
            {
                var enumDescription = EnumExtensions.GetDescription(enumValue);
                if (enumValue.ToString() == value || enumDescription == value)
                {
                    return enumValue;
                }
            }
            return 0;
        }

        /// <summary>
        /// Parses a string value into a ClaimLineItemStatusEnum enumeration.  //En-81
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <returns>The parsed ClaimLineItemStatusEnum value.</returns>
        public static ClaimLineItemStatusEnum ParseClaimLineItemStatusEnum(string value)
        {
            foreach (ClaimLineItemStatusEnum enumValue in Enum.GetValues(typeof(ClaimLineItemStatusEnum)))
            {
                var enumDescription = EnumExtensions.GetDescription(enumValue);
                if (enumValue.ToString() == value || enumDescription == value)
                {
                    return enumValue;
                }
            }
            return 0;
        }

        public static List<ClaimLineItemStatusEnum> GetClaimLineItemStatusByGroupedStatus(string groupedStatuses)
        {
            try
            {
                List<ClaimLineItemStatusEnum> selectedClaimStatuses = new();
                List<string> categories = groupedStatuses.Split(',').ToList() ?? new();
                if (categories.Any())
                {
                    foreach (string category in categories)
                    {
                        switch (category)
                        {
                            case ClaimFiltersHelpers.other_flattenedLineItemStatus:
                                selectedClaimStatuses.AddRange(ReadOnlyObjects.ReadOnlyObjects.OtherClaimLineItemStatuses.ToList());
                                break;
                            //case ClaimFiltersHelpers.zero_pay_flattenedLineItemStatus:
                            //    selectedClaimStatuses.AddRange(ReadOnlyObjects.ReadOnlyObjects.ZeroPayBundledClaimLineItemStatuses.ToList());
                            //    break;
                            case ClaimFiltersHelpers.denied_flattenedLineItemStatus:
                                selectedClaimStatuses.AddRange(ReadOnlyObjects.ReadOnlyObjects.DeniedClaimLineItemStatuses.ToList());
                                break;
                            case ClaimFiltersHelpers.paid_approved_flattenedLineItemStatus:
                                selectedClaimStatuses.AddRange(ReadOnlyObjects.ReadOnlyObjects.AllPaidClaimLineItemStatuses.ToList());
                                break;
                            case ClaimFiltersHelpers.not_adgudicated_flattenedLineItemStatus:
                                selectedClaimStatuses.AddRange(ReadOnlyObjects.ReadOnlyObjects.OpenClaimLineItemStatuses.ToList());
                                break;
                            case ClaimFiltersHelpers.reviewed_flattenedLineItemStatus:
                                selectedClaimStatuses.AddRange(ReadOnlyObjects.ReadOnlyObjects.ErrorClaimLineItemStatuses.ToList());
                                break;
                            case ClaimFiltersHelpers.export_All_flattenedLineItemStatus:
                                selectedClaimStatuses.AddRange(ReadOnlyObjects.ReadOnlyObjects.AllClaimLineItemStatuses.ToList());
                                break;
                            default:
                                selectedClaimStatuses.Add((ClaimLineItemStatusEnum)(int.Parse(category)));
                                break;
                        }
                    }
                }
                return selectedClaimStatuses.Distinct().ToList() ?? new List<ClaimLineItemStatusEnum>();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public static ClaimStatusTypeEnum? GetClaimStatusTypesFromFlattenedName(string flattenedLineItemStatus, string deliminatedStatusIds)
        {
            switch (flattenedLineItemStatus)
            {
                case "Other":
                    return ClaimStatusTypeEnum.OtherOpenClaimStatusType;

                case "Paid/Approved":
                case "Paid/Approved/ZeroPay":
                    return ClaimStatusTypeEnum.PaidClaimStatusType;

                case "Not-Adjudicated":
                    return ClaimStatusTypeEnum.OtherAdjudicatedClaimStatusType;

                case "Denied":
                    return ClaimStatusTypeEnum.DeniedClaimStatusType;
                default:
                    return null;
            }
        }


        public static ClaimLineItemStatus GetClaimLineItemStatus(ClaimLineItemStatusEnum statusEnum)
        {
            return statusEnum switch
            {
                ClaimLineItemStatusEnum.Unknown => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Unknown, "Unknown", "Unknown", 1, 10, 30, 100, 1, ClaimStatusTypeEnum.OtherOpenClaimStatusType),
                ClaimLineItemStatusEnum.Paid => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Paid, "Paid", "Paid", 0, 0, 0, 0, 23, ClaimStatusTypeEnum.PaidClaimStatusType),
                ClaimLineItemStatusEnum.Approved => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Approved, "Approved", "Approved", 1, 14, 4, 14, 18, ClaimStatusTypeEnum.PaidClaimStatusType),// initial Wait of 6
                ClaimLineItemStatusEnum.Rejected => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Rejected, "Rejected", "Rejected", 0, 0, 0, 0, 9, ClaimStatusTypeEnum.DeniedClaimStatusType),
                ClaimLineItemStatusEnum.Voided => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Voided, "Voided", "Voided", 0, 0, 0, 0, 10, ClaimStatusTypeEnum.OtherOpenClaimStatusType),
                ClaimLineItemStatusEnum.Received => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Received, "Received", "Received", 1, 14, 4, 14, 15, ClaimStatusTypeEnum.OpenClaimStatusType),// Has not been used yet. Candidate for replacement.
                ClaimLineItemStatusEnum.NotAdjudicated => new ClaimLineItemStatus(ClaimLineItemStatusEnum.NotAdjudicated, "NotAdjudicated", "Not-Adjudicated", 1, 20, 4, 20, 11, ClaimStatusTypeEnum.OpenClaimStatusType),// initial Wait of 2
                ClaimLineItemStatusEnum.Denied => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Denied, "Denied", "Denied", 10, 60, 3, 6, 17, ClaimStatusTypeEnum.DeniedClaimStatusType),
                ClaimLineItemStatusEnum.Pended => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Pended, "Pended", "Pended", 2, 20, 4, 20, 14, ClaimStatusTypeEnum.OpenClaimStatusType),// initial Wait of 5
                ClaimLineItemStatusEnum.UnMatchedProcedureCode => new ClaimLineItemStatus(ClaimLineItemStatusEnum.UnMatchedProcedureCode, "UnMatchedProcedureCode", "UnMatched-ProcedureCode", 10, 14, 2, 100, 8, ClaimStatusTypeEnum.DeniedClaimStatusType),
                ClaimLineItemStatusEnum.Error => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Error, "Error", "Error / Exception", 0, 10, 4, 21, 3, null),
                ClaimLineItemStatusEnum.Unavailable => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Unavailable, "Unavailable", "Unavailable For Review", 1, 20, 4, 20, 7, ClaimStatusTypeEnum.OpenClaimStatusType),
                ClaimLineItemStatusEnum.MemberNotFound => new ClaimLineItemStatus(ClaimLineItemStatusEnum.MemberNotFound, "MemberNotFound", "Member Not Found", 5, 16, 2, 100, 5, ClaimStatusTypeEnum.DeniedClaimStatusType),
                ClaimLineItemStatusEnum.Ignored => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Ignored, "Ignored", "Ignored", 0, 0, 0, 0, 4, ClaimStatusTypeEnum.DeniedClaimStatusType),
                ClaimLineItemStatusEnum.ZeroPay => new ClaimLineItemStatus(ClaimLineItemStatusEnum.ZeroPay, "ZeroPay", "Zero Pay", 0, 0, 0, 0, 24, ClaimStatusTypeEnum.PaidClaimStatusType),
                ClaimLineItemStatusEnum.BundledFqhc => new ClaimLineItemStatus(ClaimLineItemStatusEnum.BundledFqhc, "BundledFqhc", "Bundled Fqhc", 0, 0, 0, 0, 21, ClaimStatusTypeEnum.PaidClaimStatusType),
                ClaimLineItemStatusEnum.NeedsReview => new ClaimLineItemStatus(ClaimLineItemStatusEnum.NeedsReview, "NeedsReview", "Needs Review", 0, 0, 0, 0, 12, ClaimStatusTypeEnum.OpenClaimStatusType),
                ClaimLineItemStatusEnum.TransientError => new ClaimLineItemStatus(ClaimLineItemStatusEnum.TransientError, "TransientError", "Transient Error", 0, 99, 10, 99, 2, null),
                ClaimLineItemStatusEnum.CallPayer => new ClaimLineItemStatus(ClaimLineItemStatusEnum.CallPayer, "CallPayer", "Call Payer", 0, 0, 0, 0, 13, ClaimStatusTypeEnum.DeniedClaimStatusType),
                ClaimLineItemStatusEnum.Returned => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Returned, "Returned", "Returned", 0, 0, 0, 0, 19, ClaimStatusTypeEnum.DeniedClaimStatusType),
                ClaimLineItemStatusEnum.Writeoff => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Writeoff, "Writeoff", "Write-off", 0, 0, 0, 0, 20, ClaimStatusTypeEnum.OtherAdjudicatedClaimStatusType),
                ClaimLineItemStatusEnum.Rebilled => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Rebilled, "Rebilled", "Rebilled", 1, 14, 4, 14, 4, ClaimStatusTypeEnum.OpenClaimStatusType),// Does not appear to be used. A candidate for replacement.
                ClaimLineItemStatusEnum.Contractual => new ClaimLineItemStatus(ClaimLineItemStatusEnum.Contractual, "Contractual", "Contractual", 0, 0, 0, 0, 25, ClaimStatusTypeEnum.OtherAdjudicatedClaimStatusType),// EN-97
                ClaimLineItemStatusEnum.NotOnFile => new ClaimLineItemStatus(ClaimLineItemStatusEnum.NotOnFile, "NotOnFile", "Not On File", 0, 0, 0, 0, 22, ClaimStatusTypeEnum.DeniedClaimStatusType),
                ClaimLineItemStatusEnum.MemberNotEligible => new ClaimLineItemStatus(ClaimLineItemStatusEnum.MemberNotEligible, "MemberNotEligible", "Member Not Eligible", 0, 0, 0, 0, 16, ClaimStatusTypeEnum.DeniedClaimStatusType),
                ClaimLineItemStatusEnum.RetryMemberNotFound => new ClaimLineItemStatus(ClaimLineItemStatusEnum.RetryMemberNotFound, "RetryMemberNotFound", "Retry Member Not Found", 0, 16, 0, 0, 6, ClaimStatusTypeEnum.OpenClaimStatusType),
                _ => throw new ArgumentOutOfRangeException(nameof(statusEnum), statusEnum, null),
            };
        }

        public static ClaimStatusTypeEnum? GetClaimStatusTypeEnum(string claimStatus, out string claimStatusTypeValue)
        {
            claimStatusTypeValue = string.Empty;

            if (claimStatus == "WriteOff")
            {
                claimStatusTypeValue = claimStatus;
                return ClaimStatusTypeEnum.DeniedClaimStatusType;
            }
            else if (claimStatus == "Open")
            {
                claimStatusTypeValue = claimStatus;
                return null;
            }
            else if (claimStatus == "Contractual")
            {
                claimStatusTypeValue = claimStatus;
                return null;
            }
            else if (claimStatus == "Paid/Approved")
            {
                claimStatusTypeValue = string.Empty;
                return ClaimStatusTypeEnum.PaidClaimStatusType; ;
            }
            else if (claimStatus == "Denied")
            {
                claimStatusTypeValue = string.Empty;
                return ClaimStatusTypeEnum.DeniedClaimStatusType; ;
            }
            else
            {
                Enum.TryParse(claimStatus, out ClaimLineItemStatusEnum claimLineItem);
                return GetClaimLineItemStatus(claimLineItem).ClaimStatusTypeId;
            }
        }

    }
}
