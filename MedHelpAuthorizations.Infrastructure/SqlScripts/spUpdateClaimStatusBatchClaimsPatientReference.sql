SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [IntegratedServices].[spUpdateClaimStatusBatchClaimsPatientReference]
AS
BEGIN
    DECLARE @PatientFirstName NVARCHAR(100);
    DECLARE @PatientLastName NVARCHAR(100);
    DECLARE @DateOfBirth DATE;
    DECLARE @ClientId INT;
    DECLARE @PersonId INT;
    DECLARE @PatientId INT;

    DECLARE cursorPatients CURSOR FOR
    SELECT DISTINCT PatientFirstName, PatientLastName, DateOfBirth, ClientId
    FROM IntegratedServices.ClaimStatusBatchClaims

    OPEN cursorPatients;
    FETCH NEXT FROM cursorPatients INTO @PatientFirstName, @PatientLastName, @DateOfBirth, @ClientId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @PersonId = NULL;
        SET @PatientId = NULL;

        -- Check if Person already exists
        SELECT @PersonId = Id
        FROM Persons
        WHERE FirstName = @PatientFirstName
            AND LastName = @PatientLastName
            AND DateOfBirth = @DateOfBirth;

        IF @PersonId IS NULL
        BEGIN
            -- Insert new Person
            INSERT INTO Persons (FirstName, LastName, DateOfBirth, CreatedOn)
            VALUES (@PatientFirstName, @PatientLastName, @DateOfBirth, GETDATE());

            SET @PersonId = SCOPE_IDENTITY();
        END

        -- Check if Patient already exists
        SELECT @PatientId = Id
        FROM Patients
        WHERE PersonId = @PersonId;

        IF @PatientId IS NULL
        BEGIN
            -- Insert new Patient
            INSERT INTO Patients (ClientId, CreatedOn, AdministrativeGenderId, ResponsiblePartyRelationshipToPatient, PersonId)
            VALUES (@ClientId, GETDATE(), 0, 1, @PersonId);

            SET @PatientId = SCOPE_IDENTITY();
        END

        -- Update PatientId in ClaimStatusBatchClaims
        UPDATE IntegratedServices.ClaimStatusBatchClaims
        SET PatientId = @PatientId
        WHERE PatientFirstName = @PatientFirstName
            AND PatientLastName = @PatientLastName
            AND DateOfBirth = @DateOfBirth
            AND ClientId = @ClientId

        FETCH NEXT FROM cursorPatients INTO @PatientFirstName, @PatientLastName, @DateOfBirth, @ClientId;
    END

    CLOSE cursorPatients;
    DEALLOCATE cursorPatients;
END
