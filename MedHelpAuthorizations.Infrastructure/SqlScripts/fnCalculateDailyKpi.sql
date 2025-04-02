SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[IntegratedServices].[fnCalculateDailyKpi]') 
    --AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [IntegratedServices].[fnCalculateDailyKpi]
GO
CREATE OR ALTER     FUNCTION [IntegratedServices].[fnCalculateDailyKpi](
	@ClientId NVARCHAR(MAX)
	,@ClaimBilledFrom DATETIME
	,@ClaimBilledTo DATETIME
	)
RETURNS TABLE
AS
RETURN
(
    WITH Dates AS (
        SELECT DATEADD(DAY, n - 1, DATEFROMPARTS(YEAR(@ClaimBilledFrom), MONTH(@ClaimBilledFrom), 1)) AS Date
        FROM (
            SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
			FROM sys.all_columns
        ) AS Numbers
        WHERE DATEADD(DAY, n - 1, DATEFROMPARTS(YEAR(@ClaimBilledFrom), MONTH(@ClaimBilledFrom), 1)) <= EOMONTH(@ClaimBilledFrom)
    ),
    DaysOperated AS (
        SELECT COUNT(*) AS DaysOperated,
               SUM(CASE WHEN D.Date >= @ClaimBilledFrom AND D.Date <= @ClaimBilledTo THEN 1 ELSE 0 END) AS DaysInRange
        FROM Dates AS D
        INNER JOIN dbo.ClientDaysOfOperation AS CDO ON DATEDIFF(DAY, 0, D.Date) % 7 + 1 = CDO.DayOfWeekId
        WHERE CDO.ClientId = @ClientId
		AND D.Date NOT IN (select * from [IntegratedServices].[GetClientHolidayDatesByMonth] (
									@ClientId,
									MONTH(@ClaimBilledFrom),
									YEAR(@ClaimBilledFrom)
								))
    ),
    KPI_Goals AS (
        SELECT 
		--DaysOfOperation,
		--DOs.DaysOperated,
		--Charges,
		--DOs.DaysInRange,
            Visits / NULLIF(DOs.DaysOperated, 0) * DOs.DaysInRange AS VisitsGoal,
            CleanClaimRate AS CleanClaimRateGoal,
            DenialRate AS DenialRateGoal,
            Charges / NULLIF(DOs.DaysOperated, 0) * DOs.DaysInRange AS ChargesGoal,
            CollectionPercentage AS CollectionPercentageGoal,
            CashCollections / NULLIF(DOs.DaysOperated, 0) * DOs.DaysInRange AS CashCollectionsGoal,
            Over90Days / NULLIF(DOs.DaysOperated, 0) * DOs.DaysInRange AS Over90DaysGoal,
            DaysInAR / NULLIF(DOs.DaysOperated, 0) * DOs.DaysInRange AS DaysInARGoal,
            BDRate / NULLIF(DOs.DaysOperated, 0) * DOs.DaysInRange AS BDRateGoal
        FROM [dbo].ClientKpi AS CK
       
		CROSS JOIN DaysOperated AS DOs

        WHERE CK.ClientId = @ClientId
    )
    SELECT *
    FROM KPI_Goals
);