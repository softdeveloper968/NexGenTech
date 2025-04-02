SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetARAgingReportExportDetails]
    @filterDayGroupby int,--30,31,61,91,121,151;
    @ClientId int,
    @ClientLocationIds varchar(MAX)='',
    @ClientProviderIds varchar(MAX)='',
    @ClientInsuranceIds varchar(MAX)='',
    @filterType varchar(MAX)='BilledOnDate'

AS
BEGIN
    
Declare @PaidClaimStatus int = 1;
Declare @ApprovedClaimStatus int = 2;
Declare @WrittenOffClaimStatus int = 20;
Declare @ZeroPay int= 14;
Declare @BundledFqhc int= 15;


if @filterType = 'BilledOnDate'
    SELECT 
            PatientPerson.LastName as 'Last Name',
            PatientPerson.FirstName as 'First Name',
            PatientPerson.DateOfBirth as 'DOB',
            i.Name as 'Insurance Name',
            cb.PolicyNumber as 'Policy Number',
            auth.Name as 'Service Type',
            cb.DateOfServiceFrom as 'DOS From',
            cb.ProcedureCode as 'CPT Code',
            cb.BilledAmount as 'Billed Amt',
            cb.ClaimBilledOn as 'Billed On', 
            claimStatus.Code as 'Lineitem Status', 
            claimStatus.Id as 'ClaimLineItemStatusId', 
            exp.Code as 'Exception Category',
            t.ExceptionReason as 'Exception Reason',
            cl.Name as 'ClientLocationName',
            cl.Npi as 'ClientLocationNpi'           

    FROM IntegratedServices.ClaimStatusBatchClaims as cb
        join IntegratedServices.ClaimStatusBatches as csb on csb.Id=cb.ClaimStatusBatchId
        Left join AuthTypes as auth on csb.AuthTypeId=auth.Id
        left join IntegratedServices.ClaimStatusTransactions as t on t.Id=cb.ClaimStatusTransactionId
        left join IntegratedServices.ClaimStatusExceptionReasonCategories as exp on t.ClaimStatusExceptionReasonCategoryId=exp.Id
        left join IntegratedServices.ClaimLineItemStatuses as claimStatus on t.ClaimLineItemStatusId =claimStatus.Id
        left join ClientInsurances as i on i.Id=csb.ClientInsuranceId
        left join IntegratedServices.ClaimLineItemStatuses as cs on cs.Id=t.ClaimLineItemStatusId
        left join ClientLocations as cl on cl.Id=cb.ClientLocationId
        left join Providers as pr on pr.Id=cb.ClientProviderId
        Left join Persons as p on p.Id=pr.PersonId

        --patient's data
        left JOIN Patients as patient on patient.Id = cb.PatientId
        left JOIN Persons as PatientPerson on PatientPerson.Id = patient.PersonId

        WHERE i.ClientId = @ClientId and
            (
                ( @filterDayGroupby= 30 and DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) BETWEEN 1 AND 30)
                        OR( @filterDayGroupby=31 AND DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) BETWEEN 31 and 60 )
                        OR( @filterDayGroupby=61 AND DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) BETWEEN 61 and 90 )
                        OR( @filterDayGroupby=91 AND DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) BETWEEN 91 and 120 )
                        OR( @filterDayGroupby=121 AND DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) BETWEEN 121 and 150 )
                        OR( @filterDayGroupby=151 AND DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) BETWEEN 151 and 180 )
                        OR( @filterDayGroupby=181 AND DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) >=181)
            )
            And (t.ClaimLineItemStatusId not in (@PaidClaimStatus, @ApprovedClaimStatus,@WrittenOffClaimStatus,@ZeroPay,@BundledFqhc))
            And (csb.ClientInsuranceId in (SELECT convert(int, value)
                FROM string_split(@ClientInsuranceIds, ',')) OR 
                     (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
            And (cl.Id in (SELECT convert(int, value)
                FROM string_split(@ClientLocationIds, ',')) OR 
                     (@ClientLocationIds is null OR @ClientLocationIds = ''))
            And (pr.Id in (SELECT convert(int, value)
                FROM string_split(@ClientProviderIds, ',')) OR 
                     (@ClientProviderIds is null OR @ClientProviderIds = ''))
            AND cb.IsDeleted = 0 
            AND cb.IsSupplanted = 0
    --GROUP BY cb.BilledAmount, cb.PatientLastName,
    --        cb.PatientFirstName ,
    --        p.DateOfBirth ,
    --        i.Name ,
    --        cb.PolicyNumber ,
    --        auth.Name ,
    --        cb.DateOfServiceFrom,
    --        cb.ProcedureCode ,
    --        cb.ClaimBilledOn,
    --        claimStatus.Code,
    --        claimStatus.Id, 
    --        exp.Code ,
    --        t.ExceptionReason,
    --        cb.ClientId
    --        ,PatientPerson.LastName,
    --        PatientPerson.FirstName ,
    --        PatientPerson.DateOfBirth
    --        ,cl.Name, cl.Npi

    ORDER BY cb.ClaimBilledOn

ELSE
    SELECT 
            PatientPerson.LastName as 'Last Name',
            PatientPerson.FirstName as 'First Name',
            PatientPerson.DateOfBirth as 'DOB',
            i.Name as 'Insurance Name',
            cb.PolicyNumber as 'Policy Number',
            auth.Name as 'Service Type',
            cb.DateOfServiceFrom as 'DOS From',
            cb.ProcedureCode as 'CPT Code',
            cb.BilledAmount as 'Billed Amt',
            cb.ClaimBilledOn as 'Billed On', 
            claimStatus.Code as 'Lineitem Status', 
            claimStatus.Id as 'ClaimLineItemStatusId', 
            exp.Code as 'Exception Category',
            t.ExceptionReason as 'Exception Reason',
            cl.Name as 'ClientLocationName',
            cl.Npi as 'ClientLocationNpi'  

    FROM IntegratedServices.ClaimStatusBatchClaims as cb
        join IntegratedServices.ClaimStatusBatches as csb on csb.Id=cb.ClaimStatusBatchId
        Left join AuthTypes as auth on csb.AuthTypeId=auth.Id
        left join IntegratedServices.ClaimStatusTransactions as t on t.Id=cb.ClaimStatusTransactionId
        left join IntegratedServices.ClaimStatusExceptionReasonCategories as exp on t.ClaimStatusExceptionReasonCategoryId=exp.Id
        left join IntegratedServices.ClaimLineItemStatuses as claimStatus on t.ClaimLineItemStatusId =claimStatus.Id
        left JOIN ClientInsurances as i on i.Id=csb.ClientInsuranceId
        left join IntegratedServices.ClaimLineItemStatuses as cs on cs.Id=t.ClaimLineItemStatusId
        left join ClientLocations as cl on cl.Id=cb.ClientLocationId
        left join Providers as pr on pr.Id=cb.ClientProviderId
        left join Persons as p on p.Id=pr.PersonId
        
        --patient's data
        left JOIN Patients as patient on patient.Id = cb.PatientId
        left JOIN Persons as PatientPerson on PatientPerson.Id = patient.PersonId

        WHERE i.ClientId = @ClientId and
            (
                ( @filterDayGroupby= 30 and DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) BETWEEN 1 AND 30)
                        OR( @filterDayGroupby=31 AND DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) BETWEEN 31 and 60 )
                        OR( @filterDayGroupby=61 AND DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) BETWEEN 61 and 90 )
                        OR( @filterDayGroupby=91 AND DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) BETWEEN 91 and 120 )
                        OR( @filterDayGroupby=121 AND DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) BETWEEN 121 and 150 )
                        OR( @filterDayGroupby=151 AND DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) BETWEEN 151 and 180 )
                        OR( @filterDayGroupby=181 AND DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) >=181)
            )
            And (t.ClaimLineItemStatusId not in (@PaidClaimStatus, @ApprovedClaimStatus,@WrittenOffClaimStatus,@ZeroPay,@BundledFqhc))
            And (csb.ClientInsuranceId in (SELECT convert(int, value)
                FROM string_split(@ClientInsuranceIds, ',')) OR 
                     (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
            And (cl.Id in (SELECT convert(int, value)
                FROM string_split(@ClientLocationIds, ',')) OR 
                     (@ClientLocationIds is null OR @ClientLocationIds = ''))
            And (pr.Id in (SELECT convert(int, value)
                FROM string_split(@ClientProviderIds, ',')) OR 
                     (@ClientProviderIds is null OR @ClientProviderIds = ''))            
            AND cb.IsDeleted = 0 
            AND cb.IsSupplanted = 0

    --GROUP BY cb.BilledAmount, cb.PatientLastName,
    --        cb.PatientFirstName ,
    --        p.DateOfBirth ,
    --        i.Name ,
    --        cb.PolicyNumber ,
    --        auth.Name ,
    --        cb.DateOfServiceFrom,
    --        cb.ProcedureCode ,
    --        cb.ClaimBilledOn,
    --        claimStatus.Code,
    --        claimStatus.Id, 
    --        exp.Code ,
    --        t.ExceptionReason,
    --        cb.ClientId
    --        ,PatientPerson.LastName,
    --        PatientPerson.FirstName ,
    --        PatientPerson.DateOfBirth
    --        ,cl.Name, cl.Npi


    ORDER BY cb.ClaimBilledOn

END
GO
