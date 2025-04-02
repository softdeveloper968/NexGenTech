SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [IntegratedServices].[spCalculateExecutiveMonthlyDaysInAR]
	@ClientId INT = NULL

AS
BEGIN
    DECLARE @CurrentMonthlyDaysInAR INT = NULL
    DECLARE @PreviousMonthlyDaysInAR INT = NULL
    

    -- Calculate the End Date for the month
    DECLARE @EndOfMonth DATE = EOMONTH(DATEADD(MONTH, -1, GETDATE()))
    --DECLARE @PreviousEndOfMonth DATE = EOMONTH(DATEADD(MONTH, -1, @EndOfMonth))

    -- Assign the result of the function to a variable
	SET @CurrentMonthlyDaysInAR = [IntegratedServices].[fnCalculateExecutiveMonthlyDaysInAR](@ClientId, GETDATE())
	SET @PreviousMonthlyDaysInAR = [IntegratedServices].[fnCalculateExecutiveMonthlyDaysInAR](@ClientId, @EndOfMonth)

	SELECT @CurrentMonthlyDaysInAR AS 'CurrentMonthlyDaysInAR'
	,@PreviousMonthlyDaysInAR AS 'PreviousMonthlyDaysInAR'

END