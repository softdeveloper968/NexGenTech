SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE or ALTER PROCEDURE [IntegratedServices].[spGetInitialClaimStatusInProcessDetails]
	@ClientId int
	,@DelimitedLineItemStatusIds nvarchar(MAX) = NULL    
	--,@ClientInsuranceId int = NULL 
	--,@AuthTypeId int = NULL 
	--,@ProcedureCode nvarchar(24) = NULL 
	--,@ExceptionReasonCategory int = NULL 
	,@ClientInsuranceIds nvarchar(MAX)= null
	,@ClientExceptionReasonCategoryIds nvarchar(MAX)= null
	,@ClientAuthTypeIds nvarchar(MAX)= null
	,@ClientProcedureCodes nvarchar(MAX)= null
	,@ReceivedFrom DateTime = NULL 
	,@ReceivedTo DateTime = NULL 
	,@DateOfServiceFrom DateTime = NULL 
	,@DateOfServiceTo DateTime = NULL 
	,@TransactionDateFrom DateTime = NULL 
	,@TransactionDateTo DateTime = NULL 
	,@ClaimBilledFrom DateTime = NULL 
	,@ClaimBilledTo DateTime = NULL 
    ,@PatientId int = NULL
    ,@ClientLocationIds nvarchar(MAX)= null
	,@FlattenedLineItemStatus nvarchar(MAX) = NULL

AS
BEGIN
   SELECT c1.PatientLastName,
		c1.PatientFirstName,
		c1.DateOfBirth,
		i.LookupName as 'PayerName',
		a.Name as 'ServiceType',
		c1.PolicyNumber,
		c1.ClaimNumber as 'OfficeClaimNumber',
		c1.DateOfServiceFrom,
		c1.DateOfServiceTo,
		c1.ProcedureCode,
		c1.ClaimBilledOn,
		c1.BilledAmount,
		b.BatchNumber,
		c1.CreatedOn,
        PrvPer.LastName + ',' + PrvPer.FirstName as 'ProviderName',
        cl.Name as 'ClientLocationName',
        cl.Npi as 'ClientLocationNpi',
		t.PaymentType
	FROM [IntegratedServices].ClaimStatusBatchClaims as c1
	JOIN(
			SELECT min(Id) as 'MinId', EntryMd5Hash as 'HashKey' --, min(CreatedOn) as 'ReceivedDate'
			FROM  [IntegratedServices].ClaimStatusBatchClaims 
			GROUP BY EntryMd5Hash
		) as c2 ON c1.Id = c2.MinId

 JOIN [IntegratedServices].ClaimStatusBatches as b  ON c1.ClaimStatusBatchId = b.Id
 JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id 
 JOIN [dbo].AuthTypes as a ON b.AuthTypeId = a.Id 
 JOIN [IntegratedServices].ClaimStatusTransactions as t ON t.ClaimStatusBatchClaimId = c1.Id
 JOIN Providers as Prv ON c1.ClientProviderId = Prv.Id
 JOIN Persons as PrvPer ON Prv.PersonId = PrvPer.Id
 JOIN ClientLocations as cl On c1.ClientLocationId = cl.Id
 WHERE C1.ClientId = @ClientId
	AND (c1.ClaimStatusTransactionId is NULL OR t.ClaimLineItemStatusId in (10,17, null))	

		---Multi Select Filter----
		And (b.ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '') OR (t.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ','))))
		And (cl.Id in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
		--And (Prv.Id in (SELECT convert(int, value)
		--	FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
		And (b.AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And (c1.ProcedureCode in (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))

		AND ((c1.PatientId = @PatientId) OR (@PatientId IS NULL) OR (@PatientId = 0))
	    --AND ((c1.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))
END
GO
