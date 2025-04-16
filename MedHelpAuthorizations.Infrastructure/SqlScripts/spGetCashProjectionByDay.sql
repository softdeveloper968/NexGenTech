SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Summary:
-- This stored procedure retrieves cash projection data by day for a specific client.
-- It calculates various totals and aggregates the results for reporting purposes.

-- Parameters:
-- @ClientId: The ID of the client for which cash projection is calculated.

-- AA-326

CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetCashProjectionByDay]
    @ClientId int
    ,@FilterForDays Int = 7
    ,@ClientLocationIds VARCHAR(MAX) = NULL
    ,@ClientProviderIds VARCHAR(MAX) = Null
AS
BEGIN
    -- Calculate cash projection data
    SELECT 
    Count(c1.ClaimLevelMD5Hash) AS ClaimCount, -- Count of claims
        t.CheckNumber, -- Check number
        t.CheckDate, -- Check date
        SUM(LineItemPaidAmount) AS PaidTotals, -- Total paid amount
        SUM(c1.BilledAmount) AS RevenueTotals, -- Total billed revenue
        c1.ClaimLevelMd5Hash, -- Claim level MD5 hash
        i.Id AS ClientInsuranceId, -- Client insurance ID
        i.Name AS PayerName, -- Payer name
        p.AccountNumber, -- Account number
        p.ExternalId, -- External ID
        per.LastName + ', ' + per.FirstName AS PatientLastCommaFirst -- Patient's last name and first name
    FROM IntegratedServices.ClaimStatusBatchClaims AS c1 
	JOIN(
		SELECT * FROM [IntegratedServices].[fnGetClaimLevelGroups](
					@ClientId, 
					null, 
					null, 
					null, 
					null, 
					null, 
					null, 
					null, 
					null, 
					null, 
					@ClientProviderIds, 
					@ClientLocationIds, 
					null, 
					null, 
					null, 
					null, 
					null, 
					null
				)
	) as c2 ON c1.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
    RIGHT JOIN IntegratedServices.ClaimStatusTransactions as t on c1.ClaimStatusTransactionId = t.Id
    JOIN Patients AS p ON p.Id = c1.PatientId
    JOIN Persons AS per ON per.Id = p.PersonId
    JOIN ClientInsurances AS i ON c1.ClientInsuranceId = i.Id
    --JOIN ClientLocations as loc ON loc.Id = c1.ClientLocationId
    --JOIN Providers AS prv ON prv.Id = c1.ClientProviderId

    WHERE c1.ClientId = @ClientId -- The client ID
    AND c1.IsSupplanted = 0
    AND c1.IsDeleted = 0
    AND t.CheckDate < DATEADD(DAY, @FilterForDays, GETDATE()) 
    AND t.CheckDate >= GETDATE()
    AND t.ClaimLineItemStatusId in (1, 2) 
    AND t.LineItemPaidAmount IS NOT NULL -- AND t.CheckNumber is not null
    And (c1.ClientLocationId in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
    And (c1.ClientProviderId in (SELECT convert(int, value)
        FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
        
    GROUP BY c1.ClaimLevelMd5Hash, t.CheckNumber, t.CheckDate, p.AccountNumber, p.ExternalId, per.LastName + ', ' + per.FirstName, i.Id, i.Name
    ORDER BY t.CheckDate DESC
    -- Having COUNT(c1.ClaimLevelMd5Hash) > 1
END
GO