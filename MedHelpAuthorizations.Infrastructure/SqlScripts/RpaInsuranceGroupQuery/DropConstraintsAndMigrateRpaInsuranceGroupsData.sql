-- Drop Foreign Key Constraints related to RpaInsuranceGroups
-- Replace 'FK_RpaInsurances_RpaInsuranceGroups' and 'FK_ClientRpaCredentialConfigurations_RpaInsuranceGroups' with actual constraint names if they exist

-- Check if the constraint exists before attempting to drop
DECLARE @constraintName NVARCHAR(256);
-- For RpaInsurances
SELECT @constraintName = fk.name
FROM sys.foreign_keys AS fk
INNER JOIN sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
INNER JOIN sys.tables AS tp ON fk.parent_object_id = tp.object_id
INNER JOIN sys.columns AS cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
WHERE tp.name = 'RpaInsurances' AND cp.name = 'RpaInsuranceGroupId';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE [IntegratedServices].[RpaInsurances] DROP CONSTRAINT ' + @constraintName);
END;

-- For ClientRpaCredentialConfigurations
SELECT @constraintName = fk.name
FROM sys.foreign_keys AS fk
INNER JOIN sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
INNER JOIN sys.tables AS tp ON fk.parent_object_id = tp.object_id
INNER JOIN sys.columns AS cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
WHERE tp.name = 'ClientRpaCredentialConfigurations' AND cp.name = 'RpaInsuranceGroupId';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE [dbo].[ClientRpaCredentialConfigurations] DROP CONSTRAINT ' + @constraintName);
END;
GO


-- Create a new table with the updated schema for RpaInsuranceGroups
CREATE TABLE [dbo].[RpaInsuranceGroups_New] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DefaultTargetUrl VARCHAR(MAX) NULL,
    Name NVARCHAR(Max) NULL,
    CreatedBy NVARCHAR(50) NULL,
    CreatedOn DATETIME2 NULL,
    LastModifiedBy NVARCHAR(50) NULL,
    LastModifiedOn DATETIME2 NULL
);
GO

-- Migrate data from the old table to the new table
INSERT INTO [dbo].[RpaInsuranceGroups_New] 
    (DefaultTargetUrl, Name, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT 
    DefaultTargetUrl, Name, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn
FROM dbo.RpaInsuranceGroups;
GO


-- Create a mapping table to track old and new IDs if required
IF OBJECT_ID('dbo.IdMapping', 'U') IS NOT NULL
    DROP TABLE IntegratedServices.IdMapping;
GO

-- Create IdMapping table
SELECT OldId = rpi.Id, NewId = rpin.Id
INTO IntegratedServices.IdMapping
FROM [dbo].[RpaInsuranceGroups] rpi
JOIN [dbo].[RpaInsuranceGroups_New] rpin ON rpi.Name = rpin.Name;
GO

-- Update RpaInsurances table
UPDATE [IntegratedServices].[RpaInsurances]
SET RpaInsuranceGroupId = im.NewId
FROM [IntegratedServices].[RpaInsurances] ri
JOIN IntegratedServices.IdMapping im ON ri.RpaInsuranceGroupId = im.OldId;
GO

-- Update ClientRpaCredentialConfigurations table
UPDATE [dbo].[ClientRpaCredentialConfigurations]
SET RpaInsuranceGroupId = im.NewId
FROM [dbo].[ClientRpaCredentialConfigurations] c
JOIN IntegratedServices.IdMapping im ON c.RpaInsuranceGroupId = im.OldId;
GO

-- Drop the old RpaInsuranceGroups table
DROP TABLE [dbo].[RpaInsuranceGroups];
GO

-- Rename the new table to the original name
EXEC sp_rename 'dbo.RpaInsuranceGroups_New', 'RpaInsuranceGroups';
GO