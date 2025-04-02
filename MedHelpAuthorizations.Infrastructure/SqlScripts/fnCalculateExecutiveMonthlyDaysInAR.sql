SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER FUNCTION [IntegratedServices].[fnCalculateExecutiveMonthlyDaysInAR](
	@ClientId INT = NULL
	,@FromDate DATE = NULL
	)
RETURNS int
AS
BEGIN
    DECLARE @TotalBilledAmount DECIMAL = NULL
    DECLARE @AverageDailyCharge DECIMAL = NULL
    DECLARE @EndingAR DECIMAL = NULL
    DECLARE @MonthlyDaysInAR INT = NULL

	DECLARE @PaidStatusTypes INT = 1 --Paid status types 
	DECLARE @OtherAdjudicatedClaimStatusType INT = 4 --Writte off and contractuals

    -- Calculate the End Date for the month
    DECLARE @EndOfMonth DATE = EOMONTH(DATEADD(MONTH, -1, @FromDate))

    -- Calculate the Start Date for the previous 3 months
    DECLARE @StartDate DATE = DATEADD(MONTH, DATEDIFF(MONTH, 0, DATEADD(MONTH, -2,@EndOfMonth)), 0)
--	DECLARE @tempUserAssignedClients TABLE (
--	ClientId int, 
--	INDEX idx_tbl_ClientId (ClientId)
--)

	DECLARE @clientLocations TABLE (ClientLocationId int)

	INSERT INTO @clientLocations
	SELECT cl.Id
	FROM [dbo].ClientLocations AS cl
	WHERE cl.ClientId = @clientId

    -- Calculate the total billed amount for the previous 3 months
    SELECT @TotalBilledAmount = SUM(BilledAmount)
    FROM [IntegratedServices].[ClaimStatusBatchClaims] AS c
    RIGHT JOIN @clientLocations as cl on cl.ClientLocationId = c.ClientLocationId
    WHERE ClaimBilledOn >= @StartDate AND ClaimBilledOn <= @EndOfMonth
	AND c.IsDeleted = 0 
	AND c.IsSupplanted = 0

    -- Calculate the number of days in the previous 3 months
    DECLARE @NumberOfDaysInPeriod INT = DATEDIFF(DAY, @StartDate, @EndOfMonth) + 1

    -- Calculate the average daily charged amount
    SET @AverageDailyCharge = @TotalBilledAmount / @NumberOfDaysInPeriod

    -- Get the ending Accounts Receivable (AR) for the month
	SELECT @EndingAR = SUM(BilledAmount)
	FROM [IntegratedServices].[ClaimStatusBatchClaims] as c
	LEFT JOIN [IntegratedServices].[ClaimStatusTransactions] AS t ON t.ClaimStatusBatchClaimId = c.Id
	LEFT JOIN [IntegratedServices].[ClaimLineItemStatuses] AS cst ON cst.Id = t.ClaimLineItemStatusId 
    RIGHT JOIN @clientLocations as cl on cl.ClientLocationId = c.ClientLocationId
    WHERE ClaimBilledOn <= EOMONTH(DATEADD(MONTH, 0, @FromDate))
	AND (cst.ClaimStatusTypeId NOT IN (@OtherAdjudicatedClaimStatusType, @PaidStatusTypes))
	AND c.IsDeleted = 0 
	AND c.IsSupplanted = 0
    --SELECT @EndingAR = ARTotal
    --FROM [dbo].[ClientEndOfMonthTotals] AS CEOM
    --WHERE EOMONTH(DATEFROMPARTS(CEOM.[Year], CEOM.[Month], 1)) = @EndOfMonth

    -- Calculate the days for the component
    SET @MonthlyDaysInAR = CEILING(@EndingAR / @AverageDailyCharge)
	RETURN @MonthlyDaysInAR
    -- Output the result
    --SELECT @MonthlyDaysInAR AS MonthlyDaysInAR, @EndingAR, @AverageDailyCharge, @NumberOfDaysInPeriod, @TotalBilledAmount, @StartDate, @EndOfMonth
END
