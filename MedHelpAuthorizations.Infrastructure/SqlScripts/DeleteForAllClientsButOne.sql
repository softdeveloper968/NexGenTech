DECLARE @KeepClientId int = 35
DECLARE @DefaultClientId int = 39

-- DELETE Claims/.Batches/Transaction and related

DECLARE @IdsToRemove TABLE(
    BatchId int NOT NULL,
    BatchClaimId int NOT NULL, 
    TransactionId int NULL
);
 
INSERT INTO @IdsToRemove (BatchId,BatchClaimId, TransactionId)
Select bc.ClaimStatusBatchID, bc.Id, bc.ClaimStatusTransactionId FROM IntegratedServices.ClaimStatusBatchClaims bc
JOIN IntegratedServices.ClaimStatusTransactions tx ON tx.Id = bc.ClaimStatusTransactionId
WHERE 1=1
AND bc.ClientId NOT IN (@KeepClientId, @DefaultClientId)
 
--SELECT * FROM @IdsToRemove
 
--- DELETE TransactionHx for BatchId and lineStatus ---
Print 'ClaimStatusTransactionHistories'
DELETE FROM IntegratedServices.ClaimStatusTransactionHistories
WHERE ClaimStatusTransactionId in (SELECT TransactionId
FROM @IdsToRemove)
 
 Print 'ClaimStatusBatchClaim'
--- DETACH ClaimStatusBatchClaim ---
UPDATE IntegratedServices.ClaimStatusBatchClaims
SET ClaimStatusTransactionId = null
FROM IntegratedServices.ClaimStatusBatchClaims bc
WHERE bc.ClaimStatusTransactionId In (SELECT TransactionId
FROM @IdsToRemove)
 
 Print 'Transactions'
--- DELETE Claim Status Transactions ---
DELETE FROM IntegratedServices.ClaimStatusTransactions
WHERE Id IN (SELECT TransactionId
FROM @IdsToRemove)
 
 Print 'ClaimStatusTransactionLineItemStatusChangẹs'
 --- DELETE Claim Status Last Status Change ---
DELETE FROM IntegratedServices.[ClaimStatusTransactionLineItemStatusChangẹs]
WHERE Id IN (SELECT tx.ClaimStatusTransactionLineItemStatusChangẹId FROM IntegratedServices.ClaimStatusTransactions tx Where id IN (SELECT TransactionId
FROM @IdsToRemove))

Print 'ClaimStatusBatchClaims'
--- DELETE Claim Status Batch Claims ---
DELETE FROM IntegratedServices.ClaimStatusBatchClaims
WHERE Id IN (SELECT BatchClaimId
FROM @IdsToRemove)

Print 'ClaimStatusBatchHistories'
--- DELETE Batch ---
	DELETE bh FROM IntegratedServices.ClaimStatusBatchHistories bh
	WHERE AssignedClientRpaConfigurationId in (Select rpa.Id FROM IntegratedServices.ClientInsuranceRpaConfigurations rpa
												JOIN dbo.ClientInsurances i ON i.Id = rpa.ClientInsuranceId
								 				WHERE i.ClientId NOT IN (@KeepClientId, @DefaultClientId))

Print 'ClaimStatusBatchHistories'
DELETE FROM IntegratedServices.ClaimStatusBatchHistories
WHERE ClaimStatusBatchId IN (SELECT batchId
FROM @IdsToRemove)
 
 Print 'ClaimStatusBatches'
--- DELETE Batch ---
DELETE FROM IntegratedServices.ClaimStatusBatches
WHERE Id IN (SELECT batchId
FROM @IdsToRemove)
 
 
 
 Print ''
 --- DELETE All THe rest 
--- ProviderId's
DECLARE @ProviderIdsToRemove TABLE(
    ProviderId int NOT NULL
);

INSERT INTO @ProviderIdsToRemove (ProviderId)
Select prv.Id FROM dbo.Providers prv
WHERE prv.ClientId NOT IN (@KeepClientId, @DefaultClientId)

--SELECT * FROM @ProviderIdsToRemove
--- 
DECLARE @EmployeeClientIdsToRemove TABLE(
    EmployeeClientId int NOT NULL
);
INSERT INTO @EmployeeClientIdsToRemove (EmployeeClientId)
Select empcl.Id FROM dbo.EmployeeClients empcl
WHERE empcl.ClientId NOT IN (@KeepClientId, @DefaultClientId)

DECLARE @ClientInsuranceIdsToRemove TABLE(
    ClientInsuranceId int NOT NULL
);
INSERT INTO @ClientInsuranceIdsToRemove (ClientInsuranceId)
Select ins.Id FROM dbo.ClientInsurances ins 
WHERE ins.ClientId NOT IN (@KeepClientId, @DefaultClientId)

