namespace MedHelpAuthorizations.Application.Helpers
{
    public class StoredProcedureColumnsHelper
    {
        public const string FinancialSummary = "Financial Summary";
        public const string DenialSummary = "Denial Summary";
        public const string Payment_Summary = "Payment Summary"; //EN-587
        public const string Procedure_Code_Summary = "Procedure Code Summary"; //EN-587
        public const string Summary_Of_Status = "Summary of Status"; //EN-583
        public const string Summary_Of_Payer = "Summary of Payer"; //EN-583

        public const string ClientInsuranceName = "ClientInsuranceName";
        public const string ClaimLineItemStatus = "ClaimLineItemStatus";
        public const string ClaimStatusExceptionReasonCategory = "ClaimStatusExceptionReasonCategory";
        public const string ClaimStatusExceptionReasonCategoryId = "ClaimStatusExceptionReasonCategoryId";
        public const string ProcedureCode = "ProcedureCode";
        public const string ProcedureCodeCol = "Procedure Code"; //AA-331
        public const string Quantity = "Quantity";
        public const string ChargedSum = "ChargedSum";
        public const string AllowedAmountSum = "AllowedAmountSum";
        public const string NonAllowedAmountSum = "NonAllowedAmountSum";
        public const string Non_Allowed_Paid_Amt = "Non-Allowed Paid Amt";
        public const string PaidAmountSum = "PaidAmountSum";
        public const string PaidAmount = "PaidAmount";
        public const string ClaimBilledOn = "ClaimBilledOn";
        public const string DateOfServiceFrom = "DateOfServiceFrom";
        public const string ServiceType = "ServiceType";
        public const string ServiceTypeId = "ServiceTypeId";
        public const string ClientInsuranceLookupName = "ClientInsuranceLookupName";
        public const string ClientInsuranceId = "ClientInsuranceId";
        public const string ClaimLineItemStatusId = "ClaimLineItemStatusId";
        public const string ExceptionReasonCategoryId = "ExceptionReasonCategoryId";
        public const string WeekNumberBilledOn = "WeekNumberBilledOn";
        public const string WeekYearBilledOn = "WeekYearBilledOn";
        public const string WeekNumberServiceDate = "WeekNumberServiceDate";
        public const string WeekYearServiceDate = "WeekYearServiceDate";
        public const string FirstDateOfWeekBilledOn = "FirstDateOfWeekBilledOn";
        public const string FirstDateOfWeekServiceDate = "FirstDateOfWeekServiceDate";
        public const string ClaimStatusTransactionId = "ClaimStatusTransactionId";
        public const string ClaimNumber = "ClaimNumber"; //AA-255
        public const string ClaimLevelMd5Hash = "ClaimLevelMd5Hash"; //AA-255
        public const string ServiceToBilled = "ServiceToBilled"; //AA-255
        public const string ServiceToPayment = "ServiceToPayment"; //AA-255
        public const string BilledToPayment = "BilledToPayment"; //AA-255


