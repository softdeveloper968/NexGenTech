DECLARE @DeleteClientId int = 35

-- DELETE Claims/.Batches/Transaction and related

DECLARE @IdsToRemove TABLE(
    BatchId int NOT NULL,
    BatchClaimId int NOT NULL, 
    TransactionId int NULL
);
 
INSERT INTO @IdsToRemove (BatchId,BatchClaimId, TransactionId)
Select b.Id, bc.Id, bc.ClaimStatusTransactionId FROM IntegratedServices.ClaimStatusBatchClaims bc
JOIN IntegratedServices.ClaimStatusTransactions tx ON tx.Id = bc.ClaimStatusTransactionId
Join IntegratedServices.ClaimStatusBatches b ON b.Id = bc.ClaimStatusBatchID
WHERE 1=1
AND bc.ClientId = @DeleteClientId OR b.ClientId =  @DeleteClientId
 
--SELECT * FROM @IdsToRemove
 
--- DELETE TransactionHx for BatchId and lineStatus ---
DELETE FROM IntegratedServices.ClaimStatusTransactionHistories
WHERE ClaimStatusTransactionId in (SELECT TransactionId
FROM @IdsToRemove)
 
--- DETACH ClaimStatusBatchClaim ---
UPDATE IntegratedServices.ClaimStatusBatchClaims
SET ClaimStatusTransactionId = null
FROM IntegratedServices.ClaimStatusBatchClaims bc
WHERE bc.ClaimStatusTransactionId In (SELECT TransactionId
FROM @IdsToRemove)
 
--- DELETE Claim Status Transactions ---
DELETE FROM IntegratedServices.ClaimStatusTransactions
WHERE Id IN (SELECT TransactionId
FROM @IdsToRemove)
 
 --- DELETE Claim Status Last Status Change ---
DELETE FROM IntegratedServices.[ClaimStatusTransactionLineItemStatusChangẹs]
WHERE Id IN (SELECT tx.ClaimStatusTransactionLineItemStatusChangẹId FROM IntegratedServices.ClaimStatusTransactions tx Where id IN (SELECT TransactionId
FROM @IdsToRemove))


--- DELETE Claim Status Batch Claims ---
DELETE FROM IntegratedServices.ClaimStatusBatchClaims
WHERE Id IN (SELECT BatchClaimId
FROM @IdsToRemove)

-- Detach Batch Rpa insurance
UPDATE IntegratedServices.ClaimStatusBatchHistories
SET AssignedClientRpaConfigurationId = NULL
WHERE ClientId = @DeleteClientId

UPDATE IntegratedServices.ClaimStatusBatches
SET AssignedClientRpaConfigurationId = NULL
WHERE ClientId = @DeleteClientId

--- DELETE Batch ---
	DELETE FROM IntegratedServices.ClaimStatusBatchHistories 
    WHERE ClaimStatusBatchId IN (SELECT batchId
    FROM @IdsToRemove)
	-- WHERE AssignedClientRpaConfigurationId in (Select rpa.Id FROM IntegratedServices.ClientInsuranceRpaConfigurations rpa
	-- 											JOIN dbo.ClientInsurances i ON i.Id = rpa.ClientInsuranceId
	-- 											WHERE i.ClientId = @DeleteClientId)

DELETE FROM IntegratedServices.ClaimStatusBatchHistories
WHERE ClaimStatusBatchId IN (SELECT batchId
FROM @IdsToRemove)
 
--- DELETE Batch ---
DELETE FROM IntegratedServices.ClaimStatusBatches
WHERE Id IN (SELECT batchId
FROM @IdsToRemove)
 
 
 
 --- DELETE All THe rest 
--- ProviderId's
DECLARE @ProviderIdsToRemove TABLE(
    ProviderId int NOT NULL
);

INSERT INTO @ProviderIdsToRemove (ProviderId)
Select prv.Id FROM dbo.Providers prv
WHERE prv.ClientId = @DeleteClientId

--SELECT * FROM @ProviderIdsToRemove
--- 
DECLARE @EmployeeClientIdsToRemove TABLE(
    EmployeeClientId int NOT NULL
);
INSERT INTO @EmployeeClientIdsToRemove (EmployeeClientId)
Select empcl.Id FROM dbo.EmployeeClients empcl
WHERE empcl.ClientId = @DeleteClientId

DECLARE @ClientInsuranceIdsToRemove TABLE(
    ClientInsuranceId int NOT NULL
);
INSERT INTO @ClientInsuranceIdsToRemove (ClientInsuranceId)
Select ins.Id FROM dbo.ClientInsurances ins 
WHERE ins.ClientId = @DeleteClientId

DECLARE @ClientCptCodesToRemove TABLE(
    ClientCptCodeId int NOT NULL
);
INSERT INTO @ClientCptCodesToRemove (ClientCptCodeId)
Select cpt.Id FROM dbo.ClientCptCodes cpt 
WHERE cpt.ClientId = @DeleteClientId


