SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- AA-130
CREATE OR ALTER   PROCEDURE [IntegratedServices].[spGetARAgingTotals]
	@ClientId int, 
    @ClientLocationIds VARCHAR(48)=null,
    @ClientProviderIds VARCHAR(48)=null,
    @ClientInsuranceIds VARCHAR(48)=null,
    @ClientExceptionReasonCategoryIds VARCHAR(48)=null,
    @ClientAuthTypeIds VARCHAR(48)=null,
    @ClientProcedureCodes VARCHAR(48)=null,
	@filterReportBy varchar(48)='BilledOnDate'--'DateOfService'

AS
BEGIN

--Declare @PaidClaimStatus int =1;
--Declare @ApprovedClaimStatus int =2;
--Declare @WrittenOffClaimStatus int =20;
--declare @ZeroPay int= 14;
--declare @BundledFqhc int= 15;

Declare @Denied int = 7;
Declare @MemberNotFound int = 12;
Declare @NotOnFile int = 23;
Declare @UnMatchedProcedureCode int = 9;
Declare @Rejected int = 9;
Declare @Ignored int = 13;
Declare @CallPayer int = 18;

Select  
    COUNT(cb.ClaimLevelMD5Hash) as 'Quantity', --EN-56
    -- i.Name as 'InsuranceName',
    -- i.Id as 'InsuranceId',
    SUM(cb.BilledAmount) as 'ChargedSum',
    CONVERT(date, cb.ClaimBilledOn) as 'Billed On',
    CONVERT(date, cb.DateOfServiceFrom) as 'DOS From',
    t.ClaimLineItemStatusId as 'ClaimLineItemStatusId'
    -- cl.Name as 'LocationName',
    -- cl.Id as 'LocationId',
    -- pr.id as 'ProviderId',
    -- concat(p.LastName,' ', p.FirstName) as 'ProviderName',

From IntegratedServices.ClaimStatusBatchClaims as cb
    join IntegratedServices.ClaimStatusBatches as csb on csb.Id=cb.ClaimStatusBatchId
    left join IntegratedServices.ClaimStatusTransactions as t on t.Id=cb.ClaimStatusTransactionId
    left JOIN ClientInsurances as i on i.Id=csb.ClientInsuranceId
    --left join IntegratedServices.ClaimLineItemStatuses as cs on cs.Id = t.ClaimLineItemStatusId
   -- left join ClientLocations as cl on cl.Id=cb.ClientLocationId
   -- left join Providers as pr on pr.Id=cb.ClientProviderId
    --left join Persons as p on p.Id=pr.PersonId

Where i.ClientId = @ClientId
    --And (t.ClaimLineItemStatusId not in (@PaidClaimStatus, @ApprovedClaimStatus,@WrittenOffClaimStatus,@ZeroPay,@BundledFqhc))
    And (t.ClaimLineItemStatusId in (@Denied, @MemberNotFound, @NotOnFile, @UnMatchedProcedureCode, @Rejected, @Ignored, @CallPayer))
    And (cb.ClientInsuranceId in (SELECT convert(int, value)
        FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
    And (cb.ClientLocationId in (SELECT convert(int, value)
        FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
    And (cb.ClientProviderId in (SELECT convert(int, value)
        FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
    And (t.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
        FROM string_split(@ClientExceptionReasonCategoryIds, ',')) OR (@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = ''))
    And (csb.AuthTypeId in (SELECT convert(int, value)
        FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
    And (cb.ClientCptCodeId in (SELECT convert(int, value)
        FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))

Group by  CONVERT(date, cb.ClaimBilledOn),  CONVERT(date, cb.DateOfServiceFrom), t.ClaimLineItemStatusId, cb.ClaimLevelMD5Hash
--         i.Name,i.Id --        cl.Name,        p.LastName, p.FirstName,cl.Id,pr.Id

-- Order by i.Name

END
GO