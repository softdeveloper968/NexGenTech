USE AITCredentialManager;

-- Step 1: Create a temporary table to hold the ApiKey and ClientId mapping
IF OBJECT_ID('tempdb..#ApiKeyClientMapping') IS NOT NULL
    DROP TABLE #ApiKeyClientMapping;

CREATE TABLE #ApiKeyClientMapping (
    ApiKey NVARCHAR(255),
    ClientId INT
);

-- Step 2: Insert data into the temporary table from ClientApiKeys
INSERT INTO #ApiKeyClientMapping (ApiKey, ClientId)
SELECT ApiKey, ClientId
FROM dbo.ClientApiKeys
WHERE IsActive = 1;

-- Step 3: Update ClientId in ClientRpaCredentialConfigurations based on ApiKey match
USE AITCredentialManager;
UPDATE crc
SET crc.ClientId = akm.ClientId
FROM [AITCredentialManager].[dbo].[ClientRpaCredentialConfigurations] crc
JOIN #ApiKeyClientMapping akm
ON crc.ApiKey = akm.ApiKey;

-- Clean up temporary table
DROP TABLE #ApiKeyClientMapping;
