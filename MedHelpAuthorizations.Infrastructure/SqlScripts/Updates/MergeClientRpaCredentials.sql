-- Step 1: Create a temporary table to hold combined data with an identity column
IF OBJECT_ID('tempdb..#CombinedCredentials') IS NOT NULL
    DROP TABLE #CombinedCredentials;

CREATE TABLE #CombinedCredentials (
    TempId INT IDENTITY(1,1) NOT NULL PRIMARY KEY, -- Auto-incrementing identity column
    SourceId INT NOT NULL,
    RpaInsuranceGroupId INT,
    Username NVARCHAR(255),
    Password NVARCHAR(255),
    FailureMessage NVARCHAR(MAX),
    ReportFailureToEmail NVARCHAR(255),
    ExpiryWarningReported BIT,
    IsCredentialInUse BIT,
    UseOffHoursOnly BIT,
    OtpForwardFromEmail NVARCHAR(255),
    CreatedBy NVARCHAR(255),
    CreatedOn DATETIME2,
    LastModifiedBy NVARCHAR(255),
    LastModifiedOn DATETIME2,
    FailureReported BIT,
    ConversionId INT,
    SourceDB NVARCHAR(50), -- To distinguish between data from different databases
    ClientId INT,
    TenantIdentifier NVARCHAR(100), -- New column for Tenant Identifier
    UserId NVARCHAR(255), -- New column for UserId,
    ApiKey NVARCHAR(255) -- New column for ApiKey
);

-- Step 2: Insert data from faDevDb.ClientRpaCredentialConfigurations
USE faDevDb;  

WITH UniqueData AS (
    SELECT
        crcc.Id, crcc.RpaInsuranceGroupId, crcc.Username, crcc.Password, crcc.FailureMessage, crcc.ReportFailureToEmail, 
        crcc.ExpiryWarningReported, crcc.IsCredentialInUse, crcc.UseOffHoursOnly, crcc.OtpForwardFromEmail, crcc.CreatedBy, 
        crcc.CreatedOn, crcc.LastModifiedBy, crcc.LastModifiedOn, crcc.FailureReported, crcc.Id AS ConversionId, 'faDevDb' AS SourceDB,
        ci.ClientId, t.Identifier AS TenantIdentifier, uc.UserId,
        cak.ApiKey, -- Retrieve ApiKey from ClientApiIntegrationKeys
        ROW_NUMBER() OVER (PARTITION BY crcc.Id ORDER BY crcc.Id) AS RowNum
    FROM dbo.ClientRpaCredentialConfigurations crcc
    LEFT JOIN IntegratedServices.ClientInsuranceRpaConfigurations circ ON circ.ClientRpaCredentialConfigurationId = crcc.Id
    LEFT JOIN dbo.ClientInsurances ci ON ci.Id = circ.ClientInsuranceId
    LEFT JOIN UserClients uc ON uc.ClientId = ci.ClientId
    LEFT JOIN faIdentityDb.MultiTenancy.TenantUsers tu ON tu.UserId = uc.UserId
    LEFT JOIN faIdentityDb.MultiTenancy.Tenants t ON t.Id = tu.TenantId
    LEFT JOIN dbo.ClientApiIntegrationKeys cak ON cak.ClientId = ci.ClientId
)
INSERT INTO #CombinedCredentials (SourceId, RpaInsuranceGroupId, Username, Password, FailureMessage, ReportFailureToEmail, ExpiryWarningReported,
IsCredentialInUse, UseOffHoursOnly, OtpForwardFromEmail, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, FailureReported, ConversionId, SourceDB, ClientId, TenantIdentifier, UserId, ApiKey)
SELECT Id, RpaInsuranceGroupId, Username, Password, FailureMessage, ReportFailureToEmail, ExpiryWarningReported, IsCredentialInUse, 
UseOffHoursOnly, OtpForwardFromEmail, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, FailureReported, ConversionId, SourceDB, ClientId, TenantIdentifier, UserId, ApiKey
FROM UniqueData
WHERE RowNum = 1 AND ClientId IS NOT NULL;