DECLARE @ClientCptCodesToRemove TABLE(
    ClientCptCodeId int NOT NULL
);
INSERT INTO @ClientCptCodesToRemove (ClientCptCodeId)
Select cpt.Id FROM dbo.ClientCptCodes cpt 
WHERE cpt.ClientId NOT IN (@KeepClientId, @DefaultClientId)


DECLARE @AuthorizationIdsToRemove TABLE(
    AuthorizationId int NOT NULL
);
INSERT INTO @AuthorizationIdsToRemove (AuthorizationId)
Select a.Id FROM dbo.Authorizations a 
WHERE a.ClientId NOT IN (@KeepClientId, @DefaultClientId)


-- Authorizations
Print 'Documents'
	DELETE FROM dbo.Documents
	WHERE 1=1 
	AND AuthorizationId IN (SELECT AuthorizationId FROM @AuthorizationIdsToRemove)
Print 'AuthorizationClientCptCode'
	DELETE FROM dbo.AuthorizationClientCptCode
	WHERE 1=1 
	AND AuthorizationId IN (SELECT AuthorizationId FROM @AuthorizationIdsToRemove)
Print 'Notes'
	DELETE FROM dbo.Notes
	WHERE 1=1 
	AND AuthorizationId IN (SELECT AuthorizationId FROM @AuthorizationIdsToRemove)
Print 'Authorizations'
	DELETE FROM dbo.Authorizations
	WHERE 1=1 
	AND Id IN (SELECT AuthorizationId FROM @AuthorizationIdsToRemove)

Print 'UserClients'
-- UserClients
	DELETE FROM dbo.UserClients
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'InsuranceCards'
-- Insurance Cards
	DELETE FROM dbo.InsuranceCards
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'Messages'
-- Messages
    DELETE FROM dbo.Messages
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

------------  Start EmployeeClients Dependencies ------------------
-- ClientEmployeeRoles
Print 'ClientEmployeeRoles'
    DELETE FROM dbo.ClientEmployeeRoles
	WHERE 1=1 
	AND EmployeeClientId IN (SELECT EmployeeClientId FROM @EmployeeClientIdsToRemove)

Print 'EmployeeClientAlphaSplits'
-- EmployeeClientAlphaSplits
    DELETE FROM dbo.EmployeeClientAlphaSplits
	WHERE 1=1 
	AND EmployeeClientId   IN (SELECT EmployeeClientId FROM @EmployeeClientIdsToRemove)

Print 'EmployeeClientInsurances'
	-- EmployeeClientAlphaSplits
    DELETE FROM dbo.EmployeeClientInsurances
	WHERE 1=1 
	AND EmployeeClientId   IN (SELECT EmployeeClientId FROM @EmployeeClientIdsToRemove)

Print 'EmployeeClients'
-- EmployeeClients
    DELETE FROM dbo.EmployeeClients
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

------------  End EmployeeClients Dependencies ---------------------


------------  Start ClientUserReportFilter Dependencies ------------------

Print 'EmployeeClientUserReportFilter'
-- EmployeeClientUserReportFilter
    DELETE FROM dbo.EmployeeClientUserReportFilter
	WHERE 1=1 
	AND EmployeeClientId   IN(SELECT EmployeeClientId FROM @EmployeeClientIdsToRemove)

Print 'ClientUserReportFilter'
-- ClientUserReportFilter
    DELETE FROM dbo.ClientUserReportFilter
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

------------  End ClientUserReportFilter Dependencies ---------------------

Print 'ClientSpecialties'
-- ClientSpecialties
    DELETE FROM dbo.ClientSpecialties
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'ClientPlacesOfService'
-- ClientPlacesOfService
    DELETE FROM dbo.ClientPlacesOfService
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)


Print 'Clients'
	Update dbo.Clients
	SET ClientKpiId = null
	WHERE Id NOT IN (@KeepClientId, @DefaultClientId)

Print 'ClientKpi'
-- ClientKpi
    DELETE FROM dbo.ClientKpi
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'InputDocuments'
-- InputDocuments
    DELETE FROM IntegratedServices.InputDocuments
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

	------------  Start Persons Dependencies ------------------
-- ClientProviderLocations
Print 'ClientProviderLocations'
    DELETE FROM dbo.ClientProviderLocations
	WHERE 1=1 
	AND ClientProviderId IN (SELECT ClientProviderId FROM @ProviderIdsToRemove)

 --Patients
 --UPDATE dbo.Patients 
 --SET ClientId =  @KeepClientId
 --WHERE ClientId NOT IN (@KeepClientId, @DefaultClientId)