        public const string Last_Name = "Last Name";
        public const string First_Name = "First Name";
        public const string DOB = "DOB";
        public const string Insurance_Name = "Insurance Name";
        public const string Policy_Number = "Policy Number";
        public const string Service_Type = "Service Type";
        public const string DOS_From = "DOS From";
        public const string DOS_TO = "DOS TO"; //EN-105
        public const string CPT_Code = "CPT Code";
        public const string Billed_Amt = "Billed Amt";
        public const string Billed_On = "Billed On";
        public const string Lineitem_Status = "Lineitem Status";
        public const string Exception_Category = "Exception Category";
        public const string Exception_Reason = "Exception Reason";
        public const string Exception_Category_Count = "Exception Category Count";
        public const string Billed_Amount = "Billed Amount";
        public const string InsuranceName = "InsuranceName";
        public const string InsuranceId = "InsuranceId";
        public const string ClaimBilledDate = "ClaimBilledDate";
        public const string DateOfService = "DateOfService";
        public const string ReceivedDate = "ReceivedDate";
        public const string filterReportByDate = "filterReportByDate";
        public const string ProviderId = "ProviderId";
        public const string LocationId = "LocationId";
        public const string ProviderName = "ProviderName";
        public const string Provider = "Provider"; //EN-105
        public const string ProviderNameCol = "Provider Name"; //AA-331
        public const string LocationName = "LocationName";
        public const string LocationNameCol = "Location Name"; //AA-331
        public const string WriteOffAmountSum = "WriteOffAmountSum";
        public const string ClientinsuranceId = "ClientinsuranceId";
        public const string ClientLocationId = "ClientLocationId";
        public const string ClientProviderId = "ClientProviderId";
        public const string PatientLastName = "PatientLastName";
        public const string PatientFirstName = "PatientFirstName";
        public const string DateOfBirth = "DateOfBirth";
        public const string PolicyNumber = "PolicyNumber";
        public const string PayerName = "PayerName";
        public const string PayerId = "PayerId"; //EN-138
        public const string Payer_Name = "Payer Name"; //AA-331
        public const string OfficeClaimNumber = "OfficeClaimNumber";
        public const string OfficeClaimHash = "Office Claim #";
        public const string InsClaimHash = "Ins Claim #";
        public const string DateOfServiceTo = "DateOfServiceTo";
        public const string BilledAmount = "BilledAmount";
        public const string PayerClaimNumber = "PayerClaimNumber";
        public const string PayerLineItemControlNumber = "PayerLineItemControlNumber";
        public const string PayerLineItemControlNumberCol = "Ins Lineitem Control #";
        public const string ClaimLineItemStatusValue = "ClaimLineItemStatusValue";
        public const string ExceptionReasonCategory = "ExceptionReasonCategory";
        public const string ExceptionReasonCategoryCol = "Exception Category"; //EN-105
        public const string ExceptionReason = "ExceptionReason";
        public const string ExceptionRemark = "ExceptionRemark";
        public const string ReasonCode = "ReasonCode";
        public const string AllowedAmount = "AllowedAmount";
        public const string Allowed_Amt = "Allowed Amt"; // using in the denial sheet //EN-211
        public const string TotalAllowedAmount = "TotalAllowedAmount";
        public const string NonAllowedAmount = "NonAllowedAmount";
        public const string LineItemPaidAmount = "LineItemPaidAmount";
        public const string Line_Item_Paid_Amount = "Lineitem Paid Amt";
        public const string CheckPaidAmount = "CheckPaidAmount";
        public const string CheckDate = "CheckDate";
        public const string Check_Date = "Check Date";
        public const string CheckNumber = "CheckNumber";
        public const string CheckNumberCol = "Check Number"; //AA-343
        public const string ReasonDescription = "ReasonDescription";
        public const string RemarkCode = "RemarkCode";
        public const string RemarkDescription = "RemarkDescription";
        public const string CoinsuranceAmount = "CoinsuranceAmount";
        public const string CopayAmount = "CopayAmount";
        public const string DeductibleAmount = "DeductibleAmount";
        public const string CobAmount = "CobAmount";
        public const string PenalityAmount = "PenalityAmount";
        public const string EligibilityStatus = "EligibilityStatus";
        public const string EligibilityInsurance = "EligibilityInsurance";
        public const string EligibilityPolicyNumber = "EligibilityPolicyNumber";
        public const string EligibilityFromDate = "EligibilityFromDate";
        public const string VerifiedMemberId = "VerifiedMemberId";
        public const string CobLastVerified = "CobLastVerified";
        public const string LastActiveEligibleDateRange = "LastActiveEligibleDateRange";
        public const string PrimaryPayer = "PrimaryPayer";
        public const string PrimaryPolicyNumber = "PrimaryPolicyNumber";
        public const string BatchNumber = "BatchNumber";
        public const string AitClaimReceivedDate = "AitClaimReceivedDate";
        public const string AitClaimReceivedTime = "AitClaimReceivedTime";
        public const string TransactionDate = "TransactionDate";
        public const string TransactionTime = "TransactionTime";
        public const string PartA_EligibilityFrom = "PartA_EligibilityFrom";
        public const string PartA_EligibilityTo = "PartA_EligibilityTo";
        public const string PartA_DeductibleFrom = "PartA_DeductibleFrom";
        public const string PartA_DeductibleToDate = "PartA_DeductibleToDate";
        public const string PartA_DeductibleTo = "PartA_DeductibleTo";
        public const string PartA_RemainingDeductible = "PartA_RemainingDeductible";
        public const string PartB_EligibilityFrom = "PartB_EligibilityFrom";
        public const string PartB_EligibilityTo = "PartB_EligibilityTo";
        public const string PartB_DeductibleFrom = "PartB_DeductibleFrom";
        public const string PartB_DeductibleTo = "PartB_DeductibleTo";
        public const string PartB_RemainingDeductible = "PartB_RemainingDeductible";
        public const string OtCapYearFrom = "OtCapYearFrom";
        public const string OtCapYearTo = "OtCapYearTo";
        public const string OtCapUsedAmount = "OtCapUsedAmount";
        public const string PtCapYearFrom = "PtCapYearFrom";
        public const string PtCapYearTo = "PtCapYearTo";
        public const string PtCapUsedAmount = "PtCapUsedAmount";
        public const string CreatedOn = "CreatedOn";
        public const string LastModifiedOn = "LastModifiedOn";
        public const string Email = "Email";
        public const string PreviousClaimLineItemStatusId = "PreviousClaimLineItemStatusId";
        public const string UpdatedClaimLineItemStatusId = "UpdatedClaimLineItemStatusId";
        public const string ClientLocationName = "ClientLocationName";
        public const string ClientLocationNpi = "ClientLocationNpi";
        public const string PaymentType = "PaymentType"; //AA-324
        public const string Exception_Remark = "Exception Remark"; //EN-105
        public const string Remark_Code = "Remark Code"; //EN-105
        public const string Remark_Description = "Remark Description"; //EN-105
        public const string Reason_Code = "Reason Code"; //EN-105
        public const string Reason_Description = "Reason Description"; //EN-105
        public const string Deductible_Amt = "Deductible Amt"; //EN-105
        public const string Copay_Amt = "Copay Amt"; //EN-105
        public const string Coinsurance_Amt = "Coinsurance Amt"; //EN-105
        public const string Penality_Amt = "Penality Amt"; //EN-105
        public const string Lineitem_Paid_Amt = "Lineitem Paid Amt"; //EN-105
        public const string CheckHash = "Check #"; //EN-105
        public const string CheckPaidAmt = "Check Paid Amt"; //EN-105
        public const string Eligibility_Ins = "Eligibility Ins"; //EN-105
        public const string EligibilityPolicyHash = "Eligibility Policy #"; //EN-105
        public const string Eligibility_From_Date = "Eligibility From Date"; //EN-105
        public const string Eligibility_Status = "Eligibility Status"; //EN-105
        public const string AIT_Batch_Hash = "AIT Batch #"; //EN-105
        public const string AIT_Received_Date = "AIT Received Date"; //EN-105
        public const string AIT_Received_Time = "AIT Received Time"; //EN-105
        public const string AIT_Transaction_Date = "AIT Transaction Date"; //EN-105
        public const string AIT_Transaction_Time = "AIT Transaction Time"; //EN-105
        public const string Client_Location_Name = "Client Location Name"; //EN-105
        public const string Client_Location_Npi = "Client Location Npi"; //EN-105
                                                                         // public const string Last_History_Created_On = "LastHistoryCreatedOn"; //EN-127
        public const string ClaimStatusBatchClaimId = "ClaimStatusBatchClaimId"; //EN-127
        public const string Last_Checked_Date = "Last Checked Date";
        public const string Last_Checked_Time = "Last Checked Time";

        public const string PatientId = "PatientId";
        public const string AllowedToPaidPercentage = "AllowedToPaidPercentage"; //EN-583
        ///Age Group
        public const string AgeGroup_0_30 = "AgeGroup_0_30";
        public const string AgeGroup_31_60 = "AgeGroup_31_60";
        public const string AgeGroup_61_90 = "AgeGroup_61_90";
        public const string AgeGroup_91_120 = "AgeGroup_91_120";
        public const string AgeGroup_121_150 = "AgeGroup_121_150";
        public const string AgeGroup_151_180 = "AgeGroup_151_180";
        public const string AgeGroup_Above_181 = "AgeGroup_Above_181";
        public const string ColumnName = "ColumnName";

        public const string ClientFeeScheduleEntryId = "ClientFeeScheduleEntryId";
        public const string IsReimbursable = "IsReimbursable";