-- Step 3: Insert data from faDbSharedTenantDb.ClientRpaCredentialConfigurations
USE faDbSharedTenantDb;

WITH UniqueData AS (
    SELECT
        crcc.Id, crcc.RpaInsuranceGroupId, crcc.Username, crcc.Password, crcc.FailureMessage, crcc.ReportFailureToEmail, 
        crcc.ExpiryWarningReported, crcc.IsCredentialInUse, crcc.UseOffHoursOnly, crcc.OtpForwardFromEmail, crcc.CreatedBy, 
        crcc.CreatedOn, crcc.LastModifiedBy, crcc.LastModifiedOn, crcc.FailureReported, crcc.Id AS ConversionId, 
        'faDbSharedTenantDb' AS SourceDB, ci.ClientId, t.Identifier AS TenantIdentifier, tu.UserId,
        cak.ApiKey, -- Retrieve ApiKey from ClientApiIntegrationKeys
        ROW_NUMBER() OVER (PARTITION BY crcc.Id ORDER BY crcc.Id) AS RowNum
    FROM dbo.ClientRpaCredentialConfigurations crcc
    LEFT JOIN IntegratedServices.ClientInsuranceRpaConfigurations circ ON circ.ClientRpaCredentialConfigurationId = crcc.Id
    LEFT JOIN dbo.ClientInsurances ci ON ci.Id = circ.ClientInsuranceId
    LEFT JOIN UserClients uc ON uc.ClientId = ci.ClientId
    LEFT JOIN faIdentityDb.MultiTenancy.TenantUsers tu ON tu.UserId = uc.UserId
    LEFT JOIN faIdentityDb.MultiTenancy.Tenants t ON t.Id = tu.TenantId
    LEFT JOIN dbo.ClientApiIntegrationKeys cak ON cak.ClientId = ci.ClientId
)
INSERT INTO #CombinedCredentials (SourceId, RpaInsuranceGroupId, Username, Password, FailureMessage, ReportFailureToEmail, 
ExpiryWarningReported, IsCredentialInUse, UseOffHoursOnly, OtpForwardFromEmail, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn,
FailureReported, ConversionId, SourceDB, ClientId, TenantIdentifier, UserId, ApiKey)
SELECT Id, RpaInsuranceGroupId, Username, Password, FailureMessage, ReportFailureToEmail, ExpiryWarningReported, IsCredentialInUse,
UseOffHoursOnly, OtpForwardFromEmail, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, FailureReported, ConversionId, SourceDB, ClientId, TenantIdentifier, UserId, ApiKey
FROM UniqueData
WHERE RowNum = 1 AND ClientId IS NOT NULL;

-- Step 4: Insert combined data into AITCredentialManager
USE AITCredentialManager;

SET IDENTITY_INSERT dbo.ClientRpaCredentialConfigurations ON;

INSERT INTO dbo.ClientRpaCredentialConfigurations (Id, RpaInsuranceGroupId, Username, Password, FailureMessage, ReportFailureToEmail, ExpiryWarningReported, IsCredentialInUse, UseOffHoursOnly, OtpForwardFromEmail, FailureReported, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, ConversionId, ClientId, TenantIdentifier, ApiKey)
SELECT TempId, RpaInsuranceGroupId, Username, Password, FailureMessage, ReportFailureToEmail, ExpiryWarningReported, IsCredentialInUse, UseOffHoursOnly, OtpForwardFromEmail, FailureReported, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, ConversionId, 1 AS ClientId, TenantIdentifier, ApiKey
FROM #CombinedCredentials;

SET IDENTITY_INSERT dbo.ClientRpaCredentialConfigurations OFF;

-- Clean up temporary tables
DROP TABLE #CombinedCredentials;


