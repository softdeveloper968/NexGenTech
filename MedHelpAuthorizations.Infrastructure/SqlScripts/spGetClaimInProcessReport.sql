/****** Object:  StoredProcedure [IntegratedServices].[spGetClaimInProcessDateWise]    Script Date: 02/02/2024 11:06:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER  PROCEDURE [IntegratedServices].[spGetClaimInProcessReport]
@ClientId int
    ,@DelimitedLineItemStatusIds nvarchar(MAX) = null
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
SELECT	PatPer.LastName as 'PatientLastName'
        ,PatPer.FirstName as 'PatientFirstName'
        ,c1.PolicyNumber as 'PolicyNumber'
        ,auth.Name as 'ServiceType'
        ,i.LookupName as 'PayerName'
        ,c1.ClaimNumber as 'OfficeClaimNumber'
		,c1.ClaimBilledOn as 'ClaimBilledOn'
        ,t.ClaimNumber as 'PayerClaimNumber'
        ,c1.ProcedureCode as 'ProcedureCode'
        ,c1.DateOfServiceFrom as 'DateOfServiceFrom'
        ,c1.DateOfServiceTo as 'DateOfServiceTo'
        ,b.BatchNumber as 'BatchNumber'
        ,CONVERT(date, c1.CreatedOn) as 'AitClaimReceivedDate'
        ,CONVERT(time, c1.CreatedOn) as 'AitClaimReceivedTime'
        ,CONCAT(Per.LastName, ', ', Per.FirstName) as ClientProviderName
        ,t.PaymentType as 'PaymentType'
		,clientLoc.Name as 'ClientLocationName'
		,clientLoc.Npi as  'ClientLocationNpi'
FROM [IntegratedServices].ClaimStatusBatchClaims as c1
JOIN(
		SELECT * FROM [IntegratedServices].[fnGetClaimLevelGroups](
					@ClientId, 
					null, 
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
	JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id
	LEFT JOIN [IntegratedServices].ClaimLineItemStatuses as s ON t.ClaimLineItemStatusId = s.Id
	LEFT JOIN [dbo].[AuthTypes] as auth ON b.AuthTypeId = auth.Id
	LEFT JOIN [dbo].Providers as Prv ON c1.ClientProviderId = Prv.Id
	LEFT JOIN [dbo].ClientLocations as ClientLoc On c1.ClientLocationId = ClientLoc.Id
	LEFT JOIN [dbo].Patients as Pat ON c1.PatientId = Pat.Id
	LEFT JOIN [dbo].Persons as PatPer On Pat.PersonId = PatPer.Id
	LEFT JOIN [dbo].Persons as Per On Prv.PersonId = Per.Id -- person for the provider
WHERE C1.ClientId = @ClientId
	AND (c1.ClaimStatusTransactionId is NULL OR t.ClaimLineItemStatusId in (10,17))	
		---Multi Select Filter----
		And (b.ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '') OR (t.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ','))))
		And (ClientLoc.Id in (SELECT convert(int, value)
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
		AND ((c1.PatientId = @PatientId) OR (@PatientId IS NULL) OR (@PatientId = 0))
	    AND ((c1.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))	
		GROUP BY 
		 PatPer.LastName
		,PatPer.FirstName
		,c1.PolicyNumber
		,auth.Name
		,i.LookupName 
		,c1.ClaimNumber
		,t.ClaimNumber
		,c1.ProcedureCode
		,c1.DateOfServiceFrom
		,c1.DateOfServiceTo
		,b.BatchNumber
		,CONVERT(date, c1.CreatedOn)
		,CONVERT(time, c1.CreatedOn)
		,CONCAT(Per.LastName, ', ', Per.FirstName)
		,t.PaymentType
		,clientLoc.Name
		,clientLoc.Npi
		,c1.ClaimBilledOn
	END