Print 'Patients'
    DELETE pat FROM dbo.Patients pat
	JOIN IntegratedServices.ClaimStatusBatchClaims bc ON bc.PatientId = pat.Id
	WHERE 1=1 
	AND bc.ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'Patients'
	DELETE pat FROM dbo.Patients pat
	WHERE 1=1 
	AND pat.ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'Cardholders'
-- Cardholders
    DELETE FROM dbo.Cardholders
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'Providers'
-- Providers
    DELETE FROM dbo.Providers
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

-- *Persons
	--UPDATE dbo.Persons 
	-- SET ClientId =  @KeepClientId
	-- WHERE ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'Persons'
    DELETE FROM dbo.Persons
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)
	
------------  End Persons Dependencies ---------------------
------------  Start ClientInsurances Dependencies ------------------
Print 'ClaimStatusTransactionHistories'
-- ClaimStatusTransactionHistories
    DELETE FROM IntegratedServices.ClaimStatusTransactionHistories
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'ClaimStatusTransactions'
-- ClaimStatusTransactions
    DELETE FROM IntegratedServices.ClaimStatusTransactions
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'ClaimStatusBatchClaims'
-- ClaimStatusBatchClaims
    DELETE FROM IntegratedServices.ClaimStatusBatchClaims
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'ClaimStatusBatchHistories'
-- ClaimStatusBatchHistories
	DELETE bh FROM IntegratedServices.ClaimStatusBatchHistories bh
	WHERE AssignedClientRpaConfigurationId in (Select rpa.Id FROM IntegratedServices.ClientInsuranceRpaConfigurations rpa
												JOIN dbo.ClientInsurances i ON i.Id = rpa.ClientInsuranceId
												WHERE i.ClientId NOT IN (@KeepClientId, @DefaultClientId))
Print 'ClaimStatusBatchHistories'
    DELETE FROM IntegratedServices.ClaimStatusBatchHistories
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

-- ClaimStatusBatches
Print 'ClaimStatusBatches'
DELETE b FROM IntegratedServices.ClaimStatusBatches b
	WHERE AssignedClientRpaConfigurationId in (Select rpa.Id FROM IntegratedServices.ClientInsuranceRpaConfigurations rpa
												JOIN dbo.ClientInsurances i ON i.Id = rpa.ClientInsuranceId
												WHERE i.ClientId NOT IN (@KeepClientId, @DefaultClientId))

Print 'ClaimStatusBatches'
    DELETE FROM IntegratedServices.ClaimStatusBatches
	WHERE 1=1 
	AND (ClientId NOT IN (@KeepClientId, @DefaultClientId) 
		OR ClientInsuranceId   IN (SELECT ClientInsuranceId FROM @ClientInsuranceIdsToRemove))

Print 'ClientInsuranceRpaConfigurations'
-- ClientInsuranceRpaConfigurations
    DELETE FROM IntegratedServices.ClientInsuranceRpaConfigurations
	WHERE 1=1 
	AND ClientInsuranceId   IN (SELECT ClientInsuranceId FROM @ClientInsuranceIdsToRemove)

Print 'ClientInsuranceAverageCollectionPercentages'
-- ClientInsuranceAverageCollectionPercentages
    DELETE FROM dbo.ClientInsuranceAverageCollectionPercentages
	WHERE 1=1 
	AND ClientInsuranceId IN (SELECT ClientInsuranceId FROM @ClientInsuranceIdsToRemove)

Print 'ClientLocationInsuranceIdentifiers'
-- ClientLocationInsuranceIdentifiers
    DELETE FROM dbo.ClientLocationInsuranceIdentifiers
	WHERE 1=1 
	AND ClientInsuranceId IN (SELECT ClientInsuranceId FROM @ClientInsuranceIdsToRemove)

Print 'ClientInsurances'
-- ClientInsurances
    DELETE FROM dbo.ClientInsurances
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

------------  End ClientInsurances Dependencies ---------------------

------------  Start ClientLocations Dependencies ------------------



-- ClientLocationSpecialities
Print 'ClientLocationSpecialities'
    DELETE FROM dbo.ClientLocationSpecialities
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

-- ClientLocationServiceTypes
Print 'ClientLocationServiceTypes'
    DELETE FROM dbo.ClientLocationServiceTypes
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'EmployeeClientLocations'
-- EmployeeClientLocations
    DELETE FROM dbo.EmployeeClientLocations
	WHERE 1=1 
	AND EmployeeClientId   IN(SELECT EmployeeClientId FROM @EmployeeClientIdsToRemove)

Print 'ClientLocations'
	-- ClientLocationServiceTypes
    DELETE FROM dbo.ClientLocations
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)
	