        //Cash projection
        public const string ClaimCount = "ClaimCount";
        public const string PaidTotals = "PaidTotals";
        public const string PaidTotalsCol = "Paid Totals";
        public const string RevenueTotals = "RevenueTotals";
        public const string AccountNumber = "AccountNumber";
        public const string AccountNumberCol = "Account Number"; //AA-343
        public const string ExternalId = "ExternalId";
        public const string PatientLastCommaFirst = "PatientLastCommaFirst";
        public const string ProviderLastCommaFirst = "ProviderLastCommaFirst"; //En-190
        public const string BilledDate = "BilledDate";
        public const string BilledDateCol = "Billed Date"; //AA-331
        public const string ServiceDate = "ServiceDate";
        public const string ServiceDateCol = "Service Date"; //AA-331
        public const string CashValue = "CashValue";
        public const string CashValueCol = "Cash Value";
        public const string LineItems = "Line Items"; //AA-331
        public const string RevenueTotalsCol = "Revenue Totals"; //AA-331
        public const string CollectionPercentageCol = "Collection %"; //EN-90

        //Financial Summary EN-125
        public const string ArBeginning = "ArBeginning";
        public const string ArBeginningTotals = "ArBeginningTotals";
        public const string ArBeginningVisits = "ArBeginningVisits";
        public const string Charged = "Charged";
        public const string ChargedTotals = "ChargedTotals";
        public const string ChargedVisits = "ChargedVisits";
        public const string TotalVisits = "TotalVisits";
        public const string Payment = "Payment";
        public const string PaymentTotals = "PaymentTotals";
        public const string PaymentVisits = "PaymentVisits";
        public const string PaidVisits = "PaidVisits";
        public const string Contractual = "Contractual";
        public const string ContractualTotals = "ContractualTotals";
        public const string ContractualVisits = "ContractualVisits";
        public const string WriteOff = "WriteOff";
        public const string WriteOffTotals = "WriteOffTotals";
        public const string WriteOffVisits = "WriteOffVisits";
        public const string ArEnding = "ArEnding";
        public const string ArEndingTotals = "ArEndingTotals";
        public const string ArEndingVisits = "ArEndingVisits";

        //Claims Summary EN-135
        public const string Denial = "Denial";
        public const string DenialTotals = "DenialTotals";
        public const string DenialVisits = "DenialVisits";
        public const string ZeroPayTotals = "ZeroPayTotals";
        public const string ZeroPayVisits = "ZeroPayVisits";
        public const string InProcess = "InProcess";
        public const string InProcessTotals = "InProcessTotals";
        public const string InProcessVisits = "InProcessVisits";
        public const string NotAdjudicated = "NotAdjudicated";
        public const string NotAdjudicatedTotals = "NotAdjudicatedTotals";
        public const string NotAdjudicatedVisits = "NotAdjudicatedVisits";
        public const string OtherTotals = "OtherTotals";
        public const string OtherVisits = "OtherVisits";

        public const string BilledToPaymentDateLag = "Billed To Payment Date Lag"; //EN-66
        public const string ServiceToPaymentDateLag = "Service To Payment Date Lag"; //EN-66
        public const string ServiceToBilledDateLag = "Service To Billed Date Lag"; //EN-66
        public const string InsuranceTitle = "Insurance Title"; //EN-66
        public const string PatientDOB = "PatientDOB"; //EN-66
        public const string ClientProviderName = "ClientProviderName"; //EN-66

        public const string LastMonthPaymentTotals = "LastMonthPaymentTotals";
        public const string LastMonthDenialTotals = "LastMonthDenialTotals";
        public const string LastMonthInProcessTotals = "LastMonthInProcessTotals";
        public const string LastMonthNotAdjudicatedTotals = "LastMonthNotAdjudicatedTotals";
        public const string LastMonthChargedTotals = "LastMonthChargedTotals";

        public const string CurrentMonthPaymentTotals = "CurrentMonthPaymentTotals";
        public const string CurrentMonthDenialTotals = "CurrentMonthDenialTotals";
        public const string CurrentMonthInProcessTotals = "CurrentMonthInProcessTotals";
        public const string CurrentMonthNotAdjudicatedTotals = "CurrentMonthNotAdjudicatedTotals";
        public const string CurrentMonthChargedTotals = "CurrentMonthChargedTotals";

        //claim status totals grid
        public const string PaidApproved = "Paid/Approved";
        public const string Denied = "Denied";
        //public const string Contractual = "Contractual";
        public const string Open = "Open";
        //public const string WriteOff = "Write-Off";
        public const string In_Process = "In-Process";

        public const string FailureReported = "FailureReported";
        public const string ExpiryWarningReported = "ExpiryWarningReported";
    }

