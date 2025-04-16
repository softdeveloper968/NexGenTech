SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetClientFeeScheduleEntryByClaims]
	 @ProcedureCode NVARCHAR(5)
    ,@DateOfServiceFrom DATETIME
    ,@ClientInsuranceId int
    ,@ProviderLevelId int = NULL
    ,@SpecialtyId int = NULL
AS
BEGIN
    SELECT TOP 1 cfe.Id as ClientFeeScheduleEntryId, cfe.IsReimbursable
    FROM dbo.ClientFeeScheduleEntries cfe

    JOIN dbo.ClientCptCodes cpt ON cpt.Id = cfe.ClientCptCodeId

    JOIN dbo.ClientFeeSchedules cfs ON cfe.ClientFeeScheduleId = cfs.Id

    JOIN dbo.ClientInsuranceFeeSchedules cifs ON cifs.ClientFeeScheduleId = cfs.Id

    LEFT JOIN dbo.ClientFeeScheduleProviderLevels pl ON cifs.ClientFeeScheduleId = pl.ClientFeeScheduleId

    LEFT JOIN dbo.ClientFeeScheduleSpecialties sp ON cifs.ClientFeeScheduleId = sp.ClientFeeScheduleId

    JOIN dbo.ClientInsurances ci ON ci.Id = cifs.ClientInsuranceId

    WHERE cifs.ClientInsuranceId = @ClientInsuranceId

        AND UPPER(TRIM(cpt.Code)) = UPPER(TRIM(@ProcedureCode))

        AND  (cfs.StartDate <= @DateOfServiceFrom OR cfs.StartDate is null)

        AND (cfs.EndDate >= @DateOfServiceFrom OR cfs.EndDate is null)

        AND (pl.ProviderLevelId = @ProviderLevelId OR pl.ProviderLevelId is null)
        
        AND (sp.SpecialtyId = @SpecialtyId OR sp.SpecialtyId is null)

        Order BY cifs.IsActive DESC, pl.ProviderLevelId DESC, sp.SpecialtyId DESC, cifs.CreatedOn DESC
END
GO