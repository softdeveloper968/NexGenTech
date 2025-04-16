/****** Object:  StoredProcedure [dbo].[sp_ADFLogs]    Script Date: 07/15/2024 10:08:28 AM ******/
DROP PROCEDURE [dbo].[sp_ADFLogs]
GO

/****** Object:  StoredProcedure [dbo].[sp_ADFLogs]    Script Date: 07/15/2024 10:08:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_ADFLogs]
    @FileName NVARCHAR(MAX),
    @PipeLineName VARCHAR(500),
    @ActivityName VARCHAR(500),
    @StartTime DATETIME,
    @EndTime DATETIME,
    @Status VARCHAR(50),
    @ErrorMessage NVARCHAR(MAX)
AS
BEGIN
        INSERT INTO tbl_LogDetails (FileName, PipeLineName, ActivityName, StartTime, EndTime, Status, ErrorMessage, CreatedOn)
        VALUES (@FileName, @PipeLineName, @ActivityName, @StartTime, @EndTime, @Status, @ErrorMessage, GETDATE());
END;
GO


