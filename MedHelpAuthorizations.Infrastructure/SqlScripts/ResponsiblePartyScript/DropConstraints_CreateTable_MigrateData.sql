-- Step 1: Check if the table exists and drop Foreign Key Constraints
DECLARE @ForeignKeyName NVARCHAR(128),
        @ParentTable NVARCHAR(128),
        @ForeignKeySql NVARCHAR(MAX);

-- Create a cursor to iterate over the foreign key constraints
DECLARE fk_cursor CURSOR FOR
SELECT 
    fk.name AS ForeignKeyName,
    tp.name AS ParentTable
FROM 
    sys.foreign_keys AS fk
INNER JOIN 
    sys.foreign_key_columns AS fkc 
    ON fk.object_id = fkc.constraint_object_id
INNER JOIN 
    sys.tables AS tp 
    ON fkc.parent_object_id = tp.object_id
INNER JOIN 
    sys.tables AS tr 
    ON fkc.referenced_object_id = tr.object_id
INNER JOIN 
    sys.columns AS cr 
    ON fkc.referenced_object_id = cr.object_id AND fkc.referenced_column_id = cr.column_id
WHERE 
    cr.name = 'Id' AND tr.name = 'ResponsibleParties';

-- Drop Foreign Key Constraints
OPEN fk_cursor;
FETCH NEXT FROM fk_cursor INTO @ForeignKeyName, @ParentTable;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @ForeignKeySql = 'ALTER TABLE [' + @ParentTable + '] DROP CONSTRAINT [' + @ForeignKeyName + ']';
    EXEC sp_executesql @ForeignKeySql;
    FETCH NEXT FROM fk_cursor INTO @ForeignKeyName, @ParentTable;
END

CLOSE fk_cursor;
DEALLOCATE fk_cursor;

-- Step 2: Create a New Table with Correct Schema
CREATE TABLE dbo.ResponsibleParties_New (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AccountNumber VARCHAR(7) NULL,
    ExternalId NVARCHAR(25) NULL,
    SocialSecurityNumber DECIMAL(18,2) NULL,
	ClientId INT NOT NULL,
	PersonId INT NOT NULL,
    CreatedBy NVARCHAR(50) NULL,   
    CreatedOn DATETIME2 NULL,     
    LastModifiedBy NVARCHAR(50) NULL,   
    LastModifiedOn DATETIME2 NULL,
    DfExternalId NVARCHAR(450) NULL,       
    DfCreatedOn DATETIME2 NULL,   
    DfLastModifiedOn DATETIME2 NULL 
);
GO

-- Step 3: Migrate Data to the New Table
INSERT INTO dbo.ResponsibleParties_New 
    (AccountNumber, ExternalId, SocialSecurityNumber, PersonId, ClientId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, DfExternalId, DfCreatedOn, DfLastModifiedOn)
SELECT 
    AccountNumber, 
    ExternalId, 
    SocialSecurityNumber, 
    PersonId, 
    ClientId, 
    CreatedBy, 
    CreatedOn, 
    LastModifiedBy, 
    LastModifiedOn,
    DfExternalId,   
    DfCreatedOn,  
    DfLastModifiedOn 
FROM dbo.ResponsibleParties;
GO

-- Step 4: Create Mapping Table to Track Old and New IDs
IF OBJECT_ID('dbo.IdMapping', 'U') IS NOT NULL
    DROP TABLE dbo.IdMapping;
GO

SELECT OldId = rp.Id, NewId = rpn.Id
INTO dbo.IdMapping
FROM dbo.ResponsibleParties rp
JOIN dbo.ResponsibleParties_New rpn ON rp.AccountNumber = rpn.AccountNumber;
GO

-- Step 5: Update Foreign Key Columns in Related Tables

-- Update Patients table
UPDATE Patients
SET ResponsiblePartyId = im.NewId
FROM Patients p
JOIN dbo.IdMapping im ON p.ResponsiblePartyId = im.OldId;
GO

-- Update PatientLedgerCharges table
UPDATE PatientLedgerCharges
SET ResponsiblePartyId = im.NewId
FROM PatientLedgerCharges plc
JOIN dbo.IdMapping im ON plc.ResponsiblePartyId = im.OldId;
GO

-- Step 6: Drop the Old Table and Rename the New Table
DROP TABLE dbo.ResponsibleParties;
GO

EXEC sp_rename 'dbo.ResponsibleParties_New', 'ResponsibleParties';
GO