    public class StoreProcedureParamsHelper
    {
        public const string ClientId = "@ClientId";
        public const string DelimitedLineItemStatusIds = "@DelimitedLineItemStatusIds";
        public const string ClientInsuranceIds = "@ClientInsuranceIds";
        public const string ClientInsuranceId = "@ClientInsuranceId";
        public const string ClientAuthTypeIds = "@ClientAuthTypeIds";
        public const string ClientProcedureCodes = "@ClientProcedureCodes";
        public const string ClientExceptionReasonCategoryIds = "@ClientExceptionReasonCategoryIds";
        public const string ReceivedFrom = "@ReceivedFrom";
        public const string ReceivedTo = "@ReceivedTo";
        public const string DateOfServiceFrom = "@DateOfServiceFrom";
        public const string DateOfServiceTo = "@DateOfServiceTo";
        public const string TransactionDateFrom = "@TransactionDateFrom";
        public const string TransactionDateTo = "@TransactionDateTo";
        public const string ClaimBilledFrom = "@ClaimBilledFrom";
        public const string ClaimBilledTo = "@ClaimBilledTo";
        public const string ClientLocationIds = "@ClientLocationIds";
        public const string FlattenedLineItemStatus = "@FlattenedLineItemStatus";
        public const string DashboardType = "@DashboardType";
        public const string ClientProviderIds = "@ClientProviderIds";
        public const string PageNumber = "@PageNumber";
        public const string RowsOfPage = "@RowsOfPage";
        public const string SortTypeCol = "@SortTypeCol";
        public const string searchString = "@searchString";
        public const string filterReportBy = "@filterReportBy";
        public const string filterDayGroupby = "@filterDayGroupby";
        public const string filterType = "@filterType";
        public const string PatientId = "@PatientId";
        public const string ClaimStatusBatchId = "@ClaimStatusBatchId";
        public const string FilterBy = "@FilterBy";
        public const string FilterForDays = "@FilterForDays"; //AA-331
        public const string ProcedureCode = "@ProcedureCode"; //AA-231
        public const string ProviderLevelId = "@ProviderLevelId";
        public const string SpecialtyId = "@SpecialtyId";
        public const string ClaimStatusType = "@ClaimStatusType";
        public const string ClaimStatusTypeStatus = "@ClaimStatusTypeStatus";
        public const string DenialStatusIds = "@DenialStatusIds";
        public const string HasProcedureDashboard = "@HasProcedureDashboard";
        public const string HasIncludeClaimStatusTransactionLineItemStatusChange = "@HasIncludeClaimStatusTransactionLineItemStatusChange";

    }
    public class StoreProcedureTitle
    {
        //public const string DefaultConnection = "DefaultConnection"; There is no more default appsettings default. DO NOT USE THIS
        public const string BilledOnDate = "BilledOnDate";
        public const string DateOfService = "DateOfService";
        public const string ReceivedDate = "ReceivedDate";
        public const string DateOfTransaction = "TransactionDate";
        public const string Denied = "Denied";
        public const string spGetClaimStatusUploadedChargeTrendsByDay = "[IntegratedServices].[spGetClaimStatusUploadedChargeTrendsByDay]";
        public const string spGetClaimStatusTrendsByDay = "[IntegratedServices].[spGetClaimStatusTrendsByDay]";
        public const string spGetInitialClaimStatusDetails = "[IntegratedServices].[spGetInitialClaimStatusDetails]";
        public const string spGetInitialClaimStatusInProcessDetails = "[IntegratedServices].[spGetInitialClaimStatusInProcessDetails]";
        public const string spGetInitialClaimStatusUploadedTotals = "[IntegratedServices].[spGetInitialClaimStatusUploadedTotals]";
        public const string spGetInitialClaimStatusUploadedTotalsTask = "[IntegratedServices].[spGetInitialClaimStatusUploadedTotalsTask]";
        public const string spGetInitialClaimStatusTotals = "[IntegratedServices].[spGetInitialClaimStatusTotals]";
        public const string spGetInitialClaimStatusInProcessTotals = "[IntegratedServices].[spGetInitialClaimStatusInProcessTotals]";
        public const string spGetClaimStatusTotalsTask = "[IntegratedServices].[spGetClaimStatusTotalsTask]";
        public const string spGetClaimStatusUploadedTotalsDashboardtask = "[IntegratedServices].[spGetClaimStatusUploadedTotalsDashboardtask]";
        public const string spGetClaimStatusInProcessTotalsDashboardtask = "[IntegratedServices].[spGetClaimStatusInProcessTotalsDashboardtask]";
        public const string spGetClaimStatusUploadedChargeProductivity = "[IntegratedServices].[spGetClaimStatusUploadedChargeProductivity]";
        public const string spGetClaimStatusProductivity = "[IntegratedServices].[spGetClaimStatusProductivity]";
        public const string spGetARAgingSummaryReport = "[IntegratedServices].[spGetARAgingSummaryReport]";
        public const string spGetARAgingReportExportDetails = "[IntegratedServices].[spGetARAgingReportExportDetails]";
        public const string spGetARAgingReportExportSummary = "[IntegratedServices].[spGetARAgingReportExportSummary]";
        public const string spGetClaimStatusUploadedTotals = "[IntegratedServices].[spGetClaimStatusUploadedTotals]";
        public const string spGetClaimStatusTransactionTotals = "[IntegratedServices].[spGetClaimStatusTransactionTotals]";
        public const string spGetClaimStatusInProcessTotals = "[IntegratedServices].[spGetClaimStatusInProcessTotals]";
        public const string spGetARAgingTotals = "[IntegratedServices].[spGetARAgingTotals]";
        public const string spGetEmployeeClaimStatusTransactionLineItemStatusChangẹDetails = "[IntegratedServices].[spGetEmployeeClaimStatusTransactionLineItemStatusChangẹDetails]";
        public const string spGetClaimStatusDateLag = "[IntegratedServices].[spGetClaimStatusDateLag]";
        public const string spGetClaimStatusRevenueTotals = "[IntegratedServices].[spGetClaimStatusRevenueTotals]";
        public const string spGetCashProjectionByDay = "[IntegratedServices].[spGetCashProjectionByDay]";
        public const string spGetCashValueForRevenueByDay = "[IntegratedServices].[spGetCashValueOfRevenue]";
        public const string spGetClientFeeScheduleEntryByClaims = "[IntegratedServices].[spGetClientFeeScheduleEntryByClaims]";
        public const string spGetProvidersDetails = "[IntegratedServices].[spGetProvidersDetails]";
        public const string spExportCashValueOfRevenue = "[IntegratedServices].[spExportCashValueOfRevenue]"; //AA-331
        public const string spGetClaimStatusReport = "[IntegratedServices].[spGetClaimStatusReport]"; //EN-66
        public const string spGetFinancialSummaryData = "[IntegratedServices].[spGetFinancialSummary]"; //EN-133
        public const string spGetClaimsSummaryData = "[IntegratedServices].[spGetClaimsSummary]"; //EN-133
        public const string spGetAvgDaysToPayByPayerData = "[IntegratedServices].[spGetAvgDaysToPayByPayer]"; //EN-133
        public const string spChargesByPayer = "[IntegratedServices].[spChargesByPayer]"; //EN-138
        public const string spPaymentsMonthly = "[IntegratedServices].[spPaymentsMonthly]"; //EN-138
        public const string spGetMonthlyDenialsTask = "[IntegratedServices].[spGetMonthlyDenialsTask]"; //EN-174
        public const string spGetAvgDaysToPayByProvider = "[IntegratedServices].[spGetAvgDaysToPayByProvider]";
        public const string spChargesByProvider = "[IntegratedServices].[spChargesByProvider]";
        public const string spGetClaimStatusTotalsDateWise = "[IntegratedServices].[spGetClaimStatusTotalsDateWise]"; //EN-219
        public const string spGetDenialsByInsuranceDateWise = "[IntegratedServices].[spGetDenialsByInsuranceDateWise]"; //EN-219
        public const string spGetClaimInProcessDateWise = "[IntegratedServices].[spGetClaimInProcessDateWise]"; //EN-219
        public const string spGetAvgAllowedAmtDateWise = "[IntegratedServices].[spGetAvgAllowedAmtDateWise]"; //EN-219
        public const string spGetClaimStatusTotals = "[IntegratedServices].[spGetClaimStatusTotals]";

