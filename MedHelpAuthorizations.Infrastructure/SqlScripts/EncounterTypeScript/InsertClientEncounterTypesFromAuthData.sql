-- Step: Insert data into ClientEncounterTypes based on ClientAuthTypes and AuthTypes.
INSERT INTO dbo.ClientEncounterTypes (ClientId, Name, Description, OldClientAuthTypeId, OldAuthTypeId, CreatedOn, LastModifiedOn)
SELECT
    CAT.ClientId,              -- ClientId from ClientAuthTypes
    AT.Name,                   -- Name from AuthTypes
    AT.Description,            -- Description from AuthTypes
    CAT.Id AS OldClientAuthTypeId,   -- OldClientAuthTypeId from ClientAuthTypes
    AT.Id AS OldAuthTypeId,          -- OldAuthTypeId from AuthTypes
    GETDATE(),                 -- Current date and time for CreatedOn
    GETDATE()                  -- Current date and time for LastModifiedOn
FROM
    ClientAuthTypes CAT
JOIN
    AuthTypes AT
ON
    CAT.AuthTypeId = AT.Id     -- Match AuthTypeId in ClientAuthTypes to Id in AuthTypes
WHERE
    NOT EXISTS (
        -- Avoid duplicates by checking if an entry with the same OldClientAuthTypeId and OldAuthTypeId exists in ClientEncounterTypes
        SELECT 1 
        FROM dbo.ClientEncounterTypes CET
        WHERE CET.OldClientAuthTypeId = CAT.Id
          AND CET.OldAuthTypeId = AT.Id
    );