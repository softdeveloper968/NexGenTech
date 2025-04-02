DECLARE

	@ClientId int = 3

    ,@DelimitedLineItemStatusIds nvarchar(MAX) = NULL

	,@ReceivedFrom DateTime = NULL

	,@ReceivedTo DateTime = NULL

	,@DateOfServiceFrom DateTime = NULL

	,@DateOfServiceTo DateTime = NULL

	,@TransactionDateFrom DateTime = NULL

	,@TransactionDateTo DateTime = NULL

	,@ClaimBilledFrom DateTime = '01-26-2024'

	,@ClaimBilledTo DateTime = '04-25-2024'

	,@ClientProviderIds nvarchar(MAX) = NULL

    ,@ClientLocationIds nvarchar(MAX) = NULL

	,@ClientInsuranceIds nvarchar(MAX)= null

	,@ClientExceptionReasonCategoryIds nvarchar(MAX)= null

	,@ClientAuthTypeIds nvarchar(MAX)= null

	,@ClientProcedureCodes nvarchar(MAX)= null

    ,@PatientId int = NULL

	,@ClaimStatusBatchId int = NULL


Select 

	COUNT(DISTINCT(c1.ClaimLevelMd5Hash)) as 'Quantity'

	,c1.ClaimLevelMd5Hash as 'Key'

	, c1.BilledAmount as 'BilledAmt'

	--,SUM(c1.BilledAmount) as 'Sum'

FROM IntegratedServices.ClaimStatusBatchClaims as c1 

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

	) as c2 ON c1.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash -- AND c1.ClaimBilledOn = c2.LatestClaimBilledOn  

	LEFT JOIN IntegratedServices.ClaimStatusTransactions as t on c1.Id = t.ClaimStatusBatchClaimId

	JOIN [IntegratedServices].ClaimStatusBatches as b ON c1.ClaimStatusBatchId = b.Id

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

		And (c1.ClientProviderId in (SELECT convert(int, value)

			FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))

		And (b.AuthTypeId in (SELECT convert(int, value)

			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))

		And (c1.ProcedureCode in (SELECT convert(nvarchar(16), value)

			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))

		AND c1.IsDeleted = 0

		AND c1.IsSupplanted = 0
	--AND c1.ClaimLevelMd5Hash in ('0x422300364213A631A98FD91C15F54D26', '0x72F642031F9EB0369E9C1F84A68473E2', '0xAA814D449171BEC7570F201E1DC0720C')
 
	GROUP BY c1.ClaimLevelMd5Hash--, c1.BilledAmount

--	END

--GO