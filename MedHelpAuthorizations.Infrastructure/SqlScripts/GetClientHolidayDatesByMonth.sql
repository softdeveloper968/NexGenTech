SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[IntegratedServices].[GetClientHolidayDatesByMonth]') 
    --AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [IntegratedServices].[GetClientHolidayDatesByMonth]
GO
CREATE OR ALTER FUNCTION [IntegratedServices].[GetClientHolidayDatesByMonth] (
	@ClientId NVARCHAR(MAX),
	@MonthNumber INT,
	@Year INT
)

RETURNS @Holidays TABLE (
	Dates DATETIME
)
AS 
BEGIN
	INSERT INTO @Holidays
	SELECT [IntegratedServices].[GetHolidayDate](CH.HolidayId, @Year)
	FROM dbo.[ClientHolidays] AS CH
	LEFT JOIN [dbo].[Holidays] AS H ON H.Id = CH.HolidayId
	WHERE ClientId = @ClientId AND [Month] = @MonthNumber

	RETURN
END