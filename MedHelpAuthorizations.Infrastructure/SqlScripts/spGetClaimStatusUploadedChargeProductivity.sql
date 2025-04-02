SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [IntegratedServices].[spGetEmployeeClaimStatusTransactionLineItemStatusChangẹDetails]
    @clientId int,
	@DelimitedLineItemStatusIds nvarchar(max)=NULL

AS
BEGIN

select
        cstsc.ClaimStatusTransactionId
        ,cstsc.PreviousClaimLineItemStatusId
        ,cstsc.UpdatedClaimLineItemStatusId
		,bc.PatientFirstName
		,bc.PatientLastName
		,bc.PolicyNumber
		,bc.DateOfBirth as 'DateOfBirth'
		,bc.ProcedureCode
		,bc.BilledAmount
		,i.LookupName as 'InsuranceName'
		,CONVERT(DATE, bc.DateOfServiceFrom) as 'DateOfService'
		,bc.ClaimNumber as 'OfficeClaimNumber'
		,t.ClaimNumber as 'InsuranceClaimNumber'
		,CONVERT(DATE, bc.ClaimBilledOn)
		,bc.BilledAmount 
		,t.ExceptionRemark
		,t.ExceptionReason
		,t.ReferringProviderName
		,bc.ClientLocationId
		,loc.[Name] as 'ClientLocationName'
        ,loc.Npi as 'ClientLocationNpi'
		,per.LastName + ', ' + per.FirstName as 'ProviderName'
		,bc.ClientInsuranceId		
		
from IntegratedServices.[ClaimStatusBatchClaims] as bc
JOIN(
		SELECT * FROM [IntegratedServices].[fnGetClaimLevelGroups](
					@ClientId, 
					@DelimitedLineItemStatusIds
					,null
					,null
					,null
					,null
					,null
					,null
					,null
					,null
					,null
					,null
					,null
					,null
					,null
					,null
					,null
					,null
				)
	) as c2 ON bc.ClaimBilledOn = c2.LatestClaimBilledOn AND bc.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
    JOIN IntegratedServices.ClaimStatusTransactions as t on t.Id = bc.ClaimStatusTransactionId
	JOIN IntegratedServices.[ClaimStatusTransactionLineItemStatusChangẹs] as cstsc on cstsc.Id = t.ClaimStatusTransactionLineItemStatusChangẹId
	left join ClientLocations as loc on loc.Id = bc.ClientLocationId
	left join Providers as pro on pro.Id = bc.ClientProviderId
    join Persons as per on per.Id = pro.PersonId
	left join [dbo].ClientInsurances as i ON i.Id = bc.ClientInsuranceId

WHERE bc.ClientId = @clientId
     And (COALESCE(cstsc.LastModifiedOn, cstsc.CreatedOn) >= DATEADD(DAY, -1, CAST(GETDATE() AS DATE)))
     AND (COALESCE(cstsc.LastModifiedOn, cstsc.CreatedOn) < DATEADD(DAY, 1, CAST(GETDATE() AS DATE)))	

    AND (cstsc.UpdatedClaimLineItemStatusId IN (SELECT convert(int, value) 
        FROM string_split(@DelimitedLineItemStatusIds, ',')) 
        OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))

Group By cstsc.ClaimStatusTransactionId
        ,cstsc.PreviousClaimLineItemStatusId
        ,cstsc.UpdatedClaimLineItemStatusId
		,bc.PatientFirstName
		,bc.PatientLastName
		,bc.PolicyNumber
		,bc.DateOfBirth
		,bc.ProcedureCode
		,bc.BilledAmount
        ,CONVERT(DATE, bc.DateOfServiceFrom)
		,bc.ClaimNumber
		,t.ClaimNumber
		,CONVERT(DATE, bc.ClaimBilledOn)
		,bc.BilledAmount
		,t.ExceptionRemark
		,t.ExceptionReason
		,t.ReferringProviderName
		,bc.ClientLocationId
        ,loc.Id
		,loc.[Name]
		,per.LastName + ', ' + per.FirstName
		,loc.Npi
		,i.LookupName
		,bc.ClientInsuranceId
End
GO