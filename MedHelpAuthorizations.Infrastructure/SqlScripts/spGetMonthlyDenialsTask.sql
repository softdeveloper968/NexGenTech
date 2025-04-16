SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetMonthlyDenialsTask]
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

AS
BEGIN


Declare @denialTypeStatusTypeId int = 2;

SELECT	[claimStatusBatchClaim].ClaimLevelMd5Hash as 'ClaimLevelMd5Hash'
        ,[claimLineItemStatus].Id as 'ClaimLineItemStatusId'
		,[claimStatusExceptionReasonCategory].id as 'ExceptionReasonCategoryId'
		,SUM([claimStatusBatchClaim].BilledAmount) AS 'ChargedSum'
		,COUNT(DISTINCT([claimStatusBatchClaim].ClaimLevelMd5Hash)) AS 'Quantity'
		,CONVERT(date, [claimStatusBatchClaim].ClaimBilledOn) as 'BilledOnDate'
		,CONVERT(date, [claimStatusBatchClaim].DateOfServiceFrom) as 'DateOfServiceFrom'

FROM [IntegratedServices].ClaimStatusBatchClaims as [claimStatusBatchClaim]
JOIN(
		SELECT * FROM [IntegratedServices].[fnGetClaimLevelGroups](
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
	) as [claimLevelGrouping] ON [claimStatusBatchClaim].ClaimLevelMd5Hash = [claimLevelGrouping].ClaimLevelMd5Hash
	LEFT JOIN [IntegratedServices].ClaimStatusTransactions as [claimStatusTransaction] ON [claimStatusTransaction].ClaimStatusBatchClaimId = [claimStatusBatchClaim].Id
	JOIN [IntegratedServices].ClaimStatusBatches as [claimStatusBatch] ON [claimStatusBatchClaim].ClaimStatusBatchId = [claimStatusBatch].Id
	JOIN [dbo].ClientInsurances as [clientInsurance] ON [claimStatusBatch].ClientInsuranceId = [clientInsurance].Id
	LEFT JOIN [IntegratedServices].ClaimLineItemStatuses as [claimLineItemStatus] ON [claimStatusTransaction].ClaimLineItemStatusId = [claimLineItemStatus].Id
	LEFT JOIN [dbo].Providers as [provider] ON [claimStatusBatchClaim].ClientProviderId = [provider].Id
	LEFT JOIN [dbo].Persons as [person] On [provider].PersonId = [person].Id
	LEFT JOIN [dbo].ClientLocations as [clientLocation] On [claimStatusBatchClaim].ClientLocationId = [clientLocation].Id
	LEFT JOIN [IntegratedServices].ClaimStatusExceptionReasonCategories as [claimStatusExceptionReasonCategory] ON [claimStatusTransaction].ClaimStatusExceptionReasonCategoryId = [claimStatusExceptionReasonCategory].Id

WHERE [claimStatusBatchClaim].ClientId = @ClientId
	AND (
		    ([claimLineItemStatus].ClaimStatusTypeId = @denialTypeStatusTypeId AND ([claimStatusTransaction].WriteoffAmount IS NULL OR [claimStatusTransaction].WriteoffAmount = 0) AND (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))
			OR ([claimStatusTransaction].ClaimLineItemStatusId IN (SELECT convert(int, value) FROM string_split(@DelimitedLineItemStatusIds, ',')))AND (@DelimitedLineItemStatusIds is not null OR @DelimitedLineItemStatusIds != '')
		)

		---Multi Select Filter----
		And ([claimStatusBatch].ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '') OR ([claimStatusTransaction].ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ','))))
		And ([clientLocation].Id in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
		And ([provider].Id in (SELECT convert(int, value)
			FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
		And ([claimStatusBatch].AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And ([claimStatusBatchClaim].ProcedureCode in (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))

		AND [claimStatusBatchClaim].IsDeleted = 0
		AND [claimStatusBatchClaim].IsSupplanted = 0
		AND (([claimStatusBatchClaim].PatientId = @PatientId) OR (@PatientId IS NULL))
	    AND (([claimStatusBatchClaim].ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))	

GROUP BY 
				[claimLineItemStatus].Id
				, [claimStatusExceptionReasonCategory].Id
				, [claimStatusBatchClaim].ClaimLevelMD5hash
				,CONVERT(date, [claimStatusBatchClaim].ClaimBilledOn)
				,CONVERT(date, [claimStatusBatchClaim].DateOfServiceFrom)


END