        public const string spGetReimbursementByLocation = "[IntegratedServices].[spGetReimbursementByLocation]"; //EN-229
        public const string spGetReimbursementByProvider = "[IntegratedServices].[spGetReimbursementByProvider]"; //EN-229
        public const string spGetProcedureTotalsByProvider = "[IntegratedServices].[spGetProcedureTotalsByProvider]"; //EN-241
        public const string spGetProviderProcedureTotal = "[IntegratedServices].[spGetProviderProcedureTotal]";
        public const string spGetInsuranceTotalsByProvider = "[IntegratedServices].[spGetInsuranceTotalsByProvider]"; //EN-250
        public const string spGetDenialReasonsByProvider = "[IntegratedServices].[spGetDenialReasonsByProvider]"; //EN-250
        public const string spGetProviderDenialReasonTotal = "[IntegratedServices].[spGetProviderDenialReasonTotal]";
        public const string spGetProcedureReimbursementByProvider = "[IntegratedServices].[spGetProcedureReimbursementByProvider]"; //EN-254
        public const string spGetPayerReimbursementByProvider = "[IntegratedServices].[spGetPayerReimbursementByProvider]"; //EN-257
        public const string spGetDenialsByProcedure = "[IntegratedServices].[spGetDenialsByProcedure]"; //EN-289
        public const string spGetDenialsByInsurance = "[IntegratedServices].[spGetDenialsByInsurance]"; //EN-289
        public const string spGetProviderTotalsbyPayer = "[IntegratedServices].[spGetProviderTotalsbyPayer]"; //EN-278
        public const string spGetPaymentTotalsbyPayer = "[IntegratedServices].[spGetPaymentTotalsbyPayer]"; //EN-278
        public const string spGetDenialTotalsbyPayer = "[IntegratedServices].[spGetDenialTotalsbyPayer]"; //EN-278
        public const string spGetClaimInProcessReport = "[IntegratedServices].[spGetClaimInProcessReport]"; //EN-282

        public const string spGetClaimSummaryTotals = "[IntegratedServices].[spGetClaimSummaryTotals]"; //EN-339
        public const string spGetInitialClaimsSummaryTotals = "[IntegratedServices].[spGetInitialClaimsSummaryTotals]"; //EN-339
        //Provider Level Dashboard
        public const string spGetProviderTotalsByProcedure = "[IntegratedServices].[spGetProviderTotalsByProcedure]"; //EN-334
        public const string spGetProviderTotalsByProcedureCode = "[IntegratedServices].[spGetProviderTotalsByProcedureCode]"; //EN-334
        public const string spGetInsuranceTotalsByProcedureCode = "[IntegratedServices].[spGetInsuranceTotalsByProcedureCode]"; //EN-334
        public const string spGetDenialReasonsByProcedureCode = "[IntegratedServices].[spGetDenialReasonsByProcedureCode]"; //EN-334
        public const string spGetPayerReimbursementByProcedureCode = "[IntegratedServices].[spGetPayerReimbursementByProcedureCode]"; //EN-334
        public const string spChargesByProcedureCode = "[IntegratedServices].[spChargesByProcedureCode]"; //EN-334
        public const string spGetReimbursementByProcedureCode = "[IntegratedServices].[spGetReimbursementByProcedureCode]"; //EN-334
        public const string spGetAvgDaysToPayByProcedureCode = "[IntegratedServices].[spGetAvgDaysToPayByProcedureCode]";


        //Initial summary dashboard
        public const string spGetInitialClaimsSummaryQuery = "[IntegratedServices].[spGetInitialClaimsSummaryQuery]"; //EN-295
        public const string spGetInitialDenialsByInsurance = "[IntegratedServices].[spGetInitialDenialsByInsurance]"; //EN-295
        public const string spGetInitialInProcessClaims = "[IntegratedServices].[spGetInitialInProcessClaims]"; //EN-295

        //Locations dashboard
        public const string spGetProcedureTotalsByLocation = "[IntegratedServices].[spGetProcedureTotalsByLocation]"; //EN-312
        public const string spGetInsuranceTotalsByLocation = "[IntegratedServices].[spGetInsuranceTotalsByLocation]"; //EN-312
        public const string spGetDenialReasonsByLocation = "[IntegratedServices].[spGetDenialReasonsByLocation]"; //EN-312
        public const string spGetProcedureReimbursementByLocation = "[IntegratedServices].[spGetProcedureReimbursementByLocation]"; //EN-312
        public const string spGetPayerReimbursementByLocation = "[IntegratedServices].[spGetPayerReimbursementByLocation]"; //EN-312
        public const string spGetAvgDaysToPayByLocation = "[IntegratedServices].[spGetAvgDaysToPayByLocation]"; //EN-312
        public const string spGetChargesByLocation = "[IntegratedServices].[spGetChargesByLocation]"; //EN-312

        //Dashboard Tiles Data
        public const string spGetClaimStatusVisits = "[IntegratedServices].[spGetClaimStatusVisits]"; //spGetClaimStatusTilesData
        public const string spGetInitialClaimStatusVisits = "[IntegratedServices].[spGetInitialClaimStatusVisits]";
        public const string spGetLastFourYearClaims = "[IntegratedServices].[spGetLastFourYearClaims]"; //EN-231

        //Corporate dashboard
        public const string spGetCurrentMonthCharges = "[IntegratedServices].[spGetCurrentMonthCharges]"; //EN-176
        public const string spGetCurrentMonthPayments = "[IntegratedServices].[spGetCurrentMonthPayments]"; //EN-176
        public const string spGetCurrentMonthDenials = "[IntegratedServices].[spGetCurrentMonthDenials]"; //EN-176
        public const string spGetCurrentMonthEmployeeWork = "[IntegratedServices].[spGetCurrentMonthEmployeeWork]"; //EN-418
        public const string spGetCurrentAROverPercentageNintyDaysClient = "[IntegratedServices].[spGetCurrentAROverPercentageNintyDaysClient]"; //EN-419
        public const string spGetCurrentAROverPercentageNintyDaysPayer = "[IntegratedServices].[spGetCurrentAROverPercentageNintyDaysPayer]"; //EN-419 NOT IN USE
        public const string spCalculateMonthlyDaysInARForClientInsurance = "[IntegratedServices].[spCalculateMonthlyDaysInARForClientInsurances]"; //EN-419
        public const string spCalculateMonthlyDaysInAR = "[IntegratedServices].[spCalculateMonthlyDaysInAR]"; //EN-419
        public const string spGetMonthlyDaysInAR = "[IntegratedServices].[spGetMonthlyDaysInAR]";
        public const string spGetClaimRate = "[IntegratedServices].[spGetClaimRate]"; //EN-419

