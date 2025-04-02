SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER     PROCEDURE [IntegratedServices].[spGetExecutiveClaimRate]
	@ClientId NVARCHAR(MAX) = NULL
	,@claimStatusTypeId INT = NULL

WITH RECOMPILE
AS
BEGIN

SELECT 
		cl.Id AS 'ClientLocationId'
		,cl.[Name] AS 'ClientLocationName'
		,COUNT(CASE WHEN cst.ClaimStatusTypeId = @claimStatusTypeId THEN c1.ClaimLevelMd5Hash ELSE NULL END) AS 'PaidonInitialReview'
		,COUNT(c1.ClaimLevelMd5Hash) AS 'Visits'
		,[IntegratedServices].[fnCalculatePercentage](COUNT(CASE WHEN cst.ClaimStatusTypeId = @claimStatusTypeId THEN c1.ClaimLevelMd5Hash ELSE NULL END), COUNT(c1.ClaimLevelMd5Hash)) AS 'CleanCLaimRate'

FROM 
		[IntegratedServices].ClaimStatusBatchClaims as c1
		JOIN(
			SELECT min(bc.Id) as 'MinId'
				, EntryMd5Hash as 'HashKey' --, min(CreatedOn) as 'ReceivedDate'
				FROM  [IntegratedServices].ClaimStatusBatchClaims bc
				GROUP BY EntryMd5Hash
			) as c2 ON c1.EntryMd5Hash = c2.HashKey AND c1.Id = c2.MinId
	
		LEFT JOIN [IntegratedServices].ClaimStatusTransactions AS t ON t.ClaimStatusBatchClaimId = c1.Id
		LEFT JOIN [dbo].[Clients] AS c ON c1.ClientId = c.Id
		LEFT JOIN [IntegratedServices].[ClaimLineItemStatuses] AS cst ON cst.Id = t.ClaimLineItemStatusId
		LEFT JOIN [dbo].[ClientLocations] AS cl ON cl.Id = c1.ClientLocationId
		AND c1.ClientId = @ClientId
		GROUP BY cl.Id, cl.[Name]
	END