DECLARE @AuthorizationIdsToRemove TABLE(
    AuthorizationId int NOT NULL
);
INSERT INTO @AuthorizationIdsToRemove (AuthorizationId)
Select a.Id FROM dbo.Authorizations a 
WHERE a.ClientId = @DeleteClientId


-- Authorizations
	DELETE FROM dbo.Documents
	WHERE 1=1 
	AND AuthorizationId IN (SELECT AuthorizationId FROM @AuthorizationIdsToRemove)

	DELETE FROM dbo.AuthorizationClientCptCode
	WHERE 1=1 
	AND AuthorizationId IN (SELECT AuthorizationId FROM @AuthorizationIdsToRemove)

	DELETE FROM dbo.Notes
	WHERE 1=1 
	AND AuthorizationId IN (SELECT AuthorizationId FROM @AuthorizationIdsToRemove)

	DELETE FROM dbo.Authorizations
	WHERE 1=1 
	AND Id IN (SELECT AuthorizationId FROM @AuthorizationIdsToRemove)

-- UserClients
	DELETE FROM dbo.UserClients
	WHERE 1=1 
	AND ClientId = @DeleteClientId


-- Insurance Cards
	DELETE FROM dbo.InsuranceCards
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- Messages
    DELETE FROM dbo.Messages
	WHERE 1=1 
	AND ClientId = @DeleteClientId

------------  Start EmployeeClients Dependencies ------------------
-- ClientEmployeeRoles
    DELETE FROM dbo.ClientEmployeeRoles
	WHERE 1=1 
	AND EmployeeClientId IN (SELECT EmployeeClientId FROM @EmployeeClientIdsToRemove)

-- EmployeeClientAlphaSplits
    DELETE FROM dbo.EmployeeClientAlphaSplits
	WHERE 1=1 
	AND EmployeeClientId   IN (SELECT EmployeeClientId FROM @EmployeeClientIdsToRemove)

	-- EmployeeClientAlphaSplits
    DELETE FROM dbo.EmployeeClientInsurances
	WHERE 1=1 
	AND EmployeeClientId   IN (SELECT EmployeeClientId FROM @EmployeeClientIdsToRemove)

-- EmployeeClients
    DELETE FROM dbo.EmployeeClients
	WHERE 1=1 
	AND ClientId = @DeleteClientId

------------  End EmployeeClients Dependencies ---------------------


------------  Start ClientUserReportFilter Dependencies ------------------

-- EmployeeClientUserReportFilter
    DELETE FROM dbo.EmployeeClientUserReportFilter
	WHERE 1=1 
	AND EmployeeClientId   IN(SELECT EmployeeClientId FROM @EmployeeClientIdsToRemove)

-- ClientUserReportFilter
    DELETE FROM dbo.ClientUserReportFilter
	WHERE 1=1 
	AND ClientId = @DeleteClientId

------------  End ClientUserReportFilter Dependencies ---------------------

-- ClientSpecialties
    DELETE FROM dbo.ClientSpecialties
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- ClientPlacesOfService
    DELETE FROM dbo.ClientPlacesOfService
	WHERE 1=1 
	AND ClientId = @DeleteClientId


	Update dbo.Clients
	SET ClientKpiId = null
	WHERE Id = @DeleteClientId

-- ClientKpi
    DELETE FROM dbo.ClientKpi
	WHERE 1=1 
	AND ClientId = @DeleteClientId
	
-- InputDocuments
    DELETE FROM IntegratedServices.InputDocuments
	WHERE 1=1 
	AND ClientId = @DeleteClientId

	------------  Start Persons Dependencies ------------------
-- ClientProviderLocations

    DELETE FROM dbo.ClientProviderLocations
	WHERE 1=1 
	AND ClientProviderId IN (SELECT ClientProviderId FROM @ProviderIdsToRemove)

 --Patients
 --UPDATE dbo.Patients 
 --SET ClientId =  @KeepClientId
 --WHERE ClientId = @DeleteClientId

    DELETE pat FROM dbo.Patients pat
	JOIN IntegratedServices.ClaimStatusBatchClaims bc ON bc.PatientId = pat.Id
	WHERE 1=1 
	AND bc.ClientId = @DeleteClientId

	DELETE pat FROM dbo.Patients pat
	WHERE 1=1 
	AND pat.ClientId = @DeleteClientId

-- Cardholders
    DELETE FROM dbo.Cardholders
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- Providers
    DELETE FROM dbo.Providers
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- *Persons
	--UPDATE dbo.Persons 
	-- SET ClientId =  @KeepClientId
	-- WHERE ClientId = @DeleteClientId

    DELETE FROM dbo.Persons
	WHERE 1=1 
	AND ClientId = @DeleteClientId
	