        public const string spGetFinancialSummaryTotals = "[IntegratedServices].[spGetFinancialSummaryTotals]"; //EN-347
        public const string spGetClaimsByProcedureSummary = "[IntegratedServices].[spGetClaimsByProcedureSummary]"; //EN-231
        public const string spCurrentSummaryExportQuery = "[IntegratedServices].[spCurrentSummaryExportQuery]"; //EN-235
        public const string spDynamicExportQuery = "[spDynamicExportQuery]"; //EN-235
        public const string spDynamicExportDashboardQuery = "[spDynamicExportDashboardQuery]"; //EN-235
        public const string spExportDenialDetailQuery = "[spExportDenialDetailQuery]"; //EN-235
        public const string spGetClaimInProcessMasterReport = "[IntegratedServices].[spGetClaimInProcessMasterReport]";

        //Executive dashboard
        public const string spGetExecutiveCurrentMonthCharges = "[IntegratedServices].[spGetExecutiveCurrentMonthCharges]"; //EN-463
        public const string spGetExecutiveCurrentMonthDenials = "[IntegratedServices].[spGetExecutiveCurrentMonthDenials]"; //EN-469
        public const string spGetExecutiveCurrentMonthPayments = "[IntegratedServices].[spGetExecutiveCurrentMonthPayments]"; //EN-470
        public const string spGetCurrentAROverPercentageNintyDaysLocation = "[IntegratedServices].[spGetCurrentAROverPercentageNintyDaysLocation]"; //EN-471
        public const string spGetExecutiveClaimRate = "[IntegratedServices].[spGetExecutiveClaimRate]"; //EN-472
        public const string spCalculateExecutiveMonthlyDaysInAR = "[IntegratedServices].[spCalculateExecutiveMonthlyDaysInAR]"; //EN-419
        public const string spGetExecutiveCurrentMonthEmployeeWork = "[IntegratedServices].[spGetExecutiveCurrentMonthEmployeeWork]";

        //Billing Kpi
        public const string spGetBillingKpiQuery = "[IntegratedServices].[spGetBillingKpiQuery]"; //EN-510

        //Procedure Dashboard Tiles
        public const string spGetClaimStatusVisitsForProcedureDashboard = "[IntegratedServices].[spGetClaimStatusVisitsForProcedureDashboard]";
        public const string spUpdateClaimStatusExceptionReasonCategory = "[IntegratedServices].[spUpdateClaimStatusExceptionReasonCategory]";

        public const string spGetTotalsByPayer = "[IntegratedServices].[spGetTotalsbyPayer]";
        public const string spGetAvgDaysToPay = "[IntegratedServices].[spGetAvgDaysToPay]";//EN-374
        public const string spGetAverageDaysToPayByProvider = "[IntegratedServices].[spGetAverageDaysToPayByProvider]";//EN-374
        public const string spUpdateOutstandingBalance = "[dbo].[spUpdateOutstandingBalance]";


    }

    public class ExportHelper
    {
        public const string Grand_Total = "Grand Total";
        public const string Exception_Reason_Category = "Exception Reason Category";
        public const string Count_of_Exception_Category = "Count of Exception Category";
        public const string Sum_of_Billed_Amount = "Sum of Billed Amount";

        public const string CurrencyType = "Currency";
        public const string TimeType = "Time";
        public const string DateType = "Date";

    }

    public class ReportConstants
    {
        public const string Quantity = "Quantity";
        public const string BilledAmount = "BilledAmt";
        public const string AllowedAmountSum = "AllowedAmountSum";
        public const string PaidAmountSum = "PaidAmountSum";

        public const string AdjudicatedVisits = "AdjudicatedVisits";
        public const string AdjudicatedTotals = "AdjudicatedTotals";

        public const string AllowedVisits = "AllowedVisits";
        public const string AllowedTotals = "AllowedTotals";

        public const string PaidVisits = "PaidVisits";
        public const string PaidTotals = "PaidTotals";

        public const string ContractualTotals = "ContractualTotals";
        public const string ContractualVisits = "ContractualVisits";

        public const string DenialVisits = "DenialVisits";
        public const string DenialTotals = "DenialTotals";

        public const string WriteOffVisits = "WriteOffVisits";
        public const string WriteOffTotals = "WriteOffTotals";

        public const string SecPs = "Sec/Ps";

        public const string TotalOpen = "TotalOpen";
        public const string OpenVisits = "OpenVisits";

        public const string TotalInProcess = "TotalInProcess";
        public const string InProcessVisits = "InProcessVisits";

        public const string TotalProcessed = "TotalProcessed";

    }

    public class CorporateDashboardConstants
    {
        public const string ClientId = "ClientId";
        public const string ClientName = "ClientName";
        public const string ClientCode = "ClientCode";
        public const string Visits = "Visits";
        public const string Charges = "Charges";
        public const string AvgCharges = "AvgCharges";
        public const string Avg_Charges = "Avg Charges";
        public const string PrvMonthCharges = "PrvMonthCharges";
        public const string PrvMonthPayments = "PrvMonthPayments";
        public const string Previous_Month = "PrvMonthCharges";
        public const string Payments = "Payments";
        public const string Avg_Allowed_Amt = "Avg Allowed Amt";
        public const string Client = "Client";
        public const string AvgAllowedAmt = "AvgAllowedAmt";
        public const string LastMonthTotals = "LastMonthTotals";
        public const string Denials = "Denials";
        public const string Avg_Denial = "Avg Denials";
        public const string AvgDenial = "AvgDenials";
        public const string EmployeeId = "EmployeeId";
        public const string UserId = "UserId";
        public const string EmployeeLastCommaFirst = "EmployeeLastCommaFirst";
        public const string Employee = "Employee";
        public const string Claims = "Claims";
        public const string Clients = "Clients";
        public const string PercentageOfAR = "% of AR";
        public const string Payer_Name = "Payer Name";
        public const string PayerId = "PayerId";
        public const string PayerName = "PayerName";
        public const string PercentageAR = "PercentageAR";
        public const string ARTotals = "ARTotals";
        public const string CurrentMonthDaysInAR = "CurrentMonthlyDaysInAR";
        public const string PreviousMonthlyDaysInAR = "PreviousMonthlyDaysInAR";
        public const string PaidOnInitialReview = "Paid on Initial Review";
        public const string PaidOnInitialReviewCol = "PaidonInitialReview";
        public const string CountOfClaims = "# of Claims";
        public const string CleanClaimPercentageRate = "Clean CLaim % Rate";
        public const string CleanClaimPercentageRateCol = "CleanCLaimRate";
        public const string CleanClaimRateKPI = "Clean Claim Rate KPI";
        public const string ClaimStatusTypeId = "claimStatusTypeId";
        public const string DenialOnInitialReview = "Denial On Initial Review";
        public const string DenialClaimPercentageRate = "Denial Claim % Rate";
        public const string DenialClaimRateKPI = "Denial Claim Rate KPI";
        public const string KPI = "KPI";
    }

