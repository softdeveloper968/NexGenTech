SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [IntegratedServices].[spExportCashValueOfRevenue]
    @ClientId int
    ,@FilterBy VARCHAR(MAX) = 'ServiceDate' -- 'BilledDate'
    ,@ClientInsuranceId Int = null
    ,@FilterForDays Int = -7
    ,@ClientLocationIds VARCHAR(MAX) = null
    ,@ClientProviderIds VARCHAR(MAX) = null
AS
BEGIN
IF @FilterBy = 'ServiceDate'
    SELECT CONVERT(DATE, c1.DateOfServiceFrom) as 'ServiceDate', c1.ProcedureCode as 'ProcedureCode'
    ,i.Name AS PayerName
    ,SUM(
        CASE
            WHEN ClientFeeScheduleEntryID IS NOT NULL THEN
                    cf.AllowedAmount
                ELSE 
                    c1.BilledAmount * 0.8
            END
    ) AS 'CashValue'
    ,per.LastName as 'PatientLastName'
    ,per.FirstName as 'PatientFirstName'
    ,loc.Name as 'LocationName'
    ,CONCAT(prvPer.LastName, ',', prvPer.FirstName) as 'ProviderName'

    FROM IntegratedServices.ClaimStatusBatchClaims AS c1 
    LEFT JOIN ClientFeeScheduleEntries AS cf ON cf.Id = c1.ClientFeeScheduleEntryId
    JOIN ClientInsurances AS i ON c1.ClientInsuranceId = i.Id
    JOIN Patients AS p ON p.Id = c1.PatientId
    JOIN Persons AS per ON per.Id = p.PersonId
    JOIN Providers AS prv ON prv.Id = c1.ClientProviderId
    JOIN Persons as prvPer ON prvPer.Id = prv.PersonId
    JOIN ClientLocations as loc ON loc.Id = c1.ClientLocationId

    WHERE c1.ClientId = @ClientId
    AND c1.IsSupplanted = 0
    AND c1.IsDeleted = 0
    AND c1.DateOfServiceFrom > DATEADD(dd, @FilterForDays, DATEDIFF(dd, 0, GETDATE())) 
    AND c1.DateOfServiceFrom <= DATEADD(ms, -2, DATEADD(dd, 1, DATEDIFF(dd, 0, GetDate())))
    And (loc.Id in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
    And (prv.Id in (SELECT convert(int, value)
        FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
    AND (c1.ClientInsuranceId = @ClientInsuranceId OR (@ClientInsuranceId is NULL OR @ClientInsuranceId = 0))
    GROUP BY i.Name, CONVERT(DATE, c1.DateOfServiceFrom), c1.ProcedureCode, per.LastName, per.FirstName, loc.Name, CONCAT(prvPer.LastName, ',', prvPer.FirstName)
    ORDER BY CONVERT(DATE, c1.DateOfServiceFrom) DESC
    -- Having COUNT(c1.ClaimLevelMd5Hash) > 1
ELSE
    SELECT CONVERT(DATE, c1.ClaimBilledOn) as 'BilledDate', c1.ProcedureCode as 'ProcedureCode'
    ,i.Name AS PayerName
    ,SUM(
        CASE
            WHEN ClientFeeScheduleEntryID IS NOT NULL THEN
                    cf.AllowedAmount
                ELSE 
                    c1.BilledAmount * 0.8
            END
    ) AS 'CashValue'
    ,per.LastName as 'PatientLastName'
    ,per.FirstName as 'PatientFirstName'
    ,loc.Name as 'LocationName'
    ,CONCAT(prvPer.LastName, ',', prvPer.FirstName) as 'ProviderName'

    FROM IntegratedServices.ClaimStatusBatchClaims AS c1 
    LEFT JOIN ClientFeeScheduleEntries AS cf ON cf.Id = c1.ClientFeeScheduleEntryId
    JOIN ClientInsurances AS i ON c1.ClientInsuranceId = i.Id
    JOIN Patients AS p ON p.Id = c1.PatientId
    JOIN Persons AS per ON per.Id = p.PersonId
    JOIN Providers AS prv ON prv.Id = c1.ClientProviderId
    JOIN Persons as prvPer ON prvPer.Id = prv.PersonId
    JOIN ClientLocations as loc ON loc.Id = c1.ClientLocationId

    WHERE c1.ClientId = @ClientId
    AND c1.IsSupplanted = 0
    AND c1.IsDeleted = 0
    AND c1.ClaimBilledOn > DATEADD(dd, @FilterForDays, DATEDIFF(dd, 0, GETDATE()))
    AND c1.ClaimBilledOn <= DATEADD(ms, -2, DATEADD(dd, 1, DATEDIFF(dd, 0, GetDate())))
    And (loc.Id in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
    And (prv.Id in (SELECT convert(int, value)
        FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
    AND (c1.ClientInsuranceId = @ClientInsuranceId OR (@ClientInsuranceId is NULL OR @ClientInsuranceId = 0))
    GROUP BY i.Name, CONVERT(DATE, c1.ClaimBilledOn), c1.ProcedureCode, per.LastName, per.FirstName, loc.Name, CONCAT(prvPer.LastName, ',', prvPer.FirstName)
    ORDER BY CONVERT(DATE, c1.ClaimBilledOn) DESC
    -- Having COUNT(c1.ClaimLevelMd5Hash) > 1
END
GO



