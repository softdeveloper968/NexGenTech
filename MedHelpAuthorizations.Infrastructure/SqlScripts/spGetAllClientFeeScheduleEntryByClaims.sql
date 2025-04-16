SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetAllClientFeeScheduleEntryByClaims]
	 @procedureCode NVARCHAR(5)
    ,@dateOfService DATETIME
    ,@clientInsuranceId int
    ,@providerLevelId int = NULL
    ,@specialtyId int = NULL
AS
BEGIN
    SELECT *
    FROM dbo.ClientFeeScheduleEntries cfe

    JOIN dbo.ClientCptCodes cpt ON cpt.Id = cfe.ClientCptCodeId

    JOIN dbo.ClientFeeSchedules cfs ON cfe.ClientFeeScheduleId = cfs.Id

    JOIN dbo.ClientInsuranceFeeSchedules cifs ON cifs.ClientFeeScheduleId = cfs.Id

    LEFT JOIN dbo.ClientFeeScheduleProviderLevels pl ON cifs.ClientFeeScheduleId = pl.ClientFeeScheduleId

    LEFT JOIN dbo.ClientFeeScheduleSpecialties sp ON cifs.ClientFeeScheduleId = sp.ClientFeeScheduleId

    JOIN dbo.ClientInsurances ci ON ci.Id = cifs.ClientInsuranceId

    WHERE cifs.ClientInsuranceId = @clientInsuranceId

        AND UPPER(TRIM(cpt.Code)) = UPPER(TRIM(@procedureCode))

        AND  (cfs.StartDate <= @dateOfService OR cfs.StartDate is null)

        AND (cfs.EndDate >= @dateOfService OR cfs.EndDate is null)

        AND (pl.ProviderLevelId = @providerLevelId OR pl.ProviderLevelId is null)
        
        AND (sp.SpecialtyId = @specialtyId OR sp.SpecialtyId is null)

        Order BY cifs.IsActive DESC, pl.ProviderLevelId DESC, sp.SpecialtyId DESC, cifs.CreatedOn DESC
END
GO