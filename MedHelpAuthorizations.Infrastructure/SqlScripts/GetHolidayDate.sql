SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[IntegratedServices].[GetHolidayDate]') 
    --AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [IntegratedServices].[GetHolidayDate]
GO
CREATE OR ALTER FUNCTION [IntegratedServices].[GetHolidayDate] (
    @HolidayId INT,
    @Year INT
)
RETURNS DATE
AS
BEGIN
    DECLARE @HolidayDate DATETIME;

    SET @HolidayDate = CASE @HolidayId
        WHEN 1 THEN DATEFROMPARTS(@Year, 1, 1) -- New Year's Day
        WHEN 2 THEN 
            -- Martin Luther King, Jr. Day (Third Monday in January)
            DATEADD(DAY, 15 + (1 - DATEPART(WEEKDAY, DATEFROMPARTS(@Year, 1, 1))), DATEFROMPARTS(@Year, 1, 1))
        WHEN 3 THEN DATEFROMPARTS(@Year, 1, 20) -- Inauguration Day
        WHEN 4 THEN 
            -- Presidents Day (Third Monday in February)
            DATEADD(DAY, 15 + (1 - DATEPART(WEEKDAY, DATEFROMPARTS(@Year, 2, 1))), DATEFROMPARTS(@Year, 2, 1))
        WHEN 5 THEN 
            -- Memorial Day (Last Monday in May)
			DATEADD(DAY, -DATEDIFF(DAY, 0, EOMONTH(DATEFROMPARTS(@Year, 5, 1))) % 7, EOMONTH(DATEFROMPARTS(@Year, 5, 1)))
        WHEN 6 THEN DATEFROMPARTS(@Year, 6, 19) -- Juneteenth
        WHEN 7 THEN DATEFROMPARTS(@Year, 7, 4) -- Independence Day
        WHEN 8 THEN 
            -- Labor Day (First Monday in September)
            DATEADD(DAY, 1 - DATEPART(WEEKDAY, DATEFROMPARTS(@Year, 9, 1)), DATEFROMPARTS(@Year, 9, 1))
        WHEN 9 THEN 
            -- Columbus Day (Second Monday in October)
            DATEADD(DAY, 7 + (1 - DATEPART(WEEKDAY, DATEFROMPARTS(@Year, 10, 1))), DATEFROMPARTS(@Year, 10, 1))
        WHEN 10 THEN DATEFROMPARTS(@Year, 11, 11) -- Veterans Day
        WHEN 11 THEN 
            -- Thanksgiving Day (Fourth Thursday in November)
            DATEADD(DAY, 22 + (1 - DATEPART(WEEKDAY, DATEFROMPARTS(@Year, 11, 1))), DATEFROMPARTS(@Year, 11, 1))
        WHEN 12 THEN DATEFROMPARTS(@Year, 12, 25) -- Christmas Day
        ELSE NULL
    END;

    RETURN @HolidayDate;
END;


--SELECT [IntegratedServices].[GetHolidayDate](5, 2023)