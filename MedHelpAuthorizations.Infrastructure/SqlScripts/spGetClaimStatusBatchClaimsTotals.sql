SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetClaimStatusBatchClaimsTotals]
AS
BEGIN
    SELECT c1.ClientInsuranceId as 'ClientInsuranceId'
            , c1.DateOfServiceFrom as 'DateOfServiceFrom'
            , c1.ProcedureCode as 'ProcedureCode'
			,c1.ClientId as 'ClientId'
    FROM [IntegratedServices].ClaimStatusBatchClaims as c1
    WHERE c1.IsDeleted = 0 AND c1.IsSupplanted = 0
    GROUP BY 
        c1.ClientInsuranceId,
        c1.DateOfServiceFrom,
        c1.ProcedureCode,
		c1.ClientId
END;
