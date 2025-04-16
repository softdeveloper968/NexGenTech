-- Step 7: Recreate Foreign Key Constraints

-- Recreate foreign key constraint for Patients table
ALTER TABLE Patients
ADD CONSTRAINT FK_Patients_ResponsibleParties_ResponsiblePartyId
FOREIGN KEY (ResponsiblePartyId) REFERENCES dbo.ResponsibleParties(Id);
GO

-- Recreate foreign key constraint for PatientLedgerCharges table
ALTER TABLE PatientLedgerCharges
ADD CONSTRAINT FK_PatientLedgerCharges_ResponsibleParties_ResponsiblePartyId
FOREIGN KEY (ResponsiblePartyId) REFERENCES dbo.ResponsibleParties(Id);
GO


IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_ResponsibleParties_Clients_ClientId'
)
BEGIN
    ALTER TABLE [dbo].ResponsibleParties WITH CHECK ADD CONSTRAINT [FK_ResponsibleParties_Clients_ClientId] FOREIGN KEY([ClientId])
    REFERENCES [dbo].[Clients] ([Id]);
    

    ALTER TABLE [dbo].ResponsibleParties CHECK CONSTRAINT [FK_ResponsibleParties_Clients_ClientId];
    
END
ELSE
BEGIN
    PRINT 'Foreign key constraint FK_ResponsibleParties_Clients_ClientId already exists.';
END

-- Recreate foreign key constraint for Persons table if it does not exist
IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_ResponsibleParties_Persons_PersonId'
)
BEGIN
    ALTER TABLE [dbo].ResponsibleParties WITH CHECK ADD CONSTRAINT [FK_ResponsibleParties_Persons_PersonId] FOREIGN KEY([PersonId])
    REFERENCES [dbo].[Persons] ([Id])
    ON DELETE CASCADE;
    

    ALTER TABLE [dbo].ResponsibleParties CHECK CONSTRAINT [FK_ResponsibleParties_Persons_PersonId];
    
END


ALTER TABLE dbo.ResponsibleParties
DROP COLUMN AccountNumber;

ALTER TABLE dbo.ResponsibleParties
ADD AccountNumber AS (CHAR(65 + ID/260000) + CHAR(65 + ID%260000/10000) + RIGHT('0000' + CAST(ID % 10000 AS VARCHAR),4));