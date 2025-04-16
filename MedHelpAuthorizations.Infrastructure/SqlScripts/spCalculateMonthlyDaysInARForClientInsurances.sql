CREATE OR ALTER PROCEDURE [IntegratedServices].[spCalculateMonthlyDaysInARForClientInsurances]
@ClientId INT, 
@ClientLocationId INT = NULL 
AS
BEGIN

-- Declare variables
DECLARE @EndingARDate DATE = GETDATE(); -- Current date
DECLARE @EndOfMonth DATE = EOMONTH(DATEADD(MONTH, -1, @EndingARDate)); -- End of the last month
DECLARE @StartDate DATE = DATEADD(MONTH, -2, DATEADD(DAY, 1, @EndOfMonth)); -- Start date for the previous 3 months

-- Declare temporary table
DECLARE @TempClientEndOfMonthTotals TABLE (
    ClientInsuranceId INT,
    [Month] INT,
    [Year] INT,
    ARTotal DECIMAL,
    ARTotalAbove90Days DECIMAL,
    ARTotalAbove180Days DECIMAL,
    ARTotalVisits INT,
    ARTotalVisitsAbove90Days INT,
    ARTotalVisitsAbove180Days INT,
    MonthlyDaysInAR INT,
    LastMonthTotalOver90Days DECIMAL
);

-- Insert data into the temporary table
INSERT INTO @TempClientEndOfMonthTotals (
    ClientInsuranceId, [Month], [Year], ARTotal, ARTotalAbove90Days, 
    ARTotalAbove180Days, ARTotalVisits, ARTotalVisitsAbove90Days, 
    ARTotalVisitsAbove180Days, MonthlyDaysInAR, LastMonthTotalOver90Days
)
SELECT
    ci.Id AS ClientInsuranceId,
    MONTH(@EndingARDate) AS [Month],
    YEAR(@EndingARDate) AS [Year],
    SUM(CASE WHEN ClaimBilledOn <= @EndOfMonth THEN BilledAmount ELSE 0 END) AS ARTotal,
    SUM(CASE WHEN ClaimBilledOn < DATEADD(DAY, -90, @EndingARDate) THEN BilledAmount ELSE 0 END) AS ARTotalAbove90Days,
    SUM(CASE WHEN ClaimBilledOn < DATEADD(DAY, -180, @EndingARDate) THEN BilledAmount ELSE 0 END) AS ARTotalAbove180Days,
    COUNT(DISTINCT CASE WHEN ClaimBilledOn <= @EndOfMonth THEN c.ClaimLevelMd5Hash END) AS ARTotalVisits,
    COUNT(DISTINCT CASE WHEN ClaimBilledOn < DATEADD(DAY, -90, @EndingARDate) THEN c.ClaimLevelMd5Hash END) AS ARTotalVisitsAbove90Days,
    COUNT(DISTINCT CASE WHEN ClaimBilledOn < DATEADD(DAY, -180, @EndingARDate) THEN c.ClaimLevelMd5Hash END) AS ARTotalVisitsAbove180Days,
    CEILING(SUM(CASE WHEN ClaimBilledOn <= @EndOfMonth THEN BilledAmount ELSE 0 END) / 
        NULLIF(SUM(CASE WHEN ClaimBilledOn BETWEEN @StartDate AND @EndOfMonth THEN BilledAmount ELSE 0 END) / 
        (DATEDIFF(DAY, @StartDate, @EndOfMonth) + 1), 0)) AS MonthlyDaysInAR,
    (SELECT SUM(BilledAmount) 
     FROM [IntegratedServices].[ClaimStatusBatchClaims] AS c2
     LEFT JOIN [IntegratedServices].[ClaimStatusTransactions] AS t2 ON t2.ClaimStatusBatchClaimId = c2.Id
     LEFT JOIN [IntegratedServices].[ClaimLineItemStatuses] AS cst2 ON cst2.Id = t2.ClaimLineItemStatusId 
     WHERE c2.ClaimBilledOn < DATEADD(DAY, -90, DATEADD(MONTH, -1, @EndingARDate))
       AND c2.ClientInsuranceId = ci.Id
       AND cst2.ClaimStatusTypeId NOT IN (1, 4) -- Excluding Paid and Other Adjudicated Claims
       AND c2.IsDeleted = 0 
       AND c2.IsSupplanted = 0
       AND (c2.ClientLocationId = @ClientLocationId OR @ClientLocationId IS NULL)
    ) AS LastMonthTotalOver90Days
FROM 
    [dbo].[ClientInsurances] ci
LEFT JOIN 
    [IntegratedServices].[ClaimStatusBatchClaims] c ON ci.Id = c.ClientInsuranceId
LEFT JOIN 
    [IntegratedServices].[ClaimStatusTransactions] t ON t.ClaimStatusBatchClaimId = c.Id
LEFT JOIN 
    [IntegratedServices].[ClaimLineItemStatuses] cst ON cst.Id = t.ClaimLineItemStatusId
WHERE 
    ci.ClientId = @ClientId
    AND (c.ClientLocationId = @ClientLocationId OR @ClientLocationId IS NULL)
    AND c.IsDeleted = 0
    AND c.IsSupplanted = 0
    AND cst.ClaimStatusTypeId NOT IN (1, 4) -- Excluding Paid and Other Adjudicated Claims
GROUP BY 
    ci.Id;

-- Select from the temporary table
SELECT 
    ci.Id AS 'PayerId',
    ci.LookupName AS 'PayerName',
    tem.ARTotalAbove90Days AS 'Charges',
    tem.ARTotalVisitsAbove90Days AS 'Visits',
    tem.LastMonthTotalOver90Days AS 'LastMonthTotals'
FROM 
    @TempClientEndOfMonthTotals tem
INNER JOIN 
    [dbo].[ClientInsurances] ci ON ci.Id = tem.ClientInsuranceId;
END