    public class ExecutiveDashboardConstants
    {
        public const string ClientLocationId = "ClientLocationId";
        public const string ClientLocationName = "ClientLocationName";
        public const string Location = "Location";
    }

    public class ExportStoredProcedureColumnHelper
    {
        public const string DashBoardType = "InitialSummary";
        public const string ProcedureDashBoardType = "ProceduresDashboard";
        public const string ClaimStatusBatchClaimId = "ClaimStatusBatchClaimId";
        public const string BilledAmount = "BilledAmount";
        public const string ClaimBilledOn = "ClaimBilledOn";
        public const string OfficeClaimNumber = "OfficeClaimNumber";
        public const string DateOfServiceFrom = "DateOfServiceFrom";
        public const string DateOfServiceTo = "DateOfServiceTo";
        public const string claimStatusBatchClaims_IsDeleted = "claimStatusBatchClaims_IsDeleted";
        public const string claimStatusBatchClaims_IsSupplanted = "claimStatusBatchClaims_IsSupplanted";
        public const string claimStatusBatchClaims_LastModifiedBy = "claimStatusBatchClaims_LastModifiedBy";
        public const string claimStatusBatchClaims_LastModifiedOn = "claimStatusBatchClaims_LastModifiedOn";
        public const string AitClaimReceivedDate = "AitClaimReceivedDate";
        public const string AitClaimReceivedTime = "AitClaimReceivedTime";
        public const string claimStatusBatchClaims_PatientFirstName = "claimStatusBatchClaims_PatientFirstName";
        public const string claimStatusBatchClaims_PatientId = "claimStatusBatchClaims_PatientId";
        public const string claimStatusBatchClaims_PatientLastName = "claimStatusBatchClaims_PatientLastName";
        public const string claimStatusBatchClaims_PolicyNumber = "claimStatusBatchClaims_PolicyNumber";
        public const string claimStatusBatchClaims_ProcedureCode = "claimStatusBatchClaims_ProcedureCode";
        public const string claimStatusBatchClaims_Quantity = "claimStatusBatchClaims_Quantity";
        public const string claimStatusBatchClaims_ClientLocationId = "claimStatusBatchClaims_ClientLocationId";
        public const string claimStatusBatchClaims_ClientInsuranceId = "claimStatusBatchClaims_ClientInsuranceId";

        public const string claimStatusBatches_ClientInsuranceId = "claimStatusBatches_ClientInsuranceId";
        public const string claimStatusBatches_BatchNumber = "claimStatusBatches_BatchNumber";
        public const string claimStatusBatches_ClaimReceivedDate = "claimStatusBatches_ClaimReceivedDate";
        public const string claimStatusBatches_ReviewedOnUtc = "claimStatusBatches_ReviewedOnUtc";
        public const string claimStatusBatches_AbortedOnUtc = "claimStatusBatches_AbortedOnUtc";
        public const string claimStatusBatches_AbortedReason = "claimStatusBatches_AbortedReason";

        public const string PayerClaimNumber = "PayerClaimNumber";
        public const string PayerLineItemControlNumber = "PayerLineItemControlNumber";
        public const string ClaimLineItemStatusId = "ClaimLineItemStatusId";
        public const string ClaimLineItemStatusValue = "ClaimLineItemStatusValue";
        public const string ExceptionReason = "ExceptionReason";
        public const string ExceptionRemark = "ExceptionRemark";
        public const string ReasonCode = "ReasonCode";
        public const string LineItemPaidAmount = "LineItemPaidAmount";
        public const string TotalAllowedAmount = "TotalAllowedAmount";
        public const string NonAllowedAmount = "NonAllowedAmount";
        public const string CheckPaidAmount = "CheckPaidAmount";
        public const string CheckDate = "CheckDate";
        public const string CheckNumber = "CheckNumber";
        public const string ReasonDescription = "ReasonDescription";
        public const string RemarkCode = "RemarkCode";
        public const string RemarkDescription = "RemarkDescription";
        public const string CoinsuranceAmount = "CoinsuranceAmount";
        public const string CopayAmount = "CopayAmount";
        public const string DeductibleAmount = "DeductibleAmount";
        public const string CobAmount = "CobAmount";
        public const string PenalityAmount = "PenalityAmount";
        public const string EligibilityStatus = "EligibilityStatus";
        public const string EligibilityInsurance = "EligibilityInsurance";
        public const string EligibilityPolicyNumber = "EligibilityPolicyNumber";
        public const string EligibilityFromDate = "EligibilityFromDate";
        public const string VerifiedMemberId = "VerifiedMemberId";
        public const string CobLastVerified = "CobLastVerified";
        public const string LastActiveEligibleDateRange = "LastActiveEligibleDateRange";
        public const string PrimaryPayer = "PrimaryPayer";
        public const string PrimaryPolicyNumber = "PrimaryPolicyNumber";
        public const string TransactionDate = "TransactionDate";
        public const string TransactionTime = "TransactionTime";
        public const string PaymentType = "PaymentType";

        public const string PatientLastName = "PatientLastName";
        public const string PatientFirstName = "PatientFirstName";
        public const string PatientDOB = "PatientDOB";

        public const string ServiceType = "ServiceType";

        public const string PayerName = "PayerName";
        public const string InsuranceName = "InsuranceName";

        public const string ClaimLineItemStatus = "ClaimLineItemStatus";

        public const string ClientProviderName = "ClientProviderName";
        public const string ProViderNPI = "ProViderNPI";
        public const string ProViderSpecialtyId = "ProViderSpecialtyId";

        public const string claimStatusExceptionReasonCategory_Id = "claimStatusExceptionReasonCategory_Id";
        public const string claimStatusExceptionReasonCategory_Code = "claimStatusExceptionReasonCategory_Code";
        public const string claimStatusExceptionReasonCategory_Description = "claimStatusExceptionReasonCategory_Description";

