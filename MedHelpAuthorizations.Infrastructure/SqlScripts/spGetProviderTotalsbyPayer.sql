SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [IntegratedServices].[spGetProviderTotalsbyPayer]
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
		--i.Id as 'ClientInsuranceId'
		--,i.LookupName as 'ClientInsuranceName'
		SUM(c1.BilledAmount) as 'ChargedSum'
		,Per.LastName + ',' + per.FirstName as 'ClientProviderName'
		,c1.ClientProviderId as 'ClientProviderId'
		,COUNT(DISTINCT(c1.CLaimLevelMd5Hash)) as 'Quantity'


FROM [IntegratedServices].ClaimStatusBatchClaims as c1
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
	) as c2 ON c1.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
	LEFT JOIN [IntegratedServices].ClaimStatusTransactions as t ON t.ClaimStatusBatchClaimId = c1.Id
	JOIN [IntegratedServices].ClaimStatusBatches as b ON c1.ClaimStatusBatchId = b.Id
	LEFT JOIN [dbo].Providers as Prv ON c1.ClientProviderId = Prv.Id
	LEFT JOIN [dbo].Persons as Per On Prv.PersonId = Per.Id
	LEFT JOIN [dbo].ClientInsurances as i on i.Id = c1.ClientInsuranceId

WHERE C1.ClientId = @ClientId
	AND (t.ClaimLineItemStatusId IN (SELECT convert(int, value)
			FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))
		---Multi Select Filter----
		And (b.ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '') OR (t.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ','))))
		And (c1.ClientLocationId in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
		And (Prv.Id in (SELECT convert(int, value)
			FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
		And (b.AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And (c1.ProcedureCode in (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))
		--AND ((c1.CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
		--AND ((c1.CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
		--AND ((c1.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
		--AND ((c1.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo IS NULL)
		--AND ((c1.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		--AND ((c1.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
		--AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) >= @TransactionDateFrom) OR @TransactionDateFrom is NULL )
		--AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) <= @TransactionDateTo) OR @TransactionDateTo is NULL)
		AND c1.IsDeleted = 0
		AND c1.IsSupplanted = 0
		AND ((c1.PatientId = @PatientId) OR (@PatientId IS NULL))
	    AND ((c1.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))	
		GROUP BY
		--i.Id
		--, i.LookupName
		--, ProcedureCode
		(Per.LastName + ',' + per.FirstName)
		, c1.ClientProviderId
	END
