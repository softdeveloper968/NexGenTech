CREATE OR ALTER   PROCEDURE spGetAutoCalculateFeeScheduleData
    @clientInsuranceIds NVARCHAR(MAX),
    @clientFeeScheduleStartDate DATETIME,
    @clientFeeScheduleEndDate DATETIME,
    @specialtyIds NVARCHAR(MAX) = NULL,
    @providerLevelIds NVARCHAR(MAX) = NULL,
	@clientId int
AS
BEGIN

    DECLARE @StartDate DATE
    DECLARE @EndDate DATE

    -- Calculate the start date 90 days prior to the clientFeeScheduleStartDate
    SET @StartDate = DATEADD(DAY, -90, @clientFeeScheduleStartDate)

    SELECT  DISTINCT
        AVG(ct.LineItemPaidAmount) as 'AverageLineItemPaidAmount'
		,TRIM(cpt.Code) as 'ProcedureCode'
		, AVG(csbc.BilledAmount) as 'AvergaeBilledAmount',
		SUM(csbc.BilledAmount) as 'ChargedSum'
		, cpt.Id as 'ClientCptCodeId'
    FROM 
        IntegratedServices.ClaimStatusBatchClaims csbc
     JOIN 
        IntegratedServices.ClaimStatusTransactions ct ON ct.Id = csbc.ClaimStatusTransactionId
    JOIN 
        dbo.Providers cp ON csbc.ClientProviderId = cp.Id
    JOIN 
       dbo.ClientInsurances cii ON csbc.ClientInsuranceId = cii.Id
	JOIN
	   dbo.ClientCptCodes as cpt ON csbc.ClientCptCodeId = cpt.Id
    WHERE 
         ct.ClaimLineItemStatusId IN (1, 2, 15, 14)
		 AND csbc.ClientId = @clientId
		AND csbc.DateOfServiceFrom <= @StartDate
        AND csbc.DateOfServiceTo <= @clientFeeScheduleEndDate
		AND (
                 (
                    @ClientInsuranceIds IS NOT NULL 
                    AND @ClientInsuranceIds != '' 
                    AND csbc.[ClientInsuranceId] IN (SELECT CONVERT(INT, value) FROM STRING_SPLIT(@ClientInsuranceIds, ','))
                   )
                OR 
                (
                    @ClientInsuranceIds IS NULL OR @ClientInsuranceIds = ''
                )
            )
	   And (cp.ProviderLevelId in (SELECT convert(int, value)
										FROM string_split(@providerLevelIds, ',')) OR (@providerLevelIds is null OR @providerLevelIds = ''))

		 And (cp.SpecialtyId in (SELECT convert(int, value)
										FROM string_split(@specialtyIds, ',')) OR (@specialtyIds is null OR @specialtyIds = ''))
	GROUP BY 
		  cpt.Id,
    TRIM(cpt.Code);

END
