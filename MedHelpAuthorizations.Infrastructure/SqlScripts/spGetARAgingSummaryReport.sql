SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create or ALTER     PROCEDURE [IntegratedServices].[spGetARAgingSummaryReport]
	@ClientId int, 
    @ClientLocationIds VARCHAR(MAX)=null,
    @ClientProviderIds VARCHAR(MAX)=null,
    @ClientInsuranceIds VARCHAR(MAX)=null,
	@filterReportBy varchar(MAX)=null --'BilledOnDate'--'DateOfService'

AS
BEGIN

Declare @PaidClaimStatus int =1;
Declare @ApprovedClaimStatus int =2;
--Declare @WrittenOffClaimStatus int =20;
declare @ZeroPay int= 14;
declare @BundledFqhc int= 15;
Select  
    --COUNT(csb.Id) as 'Quantity', EN-56 updated by Mahendra Singh
    --COUNT(cb.ClaimLevelMD5Hash) as 'Quantity',
    i.Name as 'InsuranceName',
    i.Id as 'InsuranceId',
    SUM(cb.BilledAmount) as 'ChargedSum',
    -- cl.Name as 'LocationName',
    -- cl.Id as 'LocationId',
    -- pr.id as 'ProviderId',
    -- concat(p.LastName,' ', p.FirstName) as 'ProviderName',
	SUM(
        CASE 
            WHEN @filterReportBy = 'BilledOnDate' THEN 
                CASE 
                    WHEN DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) BETWEEN 1 AND 30 
                        THEN cb.BilledAmount 
                        ELSE 0
                END
            ELSE 
                CASE 
                    WHEN DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) BETWEEN 1 AND 30 
                        THEN cb.BilledAmount 
                        ELSE 0
                END
        END
    ) AS 'AgeGroup_0_30',
	
    SUM(
        CASE 
            WHEN @filterReportBy = 'BilledOnDate' THEN 
                CASE 
                    WHEN DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) BETWEEN 31 AND 60 
                        THEN cb.BilledAmount 
                        ELSE 0
                END
            ELSE 
                CASE 
                    WHEN DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) BETWEEN 31 AND 60
                        THEN cb.BilledAmount 
                        ELSE 0
                END
        END
    ) AS 'AgeGroup_31_60',
	
    SUM(
        CASE 
            WHEN @filterReportBy = 'BilledOnDate' THEN 
                CASE 
                    WHEN DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) BETWEEN 61 AND 90 
                        THEN cb.BilledAmount 
                        ELSE 0
                END
            ELSE 
                CASE 
                    WHEN DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) BETWEEN 61 AND 90 
                        THEN cb.BilledAmount 
                        ELSE 0
                END
        END
    ) AS 'AgeGroup_61_90',
	
    SUM(
        CASE 
            WHEN @filterReportBy = 'BilledOnDate' THEN 
                CASE 
                    WHEN DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) BETWEEN 91 AND 120 
                        THEN cb.BilledAmount 
                        ELSE 0
                END
            ELSE 
                CASE 
                    WHEN DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) BETWEEN 91 AND 120 
                        THEN cb.BilledAmount 
                        ELSE 0
                END
        END
    ) AS 'AgeGroup_91_120',
	
    SUM(
        CASE 
            WHEN @filterReportBy = 'BilledOnDate' THEN 
                CASE 
                    WHEN DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) BETWEEN 121 AND 150
                        THEN cb.BilledAmount 
                        ELSE 0
                END
            ELSE 
                CASE 
                    WHEN DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) BETWEEN 121 AND 150
                        THEN cb.BilledAmount 
                        ELSE 0
                END
        END
    ) AS 'AgeGroup_121_150',
    ---As per converstation with Kevin, Included 151-180 AgeGroup and Above 180 AgeGroup.
	SUM(
        CASE 
            WHEN @filterReportBy = 'BilledOnDate' THEN 
                CASE 
                    WHEN DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) BETWEEN 151 AND 180
                        THEN cb.BilledAmount 
                        ELSE 0
                END
            ELSE 
                CASE 
                    WHEN DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) BETWEEN 151 AND 180
                        THEN cb.BilledAmount 
                        ELSE 0
                END
        END
    ) AS 'AgeGroup_151_180',
    SUM(
        CASE 
            WHEN @filterReportBy = 'BilledOnDate' THEN 
                CASE 
                    WHEN DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) >= 181
                        THEN cb.BilledAmount 
                        ELSE 0
                END
            ELSE 
                CASE 
                    WHEN DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) >= 181
                        THEN cb.BilledAmount 
                        ELSE 0
                END
        END
    ) AS 'AgeGroup_Above_181'
    --SUM(
    --    CASE 
    --        WHEN @filterReportBy = 'BilledOnDate' THEN 
    --            CASE 
    --                WHEN DATEDIFF(day, cb.ClaimBilledOn, GETDATE()) >= 151
    --                    THEN cb.BilledAmount 
    --                    ELSE 0
    --            END
    --        ELSE 
    --            CASE 
    --                WHEN DATEDIFF(day, cb.DateOfServiceFrom, GETDATE()) >= 151
    --                    THEN cb.BilledAmount 
    --                    ELSE 0
    --            END
    --    END
    --) AS 'AgeGroup_151_plus'

From IntegratedServices.ClaimStatusBatchClaims as cb
    join IntegratedServices.ClaimStatusBatches as csb on csb.Id=cb.ClaimStatusBatchId
    left join IntegratedServices.ClaimStatusTransactions as t on t.Id=cb.ClaimStatusTransactionId
    left JOIN ClientInsurances as i on i.Id=csb.ClientInsuranceId
    left join IntegratedServices.ClaimLineItemStatuses as cs on cs.Id=t.ClaimLineItemStatusId
    left join ClientLocations as cl on cl.Id=cb.ClientLocationId
    left join Providers as pr on pr.Id=cb.ClientProviderId
    left join Persons as p on p.Id=pr.PersonId

Where i.ClientId = @ClientId
    And (t.ClaimLineItemStatusId not in (@PaidClaimStatus, @ApprovedClaimStatus,@ZeroPay,@BundledFqhc))
    And (csb.ClientInsuranceId in (SELECT convert(int, value)
        FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
    And (cl.Id in (SELECT convert(int, value)
        FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
    And (pr.Id in (SELECT convert(int, value)
        FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))     
    AND cb.IsDeleted = 0 
    AND cb.IsSupplanted = 0

Group by 
        i.Name,i.Id --, cb.ClaimLevelMD5Hash --        cl.Name,        p.LastName, p.FirstName,cl.Id,pr.Id

Order by i.Name

END
GO
