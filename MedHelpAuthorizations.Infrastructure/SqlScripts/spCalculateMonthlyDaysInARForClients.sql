SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [IntegratedServices].[spCalculateMonthlyDaysInARForClients]

	@UserId NVARCHAR = NULL
AS
BEGIN
    -- Declare variables
    DECLARE @ClientId INT;
	DECLARE @ClientLocationId INT;
    DECLARE @EndingARDate DATE;
    DECLARE @MonthlyDaysInAR INT;

    -- Cursor to iterate over each client
    DECLARE clientCursor CURSOR FOR
	SELECT DISTINCT UC.ClientId, CL.Id
	FROM [dbo].[UserClients] AS UC
	LEFT JOIN [dbo].[ClientLocations] AS CL ON CL.ClientId = UC.ClientId;

    -- Calculate MonthlyDaysInAR for the client 
	-- If we are in the beginnig of a month then
	-- we need to have the fromDate the last date of the last month
    SET @EndingARDate = DATEADD(month, DATEDIFF(month, 0, GETDATE()) - 1, 0); -- Set the FromDate as needed
	DECLARE @TotalBilledAmount DECIMAL = NULL
    DECLARE @AverageDailyCharge DECIMAL = NULL
    DECLARE @EndingAR DECIMAL = NULL
	DECLARE @EndingAROver90Days DECIMAL = NULL
    DECLARE @EndingAROver180Days DECIMAL = NULL
	DECLARE @ARTotalVisits INT = NULL
	DECLARE @ARTotalVisitsOver90Days INT = NULL
	DECLARE @ARTotalVisitsOver180Days INT = NULL
	DECLARE @PaidStatusTypes INT = 1 --Paid status types 
	DECLARE @OtherAdjudicatedClaimStatusType INT = 4 --Writte off and contractuals
	  -- Check if the temporary table already exists
    -- Declare the temporary table
    DECLARE @TempClientEndOfMonthTotals TABLE (
        ClientId INT,
		ClientLocationId INT,
        [Month] INT,
        [Year] INT,
        ARTotal DECIMAL,
        ARTotalAbove90Days DECIMAL,
        ARTotalAbove180Days DECIMAL,
        ARTotalVisits INT,
        ARTotalVisitsAbove90Days INT,
        ARTotalVisitsAbove180Days INT,
        MonthlyDaysInAR INT
    );

    -- Calculate the End Date for the month
    DECLARE @EndOfMonth DATE = EOMONTH(DATEADD(MONTH, -1, @EndingARDate))
    -- Calculate the Start Date for the previous 3 months
    DECLARE @StartDate DATE = DATEADD(MONTH, DATEDIFF(MONTH, 0, DATEADD(MONTH, -2,@EndOfMonth)), 0)

	OPEN clientCursor;
    -- Fetch the first client
	FETCH NEXT FROM clientCursor INTO @ClientId, @ClientLocationId;
    -- Loop through each client
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- Calculate the total billed amount for the previous 3 months
    SELECT @TotalBilledAmount = SUM(BilledAmount)
    FROM [IntegratedServices].[ClaimStatusBatchClaims] AS c
    WHERE ClaimBilledOn >= @StartDate AND ClaimBilledOn <= @EndOfMonth
	AND c.ClientId = @ClientId
	AND c.IsDeleted = 0 
	AND c.IsSupplanted = 0
	AND c.ClientLocationId = @ClientLocationId

    -- Calculate the number of days in the previous 3 months
    DECLARE @NumberOfDaysInPeriod INT = DATEDIFF(DAY, @StartDate, @EndOfMonth) + 1
    -- Calculate the average daily charged amount
    SET @AverageDailyCharge = @TotalBilledAmount / @NumberOfDaysInPeriod
    -- Get the ending Accounts Receivable (AR) for the month

	SELECT @EndingAR = SUM(BilledAmount), @ARTotalVisits = COUNT(DISTINCT(c.ClaimLevelMd5Hash))
	FROM [IntegratedServices].[ClaimStatusBatchClaims] as c
	LEFT JOIN [IntegratedServices].[ClaimStatusTransactions] AS t ON t.ClaimStatusBatchClaimId = c.Id
	LEFT JOIN [IntegratedServices].[ClaimLineItemStatuses] AS cst ON cst.Id = t.ClaimLineItemStatusId 
    WHERE ClaimBilledOn <= EOMONTH(DATEADD(MONTH, 0, @EndingARDate))
	AND c.ClientId = @ClientId
	AND (cst.ClaimStatusTypeId NOT IN (@OtherAdjudicatedClaimStatusType, @PaidStatusTypes))
	AND c.IsDeleted = 0 
	AND c.IsSupplanted = 0
	AND c.ClientLocationId = @ClientLocationId

	-- Get the ending Accounts Receivable (AR) for the month over 90 days
	SELECT @EndingAROver90Days = SUM(BilledAmount), @ARTotalVisitsOver90Days = COUNT(DISTINCT(c.ClaimLevelMd5Hash))
	FROM [IntegratedServices].[ClaimStatusBatchClaims] as c
	LEFT JOIN [IntegratedServices].[ClaimStatusTransactions] AS t ON t.ClaimStatusBatchClaimId = c.Id
	LEFT JOIN [IntegratedServices].[ClaimLineItemStatuses] AS cst ON cst.Id = t.ClaimLineItemStatusId 
    WHERE ClaimBilledOn < DATEADD(DAY, -90, @EndingARDate) 
	AND c.ClientId = @ClientId
	AND (cst.ClaimStatusTypeId NOT IN (@OtherAdjudicatedClaimStatusType, @PaidStatusTypes))
	AND c.IsDeleted = 0 
	AND c.IsSupplanted = 0
	AND c.ClientLocationId = @ClientLocationId

	-- Get the ending Accounts Receivable (AR) for the month over 180 days
	SELECT @EndingAROver180Days = SUM(BilledAmount), @ARTotalVisitsOver180Days = COUNT(DISTINCT(c.ClaimLevelMd5Hash))
	FROM [IntegratedServices].[ClaimStatusBatchClaims] as c
	LEFT JOIN [IntegratedServices].[ClaimStatusTransactions] AS t ON t.ClaimStatusBatchClaimId = c.Id
	LEFT JOIN [IntegratedServices].[ClaimLineItemStatuses] AS cst ON cst.Id = t.ClaimLineItemStatusId 
    WHERE ClaimBilledOn <= DATEADD(DAY, -180, @EndingARDate) 
	AND c.ClientId = @ClientId
	AND (cst.ClaimStatusTypeId NOT IN (@OtherAdjudicatedClaimStatusType, @PaidStatusTypes))
	AND c.IsDeleted = 0 
	AND c.IsSupplanted = 0
	AND c.ClientLocationId = @ClientLocationId

    --SELECT @EndingAR = ARTotal
    --FROM [dbo].[ClientEndOfMonthTotals] AS CEOM
    --WHERE EOMONTH(DATEFROMPARTS(CEOM.[Year], CEOM.[Month], 1)) = @EndOfMonth
     --Calculate the days for the component
    SET @MonthlyDaysInAR = CEILING(@EndingAR / @AverageDailyCharge)
	
        INSERT INTO @TempClientEndOfMonthTotals (ClientId, ClientLocationId, [Month], [Year], ARTotal, ARTotalAbove90Days, ARTotalAbove180Days, ARTotalVisits, ARTotalVisitsAbove90Days, ARTotalVisitsAbove180Days, MonthlyDaysInAR)
        VALUES (@ClientId, @ClientLocationId, MONTH(@EndingARDate), YEAR(@EndingARDate), COALESCE(@EndingAR,0), COALESCE(@EndingAROver90Days,0), COALESCE(@EndingAROver180Days,0), @ARTotalVisits, @ARTotalVisitsOver90Days, @ARTotalVisitsOver180Days, COALESCE(@MonthlyDaysInAR,0));
        -- Fetch the next client
        FETCH NEXT FROM clientCursor INTO @ClientId, @ClientLocationId;
    END
    CLOSE clientCursor;
    DEALLOCATE clientCursor;
	-- Select from the temporary table
    SELECT * FROM @TempClientEndOfMonthTotals;
	
	 -- Delete existing entries for the same client, month, and year combination
    DELETE FROM [dbo].[ClientEndOfMonthTotals]
    WHERE ClientId = @ClientId
	AND ClientLocationId = @ClientLocationId
    AND [Month] = MONTH(@EndingARDate)
    AND [Year] = YEAR(@EndingARDate);
    --Insert fresh entries into the ClientEndOfMonthTotal table
    INSERT INTO [dbo].[ClientEndOfMonthTotals] (ClientId, ClientLocationId, [Month], [Year], ARTotal, ARTotalAbove90Days, ARTotalAbove180Days, ARTotalVisits, ARTotalVisitsAbove90Days, ARTotalVisitsAbove180Days, MonthlyDaysInAR, CreatedOn)
    SELECT ClientId, ClientLocationId, [Month], [Year], ARTotal, ARTotalAbove90Days, ARTotalAbove180Days, ARTotalVisits, ARTotalVisitsAbove90Days, ARTotalVisitsAbove180Days, MonthlyDaysInAR, GETDATE()
    FROM @TempClientEndOfMonthTotals;
END

--EXEC  [IntegratedServices].[spCalculateMonthlyDaysInARForClients]

--select * from [ClientEndOfMonthTotals]