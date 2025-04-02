SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetClaimSummaryTotals]
@ClientId int
    ,@DelimitedLineItemStatusIds nvarchar(MAX) = NULL
	,@ReceivedFrom DateTime = NULL
	,@ReceivedTo DateTime = NULL
	,@DateOfServiceFrom DateTime = NULL
	,@DateOfServiceTo DateTime = NULL
	,@TransactionDateFrom DateTime = NULL
	,@TransactionDateTo DateTime = NULL
	,@ClaimBilledFrom DateTime = NULL
	,@ClaimBilledTo DateTime = NULL
	,@ClientProviderIds nvarchar(MAX) = NULL
    ,@ClientLocationIds nvarchar(MAX) = NULL
	,@ClientInsuranceIds nvarchar(MAX)= null
	,@ClientExceptionReasonCategoryIds nvarchar(MAX)= null
	,@ClientAuthTypeIds nvarchar(MAX)= null
	,@ClientProcedureCodes nvarchar(MAX)= null
    ,@PatientId int = NULL
	,@ClaimStatusBatchId int = NULL
WITH RECOMPILE  
AS
BEGIN
SELECT 
    -- Main query columns
	C1.ChargedTotals,
	C1.TotalVisits,
    C1.PaymentTotals,
    C1.PaymentVisits,
    C1.DenialTotals,
    C1.DenialVisits,
    C1.InProcessTotals,
    C1.InProcessVisits,
    C1.NotAdjudicatedTotals,
    C1.NotAdjudicatedVisits,

	-- Last months query columns
	C2.ChargedTotals AS 'LastMonthChargedTotals',
	C2.TotalVisits AS 'LastMonthVisitTotals',
    C2.PaymentTotals AS 'LastMonthPaymentTotals',
    C2.DenialTotals AS 'LastMonthDenialTotals',
    C2.InProcessTotals AS 'LastMonthInProcessTotals',
    C2.NotAdjudicatedTotals AS 'LastMonthNotAdjudicatedTotals',

	-- Current months query columns
	C3.ChargedTotals AS 'CurrentMonthChargedTotals',
	C3.TotalVisits AS 'CurrentMonthVisitTotals',
    C3.PaymentTotals AS 'CurrentMonthPaymentTotals',
    C3.DenialTotals AS 'CurrentMonthDenialTotals',
    C3.InProcessTotals AS 'CurrentMonthInProcessTotals',
    C3.NotAdjudicatedTotals AS 'CurrentMonthNotAdjudicatedTotals'

FROM 
(
    SELECT  
		ChargedTotals,
		TotalVisits,
        PaymentTotals,
		PaymentVisits,
        DenialTotals,
		DenialVisits,
        InProcessTotals,
		InProcessVisits,
        NotAdjudicatedTotals,
		NotAdjudicatedVisits
    FROM 
        [IntegratedServices].[fnGetClaimSummary](
           @ClientId, 
					@DelimitedLineItemStatusIds, 
					@ReceivedFrom,
					@ReceivedTo, 
					@DateOfServiceFrom, 
					@DateOfServiceTo, 
					@TransactionDateFrom, 
					@TransactionDateTo, 
					@ClaimBilledFrom, 
					@ClaimBilledTo, 
					@ClientProviderIds, 
					@ClientLocationIds, 
					@ClientInsuranceIds, 
					@ClientExceptionReasonCategoryIds, 
					@ClientAuthTypeIds, 
					@ClientProcedureCodes, 
					@PatientId, 
					@ClaimStatusBatchId 
        )
) AS C1
CROSS JOIN (
    SELECT  
		ChargedTotals,
		TotalVisits,
        PaymentTotals,
        DenialTotals,
        InProcessTotals,
        NotAdjudicatedTotals
    FROM 
        [IntegratedServices].[fnGetMonthlyClaimSummary](
            @ClientId, 
            @DelimitedLineItemStatusIds, 
            DATEADD(MONTH, DATEDIFF(MONTH, 0, @ReceivedTo) - 1, 0),  
            EOMONTH(@ReceivedTo, -1),
            DATEADD(MONTH, DATEDIFF(MONTH, 0, @DateOfServiceTo) - 1, 0),  
            EOMONTH(@DateOfServiceTo, -1),
            DATEADD(MONTH, DATEDIFF(MONTH, 0, @TransactionDateTo) - 1, 0),  
            EOMONTH(@TransactionDateTo, -1),
            DATEADD(MONTH, DATEDIFF(MONTH, 0, @ClaimBilledTo) - 1, 0),  
            EOMONTH(@ClaimBilledTo, -1),
            @ClientProviderIds, 
            @ClientLocationIds, 
            @ClientInsuranceIds, 
            @ClientExceptionReasonCategoryIds, 
            @ClientAuthTypeIds, 
            @ClientProcedureCodes, 
            @PatientId, 
            @ClaimStatusBatchId 
        )
) AS c2
CROSS JOIN (
    SELECT  
		ChargedTotals,
		TotalVisits,
        PaymentTotals,
        DenialTotals,
        InProcessTotals,
        NotAdjudicatedTotals
    FROM 
        [IntegratedServices].[fnGetMonthlyClaimSummary](
            @ClientId, 
            @DelimitedLineItemStatusIds, 
            DATEFROMPARTS(YEAR(@ReceivedTo), MONTH(@ReceivedTo), 1),  
            EOMONTH(@ReceivedTo),
            DATEFROMPARTS(YEAR(@DateOfServiceTo), MONTH(@DateOfServiceTo), 1),  
            EOMONTH(@DateOfServiceTo),
            DATEFROMPARTS(YEAR(@TransactionDateTo), MONTH(@TransactionDateTo), 1),  
            EOMONTH(@TransactionDateTo),
            DATEFROMPARTS(YEAR(@ClaimBilledTo), MONTH(@ClaimBilledTo), 1),  
            EOMONTH(@ClaimBilledTo),
            @ClientProviderIds, 
            @ClientLocationIds, 
            @ClientInsuranceIds, 
            @ClientExceptionReasonCategoryIds, 
            @ClientAuthTypeIds, 
            @ClientProcedureCodes, 
            @PatientId, 
            @ClaimStatusBatchId 
        )
) AS c3
END