        public const string ClientLocationName = "ClientLocationName";
        public const string ClientLocationNpi = "ClientLocationNpi";

        public const string ClientLocation_Id = "ClientLocation_Id";
        public const string ClientLocation_NPI = "ClientLocation_NPI";
        public const string ClientLocation_Name = "ClientLocation_Name";

        public const string ClaimStatusTransaction_PtCapUsedAmount = "claimStatusTransaction_PtCapUsedAmount";
        public const string ClaimStatusTransaction_PtCapYearTo = "claimStatusTransaction_PtCapYearTo";
        public const string ClaimStatusTransaction_PtCapYearFrom = "claimStatusTransaction_PtCapYearFrom";
        public const string ClaimStatusTransaction_OtCapUsedAmount = "claimStatusTransaction_OtCapUsedAmount";
        public const string ClaimStatusTransaction_OtCapYearTo = "claimStatusTransaction_OtCapYearTo";
        public const string ClaimStatusTransaction_OtCapYearFrom = "claimStatusTransaction_OtCapYearFrom";
        public const string ClaimStatusTransaction_PartB_RemainingDeductible = "claimStatusTransaction_PartB_RemainingDeductible";
        public const string ClaimStatusTransaction_PartB_DeductibleTo = "claimStatusTransaction_PartB_DeductibleTo";
        public const string ClaimStatusTransaction_PartB_DeductibleFrom = "claimStatusTransaction_PartB_DeductibleFrom";
        public const string ClaimStatusTransaction_PartB_EligibilityFrom = "claimStatusTransaction_PartB_EligibilityFrom";
        public const string ClaimStatusTransaction_PartA_RemainingDeductible = "claimStatusTransaction_PartA_RemainingDeductible";
        public const string ClaimStatusTransaction_PartA_DeductibleToDate = "claimStatusTransaction_PartA_DeductibleToDate";
        public const string ClaimStatusTransaction_PartA_DeductibleFrom = "claimStatusTransaction_PartA_DeductibleFrom";
        public const string ClaimStatusTransaction_PartA_EligibilityTo = "claimStatusTransaction_PartA_EligibilityTo";
        public const string ClaimStatusTransaction_PartA_EligibilityFrom = "claimStatusTransaction_PartA_EligibilityFrom";

        public const string ClaimLineItemStatus_ClaimStatusTypeId = "ClaimLineItemStatus_ClaimStatusTypeId";
        public const string ClaimStatusBatchClaim_ClaimStatusTransactionId = "claimStatusBatchClaim_ClaimStatusTransactionId";
        public const string LineItemChargeAmount = "LineItemChargeAmount";
        public const string ClaimStatusBatchClaim_ClaimLevelMd5Hash = "claimStatusBatchClaim_ClaimLevelMd5Hash";

        //EN-374
        public const string PaidDate = "PaidDate";
        public const string AvgDaysToPay = "Avg Days to Pay";
        public const string AvgDaysToBill = "Avg Days to Bill";
        public static string AvgDaysfromDOStoPay = "Total Days From DOS to Pay";
        public const string LastName = "Last Name";
        public const string FirstName = "First Name";
        public const string DOB = "DOB";
        public const string PolicyNumber = "Policy Number";
        public const string Payer_Name = "Payer Name";
        public const string Office_ClaimNumber = "Office Claim #";
        public const string InsClaimNumber = "Ins Claim #";
        public const string Quantity = "Quantity";
        public const string CptCode = "CPT Code";
        public const string Billed_Amount = "Billed Amt";
        public const string LineitemStatus = "Lineitem Status";
        public const string LineitemPaidAmount = "Lineitem Paid Amt";
        public const string AllowedAmount = "Allowed Amt";
        public const string Check_Number = "Check #";
        public const string Check_PaidAmount = "Check Paid Amt";
        public const string AitBatchNumber = "AIT Batch #";
        public const string AitReceivedDate = "AIT Received Date";
        public const string AitReceivedTime = "AIT Received Time";
        public const string AitTransactionDate = "AIT Transaction Date";
        public const string AitTransactionTime = "AIT Transaction Time";
        public const string Provider = "Provider";
        public const string Client_LocationName = "Client Location Name";
        public const string Client_LocationNpi = "Client Location Npi";
        public const string Payment_Type = "Payment Type";
        public const string ClaimStatusBatchId = "ClaimStatusBatchId";
        public const string ClaimLevelMd5Hash = "ClaimLevelMd5Hash";
        public const string ProviderId = "ProviderId";
        public const string ProviderNPI = "ProviderNPI";
        public const string ProviderName = "ProviderName";
        public const string PreviousClaimLineItemStatusId = "PreviousClaimLineItemStatusId";
        public const string UpdatedClaimLineItemStatusId = "UpdatedClaimLineItemStatusId";
        public const string LastCheckedDate = "LastCheckedDate";
        public const string LastCheckedTime = "LastCheckedTime";

    }

    public class KpiDashboardColumnsHelper
    {
        public const string Id = "Id";
        public const string ClientId = "ClientId";

        //Billing KPI Goals
        public const string DailyClaimCountGoal = "DailyClaimCountGoal";
        public const string CleanClaimRateGoal = "CleanClaimRateGoal";
        public const string DenialRateGoal = "DenialRateGoal";
        public const string ChargesGoal = "ChargesGoal";
        public const string CollectionPercentageGoal = "CollectionPercentageGoal";
        public const string CashCollectionsGoal = "CashCollectionsGoal";
        public const string Over90DaysGoal = "Over90DaysGoal";
        public const string AverageDaysInReceivablesGoal = "AverageDaysInReceivablesGoal";
        public const string BDRateGoal = "BDRateGoal";
        public const string VisitsGoal = "VisitsGoal";
        public const string DaysInARGoal = "DaysInARGoal";

        //Billing KPI Values
        public const string DailyClaimCountValue = "DailyClaimCountValue";
        public const string CleanClaimRateValue = "CleanClaimRateValue";
        public const string DenialRateValue = "DenialRateValue";
        public const string ChargesValue = "ChargesValue";
        public const string CollectionPercentageValue = "CollectionPercentageValue";
        public const string CashCollectionsValue = "CashCollectionsValue";
        public const string Over90DaysValue = "Over90DaysValue";
        public const string AverageDaysInReceivablesValue = "AverageDaysInReceivablesValue";
        public const string BDRateValue = "BDRateValue";
        public const string VisitsValue = "VisitsValue";
        public const string DaysInARValue = "DaysInARValue";
    }
}
