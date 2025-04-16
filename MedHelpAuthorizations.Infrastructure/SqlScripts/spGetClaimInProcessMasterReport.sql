SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER  PROCEDURE [IntegratedServices].[spGetClaimInProcessMasterReport]

AS
BEGIN

SELECT 
		CSBC.ClientId AS 'ClientId'
		,C.[Name] AS 'ClientName'
		,CI.Id AS 'PayerId'
		,CI.LookupName AS 'PayerName'
		,COUNT(DISTINCT(CSBC.ClaimLevelMd5Hash)) AS 'ClaimCount'
		,CRCR.FailureReported AS 'FailureReported'
		,CRCR.ExpiryWarningReported AS 'ExpiryWarningReported'

		FROM [IntegratedServices].[ClaimStatusBatchClaims] AS CSBC
		LEFT JOIN ClientInsurances AS CI ON CI.Id = CSBC.ClientInsuranceId
		LEFT JOIN [IntegratedServices].[ClaimStatusTransactions] AS T ON T.ClaimStatusBatchClaimId = CSBC.ID
		INNER JOIN [dbo].[Clients] AS C ON C.Id = CSBC.ClientId
		INNER JOIN [IntegratedServices].[ClientInsuranceRpaConfigurations] AS CIRC ON CIRC.ClientInsuranceId = CI.Id
		LEFT JOIN [dbo].[ClientRpaCredentialConfigurations] AS CRCR ON CRCR.Id = CIRC.ClientRpaCredentialConfigurationId

WHERE (CSBC.ClaimStatusTransactionId is NULL OR T.ClaimLineItemStatusId in (10,17))
		AND CSBC.IsDeleted = 0
		AND CSBC.IsSupplanted = 0

		GROUP BY 
		CI.Id, CI.LookupName, CSBC.ClientId, C.[Name], CRCR.FailureReported, CRCR.ExpiryWarningReported

	END