------------  End Persons Dependencies ---------------------
------------  Start ClientInsurances Dependencies ------------------

-- ClaimStatusTransactionHistories
    DELETE FROM IntegratedServices.ClaimStatusTransactionHistories
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- ClaimStatusTransactions
    DELETE FROM IntegratedServices.ClaimStatusTransactions
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- ClaimStatusBatchClaims
    DELETE FROM IntegratedServices.ClaimStatusBatchClaims
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- ClaimStatusBatchHistories
	DELETE bh FROM IntegratedServices.ClaimStatusBatchHistories bh
	WHERE AssignedClientRpaConfigurationId in (Select rpa.Id FROM IntegratedServices.ClientInsuranceRpaConfigurations rpa
												JOIN dbo.ClientInsurances i ON i.Id = rpa.ClientInsuranceId
												WHERE i.ClientId = @DeleteClientId)

    DELETE FROM IntegratedServices.ClaimStatusBatchHistories
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- ClaimStatusBatches
DELETE b FROM IntegratedServices.ClaimStatusBatches b
	WHERE AssignedClientRpaConfigurationId in (Select rpa.Id FROM IntegratedServices.ClientInsuranceRpaConfigurations rpa
												JOIN dbo.ClientInsurances i ON i.Id = rpa.ClientInsuranceId
												WHERE i.ClientId = @DeleteClientId)

    DELETE FROM IntegratedServices.ClaimStatusBatches
	WHERE 1=1 
	AND (ClientId = @DeleteClientId 
		OR ClientInsuranceId   IN (SELECT ClientInsuranceId FROM @ClientInsuranceIdsToRemove))

-- ClientInsuranceRpaConfigurations
    DELETE FROM IntegratedServices.ClientInsuranceRpaConfigurations
	WHERE 1=1 
	AND ClientInsuranceId   IN (SELECT ClientInsuranceId FROM @ClientInsuranceIdsToRemove)

-- ClientInsuranceAverageCollectionPercentages
    DELETE FROM dbo.ClientInsuranceAverageCollectionPercentages
	WHERE 1=1 
	AND ClientInsuranceId IN (SELECT ClientInsuranceId FROM @ClientInsuranceIdsToRemove)

-- ClientLocationInsuranceIdentifiers
    DELETE FROM dbo.ClientLocationInsuranceIdentifiers
	WHERE 1=1 
	AND ClientInsuranceId IN (SELECT ClientInsuranceId FROM @ClientInsuranceIdsToRemove)

-- ClientInsurances
    DELETE FROM dbo.ClientInsurances
	WHERE 1=1 
	AND ClientId = @DeleteClientId

------------  End ClientInsurances Dependencies ---------------------

------------  Start ClientLocations Dependencies ------------------



-- ClientLocationSpecialities
    DELETE FROM dbo.ClientLocationSpecialities
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- ClientLocationServiceTypes
    DELETE FROM dbo.ClientLocationServiceTypes
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- EmployeeClientLocations
    DELETE FROM dbo.EmployeeClientLocations
	WHERE 1=1 
	AND EmployeeClientId   IN(SELECT EmployeeClientId FROM @EmployeeClientIdsToRemove)

	-- ClientLocationServiceTypes
    DELETE FROM dbo.ClientLocations
	WHERE 1=1 
	AND ClientId = @DeleteClientId
	
	-- Addresses
	DELETE a 
	FROM dbo.Addresses a
	JOIN dbo.ClientLocations l on l.AddressId = a.Id
	WHERE l.ClientId = @DeleteClientId
	
------------  End ClientLocations Dependencies ---------------------
------------  Start ClientCptCodes Dependencies ------------------
-- UnmappedFeeScheduleCpts
	DELETE FROM dbo.UnmappedFeeScheduleCpts
	WHERE 1=1 
	AND (ClientInsuranceId IN (SELECT ClientInsuranceId FROM @ClientInsuranceIdsToRemove)
			OR ClientId = @DeleteClientId
			OR ClientCptCodeId in (SELECT ClientCptCodeId FROM @ClientCptCodesToRemove))

-- ClientFeeScheduleEntries
    DELETE FROM dbo.ClientFeeScheduleEntries
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- ClientCptCodes
    DELETE FROM dbo.ClientCptCodes
	WHERE 1=1 
	AND ClientId = @DeleteClientId

------------  End ClientCptCodes Dependencies ---------------------

------------  Start ClientFeeSchedules Dependencies ------------------

-- ClientInsuranceFeeSchedules
    DELETE FROM dbo.ClientInsuranceFeeSchedules
	WHERE 1=1 
	AND ClientInsuranceId IN (SELECT ClientInsuranceId FROM @ClientInsuranceIdsToRemove)

