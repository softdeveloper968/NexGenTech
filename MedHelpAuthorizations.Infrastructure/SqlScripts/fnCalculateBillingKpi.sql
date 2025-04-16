SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[IntegratedServices].[fnCalculateBillingKpi]') 
    --AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [IntegratedServices].[fnCalculateBillingKpi]
GO
CREATE OR ALTER     FUNCTION [IntegratedServices].[fnCalculateBillingKpi](
    @ClientId NVARCHAR(MAX),
    @FromDate DATETIME,
    @ToDate DATETIME
)
RETURNS @KpiTable TABLE (
	ClientId NVARCHAR(MAX),
    MonthNumber INT,
    VisitsGoal DECIMAL,
    CleanClaimRateGoal DECIMAL,
    DenialRateGoal DECIMAL,
    ChargesGoal DECIMAL,
    CollectionPercentageGoal DECIMAL,
    CashCollectionsGoal DECIMAL,
    Over90DaysGoal DECIMAL,
    DaysInARGoal DECIMAL,
    BDRateGoal DECIMAL
)
AS
BEGIN
    DECLARE @StartMonth INT = MONTH(@FromDate);
    DECLARE @StartYear INT = YEAR(@FromDate);
    DECLARE @EndMonth INT = MONTH(@ToDate);
    DECLARE @EndYear INT = YEAR(@ToDate);
    
    DECLARE @LoopMonth INT = @StartMonth;

	DECLARE @MonthsInvolved INT = DATEDIFF(MONTH, @FromDate, @ToDate) + 1 

    -- Loop through each month from start to end
    WHILE (@StartYear < @EndYear OR (@StartYear = @EndYear AND @LoopMonth <= @EndMonth))
    BEGIN
        DECLARE @FirstDayOfMonth DATETIME = DATEFROMPARTS(@StartYear, @LoopMonth, 1);
        DECLARE @LastDayOfMonth DATETIME = EOMONTH(@FirstDayOfMonth);

        IF @LoopMonth = MONTH(@FromDate)
        BEGIN
            IF @FromDate = @FirstDayOfMonth
            BEGIN
                IF @ToDate = @LastDayOfMonth
                BEGIN
                    INSERT INTO @KpiTable
                    SELECT @ClientId, @LoopMonth, *
                    FROM [IntegratedServices].[fnCalculateDailyKpi](@ClientId, @FirstDayOfMonth, @LastDayOfMonth);
									    --PRINT 'Current Month1: ' + CAST(@LoopMonth AS VARCHAR);

                END
                ELSE
                BEGIN
                    INSERT INTO @KpiTable
                    SELECT @ClientId,  @LoopMonth, *
                    FROM [IntegratedServices].[fnCalculateDailyKpiFromDate](@ClientId, @FromDate);
									    --PRINT 'Current Month2: ' + CAST(@LoopMonth AS VARCHAR);

                END
            END
            ELSE
            BEGIN
                INSERT INTO @KpiTable
                SELECT @ClientId,  @LoopMonth, *
                FROM [IntegratedServices].[fnCalculateDailyKpiFromDate](@ClientId, @FromDate);
							    --PRINT 'Current Month3: ' + CAST(@LoopMonth AS VARCHAR);

            END
        END
        ELSE
        BEGIN
            IF @LoopMonth = MONTH(@ToDate)
            BEGIN
                IF @ToDate = @LastDayOfMonth
                BEGIN
                    INSERT INTO @KpiTable
                    SELECT @ClientId,  @LoopMonth, *
                    FROM [IntegratedServices].[fnCalculateDailyKpi](@ClientId, @FromDate, @ToDate);
									    --PRINT 'Current Month4: ' + CAST(@LoopMonth AS VARCHAR);

                END
                ELSE
                BEGIN
                    INSERT INTO @KpiTable
                    SELECT @ClientId,  @LoopMonth, *
                    FROM [IntegratedServices].[fnCalculateDailyKpiToDate](@ClientId, @ToDate);
									    --PRINT 'Current Month5: ' + CAST(@LoopMonth AS VARCHAR);

                END
            END
            ELSE
            BEGIN
                INSERT INTO @KpiTable
                SELECT @ClientId,  @LoopMonth, *
                FROM [IntegratedServices].[fnCalculateDailyKpi](@ClientId, @FirstDayOfMonth, @LastDayOfMonth);
							    --PRINT 'Current Month6: ' + CAST(@LoopMonth AS VARCHAR);

            END
        END

        -- Move to the next month
        IF @LoopMonth = 12
        BEGIN
            SET @LoopMonth = 1;
            SET @StartYear = @StartYear + 1;
        END
        ELSE
        BEGIN
            SET @LoopMonth = @LoopMonth + 1;
        END
    END

    INSERT INTO @KpiTable(VisitsGoal, CleanClaimRateGoal, DenialRateGoal, ChargesGoal, CollectionPercentageGoal, CashCollectionsGoal, Over90DaysGoal, DaysInARGoal, BDRateGoal)
    SELECT 
        SUM(VisitsGoal) AS VisitsGoal,
        SUM(CleanClaimRateGoal)/@MonthsInvolved AS TotalCleanClaimRateGoal,
        SUM(DenialRateGoal)/@MonthsInvolved AS TotalDenialRateGoal,
        SUM(ChargesGoal) AS TotalChargesGoal,
        SUM(CollectionPercentageGoal)/@MonthsInvolved AS TotalCollectionPercentageGoal,
        SUM(CashCollectionsGoal) AS TotalCashCollectionsGoal,
        SUM(Over90DaysGoal) AS TotalOver90DaysGoal,
        SUM(DaysInARGoal) AS DaysInARGoal,
        SUM(BDRateGoal) AS TotalBDRateGoal
    FROM @KpiTable
    GROUP BY ClientId;

	  -- Optionally, clear the original rows to keep only the aggregated result
    DELETE FROM @KpiTable WHERE MonthNumber IS NOT NULL;
	RETURN;

END;