Print 'Addresses'
	-- Addresses
	DELETE a 
	FROM dbo.Addresses a
	JOIN dbo.ClientLocations l on l.AddressId = a.Id
	WHERE l.ClientId NOT IN (@KeepClientId, @DefaultClientId)
	
------------  End ClientLocations Dependencies ---------------------
------------  Start ClientCptCodes Dependencies ------------------
-- UnmappedFeeScheduleCpts
Print 'UnmappedFeeScheduleCpts'
	DELETE FROM dbo.UnmappedFeeScheduleCpts
	WHERE 1=1 
	AND (ClientInsuranceId IN (SELECT ClientInsuranceId FROM @ClientInsuranceIdsToRemove)
			OR ClientId NOT IN (@KeepClientId, @DefaultClientId)
			OR ClientCptCodeId in (SELECT ClientCptCodeId FROM @ClientCptCodesToRemove))

Print 'ClientFeeScheduleEntries'
-- ClientFeeScheduleEntries
    DELETE FROM dbo.ClientFeeScheduleEntries
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'ClientCptCodes'
-- ClientCptCodes
    DELETE FROM dbo.ClientCptCodes
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

------------  End ClientCptCodes Dependencies ---------------------

------------  Start ClientFeeSchedules Dependencies ------------------

-- ClientInsuranceFeeSchedules
Print 'ClientInsuranceFeeSchedules'
    DELETE FROM dbo.ClientInsuranceFeeSchedules
	WHERE 1=1 
	AND ClientInsuranceId IN (SELECT ClientInsuranceId FROM @ClientInsuranceIdsToRemove)

Print 'ClientFeeScheduleSpecialties'
-- ClientFeeScheduleSpecialties
    DELETE cfssp FROM dbo.ClientFeeScheduleSpecialties cfssp
	JOIN dbo.ClientFeeSchedules cfs ON cfs.Id = cfssp.ClientFeeScheduleId
	WHERE 1=1 
	AND cfs.ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'ClientFeeScheduleProviderLevels'
-- ClientFeeScheduleProviderLevels
    DELETE cfspl FROM dbo.ClientFeeScheduleProviderLevels cfspl
	JOIN dbo.ClientFeeSchedules cfs ON cfs.Id = cfspl.ClientFeeScheduleId
	WHERE 1=1 
	AND cfs.ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'ClientFeeSchedules'
-- ClientFeeSchedules
    DELETE FROM dbo.ClientFeeSchedules
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

------------  End ClientFeeSchedules Dependencies ---------------------

Print 'ClientDocumentType'
-- ClientDocumentType
    DELETE FROM dbo.ClientDocumentType
	WHERE 1=1 
	AND ClientsId NOT IN (@KeepClientId, @DefaultClientId)

Print 'ClientAuthTypes'
-- ClientAuthTypes
    DELETE FROM dbo.ClientAuthTypes
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'ClientApplicationFeatures'
-- ClientApplicationFeatures
    DELETE FROM dbo.ClientApplicationFeatures
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'ClientApiIntegrationKeys'
-- ClientApiIntegrationKeys
    DELETE FROM dbo.ClientApiIntegrationKeys
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

-- ClientAdjustmentCodes
    -- DELETE FROM IntegratedServices.ClientAdjustmentCodes
	-- WHERE 1=1 
	-- AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'ClaimStatusTransactionLineItemStatusChangẹs'
-- ClaimStatusTransactionLineItemStatusChangẹs
    DELETE FROM IntegratedServices.ClaimStatusTransactionLineItemStatusChangẹs
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'Addresses'
-- Addresses
    DELETE FROM dbo.Addresses
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)
-- ClaimStatusWorkStationNotes
Print 'ClaimStatusWorkStationNotes'
DELETE FROM IntegratedServices.ClaimStatusWorkStationNotes
WHERE Id NOT IN (@KeepClientId, @DefaultClientId)

------------  Start ChargeEntryRpaConfigurations Dependencies ------------------

-- ChargeEntryTransactionHistories
Print 'ChargeEntryTransactionHistories'
    DELETE FROM IntegratedServices.ChargeEntryTransactionHistories
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

Print 'ChargeEntryTransactions'
-- ChargeEntryTransactions
    DELETE FROM IntegratedServices.ChargeEntryTransactions
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

-- ChargeEntryRpaConfigurations
Print 'ChargeEntryRpaConfigurations'
    DELETE FROM IntegratedServices.ChargeEntryRpaConfigurations
	WHERE 1=1 
	AND ClientId NOT IN (@KeepClientId, @DefaultClientId)

------------  End ChargeEntryRpaConfigurations Dependencies ---------------------

--Clients
Print 'Clients'
DELETE FROM dbo.Clients
WHERE Id NOT IN (@KeepClientId, @DefaultClientId)

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