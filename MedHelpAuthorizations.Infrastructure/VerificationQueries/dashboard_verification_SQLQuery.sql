
DECLARE @ClientId int
DECLARE @DelimitedLineItemStatusIds nvarchar(MAX) = NULL
DECLARE @ReceivedFrom DateTime = NULL
DECLARE @ReceivedTo DateTime = NULL
DECLARE @DateOfServiceFrom DateTime = NULL
DECLARE @DateOfServiceTo DateTime = NULL
DECLARE @TransactionDateFrom DateTime = NULL
DECLARE @TransactionDateTo DateTime = NULL
DECLARE @ClaimBilledFrom DateTime = NULL
DECLARE @ClaimBilledTo DateTime = NULL
DECLARE @ClientProviderIds nvarchar(MAX) = NULL
DECLARE @ClientLocationIds nvarchar(MAX) = NULL
DECLARE @ClientInsuranceIds nvarchar(MAX)= null
DECLARE @ClientExceptionReasonCategoryIds nvarchar(MAX)= null
DECLARE @ClientAuthTypeIds nvarchar(MAX)= null
DECLARE @ClientProcedureCodes nvarchar(MAX)= null
DECLARE @PatientId int = NULL
DECLARE @ClaimStatusBatchId int = NULL
DECLARE @FlattenedLineItemStatus NVARCHAR(MAX)=NULL
DECLARE @DashboardType NVARCHAR(MAX)=NULL

SET @ClientId = 3

--SET @DelimitedLineItemStatusIds = '1,2'--Paid/Approved
--SET @DelimitedLineItemStatusIds = '1,2,14,15'--All PAID
--SET @ClaimStatusType = 1

SET @ClaimBilledFrom = '2024-02-08'
SET @ClaimBilledTo = '2024-03-08'


-------------------------------------------------------------------------------------------------------------------------

EXEC [IntegratedServices].[spGetClaimStatusVisits] @ClientId = @ClientId, @ClaimBilledFrom=@ClaimBilledFrom, @ClaimBilledTo = @ClaimBilledTo, @DelimitedLineItemStatusIds=@DelimitedLineItemStatusIds

-------------------------------------------------------------------------------------------------------------------------

SELECT 
	sum(claimStatusBatchClaim.BilledAmount) as 'Kevin Verification Query' 
FROM IntegratedServices.ClaimStatusBatchClaims  as claimStatusBatchClaim 
LEFT JOIN IntegratedServices.ClaimStatusBatches as claimStatusBatch ON claimStatusBatch.Id = claimStatusBatchClaim.ClaimStatusBatchId
LEFT JOIN dbo.ClientLocations as clientLocation ON clientLocation.Id = claimStatusBatchClaim.ClientLocationId
LEFT JOIN dbo.Providers prov ON prov.Id = claimStatusBatchClaim.ClientProviderId

WHERE claimStatusBatchClaim.ClientId = @ClientId
AND claimStatusBatchClaim.IsSupplanted = 0
AND claimStatusBatchClaim.IsDeleted = 0
AND claimStatusBatchClaim.ClaimBilledOn >= @ClaimBilledFrom
AND claimStatusBatchClaim.ClaimBilledOn <= @ClaimBilledTo


-------------------------------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------------------------------