-- ClientFeeScheduleSpecialties
    DELETE cfssp FROM dbo.ClientFeeScheduleSpecialties cfssp
	JOIN dbo.ClientFeeSchedules cfs ON cfs.Id = cfssp.ClientFeeScheduleId
	WHERE 1=1 
	AND cfs.ClientId = @DeleteClientId

-- ClientFeeScheduleProviderLevels
    DELETE cfspl FROM dbo.ClientFeeScheduleProviderLevels cfspl
	JOIN dbo.ClientFeeSchedules cfs ON cfs.Id = cfspl.ClientFeeScheduleId
	WHERE 1=1 
	AND cfs.ClientId = @DeleteClientId

-- ClientFeeSchedules
    DELETE FROM dbo.ClientFeeSchedules
	WHERE 1=1 
	AND ClientId = @DeleteClientId

------------  End ClientFeeSchedules Dependencies ---------------------

-- ClientDocumentType
    DELETE FROM dbo.ClientDocumentType
	WHERE 1=1 
	AND ClientsId = @DeleteClientId

-- ClientAuthTypes
    DELETE FROM dbo.ClientAuthTypes
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- ClientApplicationFeatures
    DELETE FROM dbo.ClientApplicationFeatures
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- ClientApiIntegrationKeys
    DELETE FROM dbo.ClientApiIntegrationKeys
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- ClientAdjustmentCodes
    -- DELETE FROM IntegratedServices.ClientAdjustmentCodes
	-- WHERE 1=1 
	-- AND ClientId = @DeleteClientId

-- ClaimStatusTransactionLineItemStatusChangẹs
    DELETE FROM IntegratedServices.ClaimStatusTransactionLineItemStatusChangẹs
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- Addresses
    DELETE FROM dbo.Addresses
	WHERE 1=1 
	AND ClientId = @DeleteClientId
-- ClaimStatusWorkStationNotes
DELETE FROM IntegratedServices.ClaimStatusWorkStationNotes
WHERE Id = @DeleteClientId

------------  Start ChargeEntryRpaConfigurations Dependencies ------------------

-- ChargeEntryTransactionHistories
    DELETE FROM IntegratedServices.ChargeEntryTransactionHistories
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- ChargeEntryTransactions
    DELETE FROM IntegratedServices.ChargeEntryTransactions
	WHERE 1=1 
	AND ClientId = @DeleteClientId

-- ChargeEntryRpaConfigurations
    DELETE FROM IntegratedServices.ChargeEntryRpaConfigurations
	WHERE 1=1 
	AND ClientId = @DeleteClientId

------------  End ChargeEntryRpaConfigurations Dependencies ---------------------

--Clients
DELETE FROM dbo.Clients
WHERE Id = @DeleteClientId

GO


--SELECT bc.ClientId as 'BCClientID', P.ClientId AS 'P.cLIENTiD', pat.ClientId as 'Pat.ClientId', * FROM IntegratedServices.ClaimStatusBatchClaims bc
--JOIN dbo.Patients pat ON pat.Id = bc.PatientId
--JOIN dbo.Persons p ON pat.PersonId = p.Id
--WHERE bc.ClientId <> p.ClientId


--SELECT bc.ClientId as 'BCClientID', P.ClientId AS 'P.cLIENTiD',* FROM IntegratedServices.ClaimStatusBatchClaims bc
--JOIN dbo.Patients p ON bc.PatientId = p.Id
--WHERE bc.ClientId <> p.ClientId

--SELECT bc.ClientId as 'BCClientID', P.ClientId AS 'P.cLIENTiD',* FROM IntegratedServices.ClaimStatusBatchClaims bc
--JOIN dbo.Providers p ON bc.ClientProviderId = p.Id
--WHERE bc.ClientId <> p.ClientId

--SELECT * FROM dbo.Clients WHERE id = 20

--SELECT * FROM dbo.ClientInsurances i
--WHERE i.ClientId = 43

--SELECT * FROM IntegratedServices.ClientInsuranceRpaConfigurations  i
--WHERE 1=1

--DELETE rpa FROM IntegratedServices.ClientInsuranceRpaConfigurations rpa
--JOIN dbo.ClientInsurances i ON i.Id = rpa.ClientInsuranceId
--WHERE i.ClientId = 43

--DELETE bh FROM IntegratedServices.ClaimStatusBatchHistories bh
--WHERE AssignedClientRpaConfigurationId in (Select rpa.Id FROM IntegratedServices.ClientInsuranceRpaConfigurations rpa
--											JOIN dbo.ClientInsurances i ON i.Id = rpa.ClientInsuranceId
--											WHERE i.ClientId = 43)

--DELETE FROM dbo.ClientInsurances 
--WHERE ClientId = 43

DELETE FROM IntegratedServices.ClaimStatusBatchHistories 
WHERE ClientId = 35